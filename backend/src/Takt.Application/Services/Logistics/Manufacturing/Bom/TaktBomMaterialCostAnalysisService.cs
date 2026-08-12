// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysisService.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析应用服务（转置 / 差异 / 月度涨跌；三页共用工厂/机种/物料级联选项）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using OfficeOpenXml;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析服务（转置 / 差异 / 月度涨跌；
/// 工厂→机种→物料级联选项供成本分析 / 产品推移 / 机种推移三页共用）
/// </summary>
public class TaktBomMaterialCostAnalysisService : TaktServiceBase, ITaktBomMaterialCostAnalysisService
{
    /// <summary>
    /// 转置/涨跌分析加载成本汇总行上限
    /// </summary>
    private const int MaxAnalysisRowLoad = 20000;

    /// <summary>BOM 成本明细按年分表基表名（与 SugarTable 一致）</summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    /// <summary>移动价格按年分表基表名</summary>
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    /// <summary>按 Id 探测年分表向前/后年数（含当年）</summary>
    private const int YearShardProbeYears = 6;

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktCompanyRepository<TaktCalendar> _calendarRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePrice> _purchasePriceRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceItem> _purchasePriceItemRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM物料成本汇总仓储（转置/涨跌分析数据源）</param>
    /// <param name="materialPlantRepository">工厂物料仓储（成本合计仅 FERT）</param>
    /// <param name="materialMovingPriceRepository">移动价格仓储（组件单价期间转置）</param>
    /// <param name="calendarRepository">工厂日历仓储（第 N 工作日判定）</param>
    /// <param name="purchasePriceRepository">采购价格主表仓储</param>
    /// <param name="purchasePriceItemRepository">采购价格明细仓储</param>
    /// <param name="supplierRepository">供应商仓储（回填采购组织）</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="companyRepository">公司仓储（读取 RelatedPlant）</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostAnalysisService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktCompanyRepository<TaktCalendar> calendarRepository,
        ITaktCompanyRepository<TaktPurchasePrice> purchasePriceRepository,
        ITaktCompanyRepository<TaktPurchasePriceItem> purchasePriceItemRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _materialPlantRepository = materialPlantRepository;
        _materialMovingPriceRepository = materialMovingPriceRepository;
        _calendarRepository = calendarRepository;
        _purchasePriceRepository = purchasePriceRepository;
        _purchasePriceItemRepository = purchasePriceItemRepository;
        _supplierRepository = supplierRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _companyRepository = companyRepository;
        _uniqueValidator = uniqueValidator;
    }

    // ========================================
    // 三页共用级联选项（工厂 → 机种 → 物料/产品）
    // ========================================

    /// <summary>
    /// 查询栏工厂选项（级联第 1 级）：当前公司 RelatedPlant ∩ 本表 PlantCode
    /// <para>非 TaktPlants 全量；无关联工厂或本表无该工厂数据时返回空列表。</para>
    /// </summary>
    /// <returns>下拉选项（通常 0～1 项；DictValue/DictLabel=PlantCode）</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisPlantOptionsAsync()
    {
        EnsureThreeLayerContext();
        // 仅当前公司关联工厂：TaktCompany.RelatedPlant ∩ 本表 PlantCode（非 TaktPlants 全量）
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
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
            new()
            {
                DictValue = relatedPlant,
                DictLabel = relatedPlant,
            },
        };
    }

    /// <summary>
    /// 查询栏物料类型去重选项（本表 MaterialType；须工厂）
    /// <para>返回该工厂下全部类型（FERT/HALB/…），不做默认截断；前端拉全量后再默认选中 FERT。</para>
    /// <para>❌ 非字典 logistics_material_type（CRUD 表单专用）。</para>
    /// </summary>
    /// <param name="queryDto">须 PlantCode</param>
    /// <returns>DictValue/DictLabel=MaterialType；PlantCode 空则空列表</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisMaterialTypeOptionsAsync(
        TaktBomMaterialCostAnalysisMaterialTypeOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plant = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        // 本表 MaterialType 全量去重（含 FERT/HALB/…）；❌ 不默认截成仅 FERT
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.MaterialType != null
                && x.MaterialType != string.Empty);
        return list
            .Select(e => e.MaterialType.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(t => t, StringComparer.Ordinal)
            .Select(t => new TaktSelectOption
            {
                DictValue = t,
                DictLabel = t,
            })
            .ToList();
    }

    /// <summary>
    /// 规范化物料类型筛选（空=不按类型过滤）
    /// </summary>
    /// <param name="materialType">查询传入类型</param>
    /// <returns>非空类型码；空则 null（不过滤）</returns>
    private static string? NormalizeMaterialTypeFilter(string? materialType)
    {
        var trimmed = materialType?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    /// <summary>
    /// 查询栏机种去重选项（本表 ModelCode；须工厂）
    /// <para>MaterialType 有值才按类型过滤，空=该工厂全部机种。DictLabel 优先型号目的地机种名。</para>
    /// <para>❌ 非 CRUD 主数据 TaktModelDestination / TaktBomMaterialCosts/model-options。</para>
    /// </summary>
    /// <param name="queryDto">机种选项查询（PlantCode 必填；MaterialType 可选）</param>
    /// <returns>下拉选项（DictValue=ModelCode；DictLabel=机种名或编码）</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisModelOptionsAsync(
        TaktBomMaterialCostAnalysisModelOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plant = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        // 本表 ModelCode 去重；MaterialType 有值才过滤，空=该工厂全部机种（含各类型）
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.ModelCode != null
            && x.ModelCode != string.Empty);
        if (materialType != null)
        {
            var type = materialType;
            exp = exp.And(x => x.MaterialType == type);
        }
        var list = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        var modelNameLookup = await BuildModelNameLookupAsync();
        return list
            .Where(e => materialType == null
                || string.Equals(e.MaterialType?.Trim(), materialType, StringComparison.OrdinalIgnoreCase))
            .GroupBy(e => e.ModelCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var modelCode = g.Key;
                var label = modelNameLookup.TryGetValue(modelCode, out var modelName) && !string.IsNullOrWhiteSpace(modelName)
                    ? modelName
                    : modelCode;
                return new TaktSelectOption
                {
                    DictValue = modelCode,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 查询栏产品编码去重选项（本表 ProductCode；须工厂）
    /// <para>仅 TaktBomMaterialCost.ProductCode；❌ 不读 TaktMaterialPlant、不读字典 logistics_material_type。</para>
    /// <para>MaterialType / ModelCode 均可空；空则不按该维过滤。类型与机种分步 And，避免 OR 吞条件。</para>
    /// </summary>
    /// <param name="queryDto">产品选项查询（PlantCode 必填；MaterialType、ModelCode 可选）</param>
    /// <returns>DictValue=ProductCode；DictLabel=编码或「编码 - 描述」；ExtValue=ModelCode；ExtLabel=MaterialType</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisProductOptionsAsync(
        TaktBomMaterialCostAnalysisProductOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plant = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var trimmedModel = queryDto.ModelCode?.Trim();
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.ProductCode != null
            && x.ProductCode != string.Empty);
        if (materialType != null)
        {
            var type = materialType;
            exp = exp.And(x => x.MaterialType == type);
        }
        if (!string.IsNullOrWhiteSpace(trimmedModel))
        {
            var model = trimmedModel;
            exp = exp.And(x => x.ModelCode == model);
        }
        var list = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        return list
            .Where(e => materialType == null
                || string.Equals(e.MaterialType?.Trim(), materialType, StringComparison.OrdinalIgnoreCase))
            .Where(e => string.IsNullOrWhiteSpace(trimmedModel)
                || string.Equals(e.ModelCode?.Trim(), trimmedModel, StringComparison.OrdinalIgnoreCase))
            .GroupBy(e => e.ProductCode!.Trim(), StringComparer.OrdinalIgnoreCase)
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

    // ========================================
    // 转置 / 差异 / 月度涨跌分析
    // ========================================

    /// <summary>
    /// 获取成本分析转置列表（产品 × 核算月成本矩阵 + 环比涨跌）
    /// <para>有 FocusPeriod 时加载区间向前扩 1 个月取上月成本；展示列仍用原核算期间。支持 TrendFilter 与分页。</para>
    /// </summary>
    /// <param name="queryDto">转置查询（工厂、期间、可选机种/产品/物料类型/涨跌筛选）</param>
    /// <returns>分页转置行、期间列顺序、可选机种汇总、期间合计与环比差额合计</returns>
    public async Task<TaktBomMaterialCostAnalysisTransposedResultDto> GetBomMaterialCostAnalysisTransposedListAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        // 列表仍受 MaxPageSize 约束；导出可传更大 PageSize（硬封顶 MaxAnalysisRowLoad）
        var requestedPageSize = queryDto.PageSize <= 0
            ? TaktPagedClamp.DefaultPageSize
            : queryDto.PageSize;
        var pageSize = requestedPageSize > TaktPagedClamp.DefaultMaxPageSize
            ? Math.Min(requestedPageSize, MaxAnalysisRowLoad)
            : TaktPagedClamp.NormalizePageSize(requestedPageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var focusPeriod = string.IsNullOrWhiteSpace(queryDto.FocusPeriod) ? null : queryDto.FocusPeriod.Trim();
        var plantCode = queryDto.PlantCode?.Trim() ?? string.Empty;
        var modelCode = queryDto.ModelCode?.Trim();
        // 环比需取关注月上月成本：加载区间向前扩 1 个月；展示列仍用原核算期间
        var loadQuery = CloneTransposedQueryForMomLoad(queryDto, focusPeriod);
        var rows = await LoadTransposedCostHeadersAsync(loadQuery);
        var periodOrder = BuildCostHeaderPeriodOrder(
            rows,
            queryDto.CostingDateStart,
            queryDto.CostingDateEnd,
            includeExtraPeriodsFromData: false);
        var displayPeriodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        // 目录键：归一化产品码（10/18 位 SAP 互认），避免同一产品拆成多行
        var productGroups = rows
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => NormalizeProductCatalogKey(r.ProductCode!), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        // 展示区间内任一核算月有成本即进目录（默认近 3 个月）；环比扩窗上月仅用于取数，不在 periodOrder 内故不会进目录
        var productCatalog = productGroups
            .Where(kv => kv.Value.Any(r =>
                !string.IsNullOrWhiteSpace(r.CostingPeriod)
                && displayPeriodSet.Contains(r.CostingPeriod!)))
            .Select(kv =>
            {
                var latest = kv.Value.OrderByDescending(r => r.CostingDate).FirstOrDefault();
                var code = latest?.ProductCode?.Trim() ?? kv.Key;
                return (
                    ProductCode: code,
                    ProductDescription: latest?.ProductDescription?.Trim() ?? string.Empty);
            })
            .OrderBy(x => x.ProductCode, StringComparer.Ordinal)
            .ToList();
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var trimmedProduct = queryDto.ProductCode.Trim();
            productCatalog = productCatalog
                .Where(p => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(p.ProductCode, trimmedProduct))
                .ToList();
        }
        var allTransposedRows = productCatalog
            .Select(item => BuildTransposedRowForCatalogItem(
                plantCode,
                item.ProductCode,
                item.ProductDescription,
                productGroups,
                periodOrder,
                focusPeriod))
            .ToList();
        TaktBomMaterialCostAnalysisModelSummaryDto? modelSummary = null;
        if (!string.IsNullOrWhiteSpace(modelCode) && string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var modelNameLookup = await BuildModelNameLookupAsync();
            modelNameLookup.TryGetValue(modelCode, out var modelName);
            modelSummary = BuildModelSummary(modelCode, modelName ?? modelCode, allTransposedRows, periodOrder);
        }
        var filteredRows = FilterTransposedRowsByTrend(allTransposedRows, queryDto.TrendFilter);
        var orderedRows = OrderTransposedRows(filteredRows, queryDto.SortBy);
        var total = orderedRows.Count;
        var transposedRows = orderedRows.Skip(skip).Take(pageSize).ToList();
        var (periodCostTotals, varianceAmountTotal) = SumTransposedRowGrandTotals(orderedRows, periodOrder);
        return new TaktBomMaterialCostAnalysisTransposedResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostAnalysisTransposedDto>.Create(transposedRows, total, pageIndex, pageSize),
            PeriodOrder = periodOrder,
            ModelSummary = modelSummary,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
    }

    /// <summary>
    /// 导出成本分析转置 Excel（机种/产品/品名 + 各月成本 + 涨跌/环比）
    /// <para>导出取全量转置行（PageSize=MaxAnalysisRowLoad），勿套用列表 MaxPageSize(100)。</para>
    /// </summary>
    /// <param name="query">查询条件；空则用默认查询 DTO</param>
    /// <param name="sheetName">工作表名称；空则默认「DTA BOM成本推移表」</param>
    /// <param name="fileName">导出文件名；空则默认 xlsx 文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisTransposedAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        query ??= new TaktBomMaterialCostAnalysisTransposedQueryDto();
        // 导出取全量转置行，勿套用列表 MaxPageSize(100)
        query.PageIndex = 1;
        query.PageSize = Math.Max(TaktPagedClamp.DefaultMaxPageSize, MaxAnalysisRowLoad);
        var result = await GetBomMaterialCostAnalysisTransposedListAsync(query);
        var periodOrder = result.PeriodOrder;
        var columnKeys = new List<string> { "modelCode", "productCode", "productDescription" };
        var columnLabels = new List<string> { "机种", "产品", "品名" };
        foreach (var period in periodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.Add("trend");
        columnLabels.Add("涨跌");
        columnKeys.Add("varianceAmount");
        columnLabels.Add("环比差额");
        columnKeys.Add("variancePercent");
        columnLabels.Add("环比%");
        var exportRows = result.Paged.Data.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["modelCode"] = row.ModelCode,
                ["productCode"] = row.ProductCode,
                ["productDescription"] = row.ProductDescription,
                ["trend"] = row.Trend,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(row.VariancePercent),
            };
            foreach (var period in periodOrder)
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
            sheetName ?? "DTA BOM成本推移表",
            fileName ?? "DTA BOM成本推移表.xlsx");
    }

    /// <summary>
    /// 获取成本分析差异（单产品两核算月组件级成本/单价/数量对比）
    /// <para>须 PlantCode、ProductCode、BasePeriod、ComparePeriod；按组件键合并基准/对比快照并计算变动类型。</para>
    /// </summary>
    /// <param name="queryDto">差异查询 DTO</param>
    /// <returns>汇总总成本、总差异及按差异绝对值排序的组件明细行</returns>
    public async Task<TaktBomMaterialCostAnalysisVarianceResultDto> GetBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ProductCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.BasePeriod);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ComparePeriod);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var productCode = queryDto.ProductCode.Trim();
        var basePeriod = queryDto.BasePeriod.Trim();
        var comparePeriod = queryDto.ComparePeriod.Trim();
        var (baseStart, baseEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(basePeriod);
        var (compareStart, compareEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(comparePeriod);
        var rangeStart = baseStart < compareStart ? baseStart : compareStart;
        var rangeEnd = baseEnd > compareEnd ? baseEnd : compareEnd;
        var rows = await LoadVarianceItemRowsAsync(plantCode, productCode, rangeStart, rangeEnd);
        var baseSnapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(rows, plantCode, productCode, basePeriod);
        var compareSnapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(rows, plantCode, productCode, comparePeriod);
        var productDescription = compareSnapshot.FirstOrDefault()?.ProductDescription
            ?? baseSnapshot.FirstOrDefault()?.ProductDescription
            ?? string.Empty;
        var baseMap = baseSnapshot.ToDictionary(TaktBomMaterialCostItemLineCostHelper.BuildComponentKey, StringComparer.Ordinal);
        var compareMap = compareSnapshot.ToDictionary(TaktBomMaterialCostItemLineCostHelper.BuildComponentKey, StringComparer.Ordinal);
        var componentKeys = baseMap.Keys.Union(compareMap.Keys, StringComparer.Ordinal).ToList();
        var lines = componentKeys
            .Select(key =>
            {
                baseMap.TryGetValue(key, out var baseRow);
                compareMap.TryGetValue(key, out var compareRow);
                return BuildVarianceLine(baseRow, compareRow);
            })
            .OrderByDescending(l => Math.Abs(l.VarianceAmount))
            .ToList();
        var baseTotal = TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(baseSnapshot);
        var compareTotal = TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(compareSnapshot);
        return new TaktBomMaterialCostAnalysisVarianceResultDto
        {
            PlantCode = plantCode,
            ProductCode = productCode,
            ProductDescription = productDescription,
            BasePeriod = basePeriod,
            ComparePeriod = comparePeriod,
            BaseTotalCost = baseTotal,
            CompareTotalCost = compareTotal,
            TotalVariance = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareTotal - baseTotal),
            Lines = lines,
        };
    }

    /// <summary>
    /// 导出成本分析差异 Excel（「汇总」+「差异明细」双工作表）
    /// </summary>
    /// <param name="query">差异查询条件（与 Get 一致，必填工厂/产品/基准与对比期间）</param>
    /// <param name="sheetName">明细工作表名；空则「差异明细」</param>
    /// <param name="fileName">导出文件名；空则默认 xlsx</param>
    /// <returns>实际文件名与文件字节</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostAnalysisVarianceAnalysisAsync(query);
        var summaryRows = new List<IReadOnlyDictionary<string, object?>>
        {
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "工厂代码", ["value"] = result.PlantCode },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "产品编码", ["value"] = result.ProductCode },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "产品描述", ["value"] = result.ProductDescription },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "基准期间", ["value"] = result.BasePeriod },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "对比期间", ["value"] = result.ComparePeriod },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "基准总成本", ["value"] = result.BaseTotalCost },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "对比总成本", ["value"] = result.CompareTotalCost },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "总差异", ["value"] = result.TotalVariance },
        };
        var detailKeys = new[]
        {
            "bomItemCode", "componentCode", "componentDescription", "purchaseType", "currencyCode",
            "baseCost", "compareCost", "varianceAmount", "variancePercent",
            "baseUnitPrice", "compareUnitPrice", "unitPriceVariance",
            "baseQuantity", "compareQuantity", "quantityVariance",
            "priceEffectAmount", "quantityEffectAmount", "changeType",
        };
        var detailLabels = new[]
        {
            "BOM项目号", "组件编码", "组件描述", "采购类型", "货币",
            "基准成本", "对比成本", "成本差异", "差异率%",
            "基准单价", "对比单价", "单价差异",
            "基准数量", "对比数量", "数量差异",
            "价格影响额", "数量影响额", "变动类型",
        };
        var detailRows = result.Lines.Select(line => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["bomItemCode"] = line.BomItemCode,
            ["componentCode"] = line.ComponentCode,
            ["componentDescription"] = line.ComponentDescription,
            ["purchaseType"] = line.PurchaseType,
            ["currencyCode"] = line.CurrencyCode,
            ["baseCost"] = line.BaseCost,
            ["compareCost"] = line.CompareCost,
            ["varianceAmount"] = line.VarianceAmount,
            ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(line.VariancePercent),
            ["baseUnitPrice"] = line.BaseUnitPrice,
            ["compareUnitPrice"] = line.CompareUnitPrice,
            ["unitPriceVariance"] = line.UnitPriceVariance,
            ["baseQuantity"] = line.BaseQuantity,
            ["compareQuantity"] = line.CompareQuantity,
            ["quantityVariance"] = line.QuantityVariance,
            ["priceEffectAmount"] = line.PriceEffectAmount,
            ["quantityEffectAmount"] = line.QuantityEffectAmount,
            ["changeType"] = line.ChangeType,
        }).ToList();
        using var package = new ExcelPackage();
        var summarySheet = package.Workbook.Worksheets.Add("汇总");
        summarySheet.Cells[1, 1].LoadFromArrays(new[] { new[] { "字段", "值" } });
        summarySheet.Cells[2, 1].LoadFromArrays(summaryRows.Select(r => new object[] { r["field"]!, r["value"]! }).ToArray());
        var detailSheet = package.Workbook.Worksheets.Add(sheetName ?? "差异明细");
        detailSheet.Cells[1, 1].LoadFromArrays(new[] { detailLabels });
        if (detailRows.Count > 0)
        {
            var dataArray = detailRows
                .Select(row => detailKeys.Select(k => row.TryGetValue(k, out var v) ? v ?? DBNull.Value : DBNull.Value).ToArray())
                .ToList();
            detailSheet.Cells[2, 1].LoadFromArrays(dataArray);
        }
        // 列宽：固定 Excel 标准列宽（不按内容 AutoFit）
        if (summarySheet.Dimension != null)
        {
            for (var i = 1; i <= summarySheet.Dimension.End.Column; i++)
            {
                summarySheet.Column(i).Width = TaktExcelHelper.ExcelStandardColumnWidth;
            }
        }
        if (detailSheet.Dimension != null)
        {
            for (var i = 1; i <= detailSheet.Dimension.End.Column; i++)
            {
                detailSheet.Column(i).Width = TaktExcelHelper.ExcelStandardColumnWidth;
            }
        }
        var actualFileName = fileName ?? "BOM材料成本差异分析.xlsx";
        var content = await package.GetAsByteArrayAsync();
        return (actualFileName, content);
    }

    /// <summary>
    /// 获取成本分析月度涨跌（指定机种下单产品或机种内产品平均月成本序列）
    /// <para>ProductCode 空=机种下全部物料按月取有成本产品的平均；否则为单产品月成本。逐月计算环比。</para>
    /// </summary>
    /// <param name="queryDto">须 PlantCode、ModelCode；可选 ProductCode、PeriodStart/End</param>
    /// <returns>月度涨跌结果（Lines 含期间、成本、环比差额/%、涨跌码）</returns>
    public async Task<TaktBomMaterialCostAnalysisMonthlyTrendResultDto> GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ModelCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var modelCode = queryDto.ModelCode.Trim();
        var productCode = string.IsNullOrWhiteSpace(queryDto.ProductCode) ? null : queryDto.ProductCode.Trim();
        var allMaterialsUnderModel = productCode == null;
        var (rangeStart, rangeEnd) = ResolvePeriodRangeBounds(queryDto.PeriodStart, queryDto.PeriodEnd);
        var loadQuery = new TaktBomMaterialCostAnalysisTransposedQueryDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode,
            CostingDateStart = rangeStart,
            CostingDateEnd = rangeEnd,
            PageIndex = 1,
            PageSize = TaktPagedClamp.DefaultPageSize,
        };
        var rows = await LoadTransposedCostHeadersAsync(loadQuery);
        var periodOrder = BuildCostHeaderPeriodOrder(rows, rangeStart, rangeEnd);
        var productCodesInScope = ResolveProductCodesInScope(rows, plantCode, modelCode, productCode);
        var productDescription = allMaterialsUnderModel
            ? string.Empty
            : rows.FirstOrDefault(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode!))?.ProductDescription ?? string.Empty;
        var trendLines = new List<TaktBomMaterialCostAnalysisMonthlyTrendLineDto>();
        decimal? previousCost = null;
        string? previousPeriod = null;
        foreach (var period in periodOrder)
        {
            decimal totalCost;
            if (allMaterialsUnderModel)
            {
                var costs = productCodesInScope
                    .Select(pc => ResolveProductMonthlyCostFromHeaders(rows, plantCode, modelCode, pc, period))
                    .Where(c => c > 0m)
                    .ToList();
                if (costs.Count == 0)
                {
                    continue;
                }
                totalCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
            }
            else
            {
                totalCost = ResolveProductMonthlyCostFromHeaders(rows, plantCode, modelCode, productCode!, period);
                if (totalCost <= 0m)
                {
                    continue;
                }
            }
            var (varianceAmount, variancePercent, trend) = ComputeMonthOverMonthTrend(totalCost, previousCost);
            trendLines.Add(new TaktBomMaterialCostAnalysisMonthlyTrendLineDto
            {
                Period = period,
                TotalCost = totalCost,
                BasePeriod = previousPeriod,
                BaseTotalCost = previousCost,
                VarianceAmount = varianceAmount,
                VariancePercent = variancePercent,
                Trend = trend,
            });
            previousCost = totalCost;
            previousPeriod = period;
        }
        return new TaktBomMaterialCostAnalysisMonthlyTrendResultDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode ?? string.Empty,
            ProductDescription = productDescription,
            AllMaterialsUnderModel = allMaterialsUnderModel,
            Lines = trendLines,
        };
    }

    /// <summary>
    /// 导出成本分析月度涨跌 Excel
    /// </summary>
    /// <param name="query">月度涨跌查询条件（与 Get 一致）</param>
    /// <param name="sheetName">工作表名称；空则由导出辅助默认</param>
    /// <param name="fileName">导出文件名；空则由导出辅助默认</param>
    /// <returns>实际文件名与文件字节</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "productCode", "productDescription", "period", "totalCost",
            "basePeriod", "baseTotalCost", "varianceAmount", "variancePercent", "trend",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "产品编码", "产品描述", "年月", "材料总成本",
            "对比基准月", "基准月成本", "环比差额", "环比%", "涨跌",
        };
        var exportRows = result.Lines.Select(line => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = result.PlantCode,
            ["modelCode"] = result.ModelCode,
            ["productCode"] = result.ProductCode,
            ["productDescription"] = result.ProductDescription,
            ["period"] = line.Period,
            ["totalCost"] = line.TotalCost,
            ["basePeriod"] = line.BasePeriod,
            ["baseTotalCost"] = line.BaseTotalCost,
            ["varianceAmount"] = line.VarianceAmount,
            ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(line.VariancePercent),
            ["trend"] = line.Trend,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "BOM材料月度涨跌",
            fileName ?? $"BOM材料月度涨跌_{result.ModelCode}.xlsx");
    }

    // ========================================
    // 成本合计 / 重算 / 机种月均（非 CRUD；避免 generate-all 覆盖 Item/Cost 服务）
    // ========================================

    /// <summary>
    /// 规范化重算查询为单个核算月（供后台任务与合计入口共用）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>规范化查询与核算月份标签</returns>
    public static TaktBomMaterialCostItemRecalculatePreparedQueryDto PrepareRecalculateModelAverageQuery(
        TaktBomMaterialCostItemQueryDto queryDto)
    {
        queryDto ??= new TaktBomMaterialCostItemQueryDto();
        var normalized = queryDto.Adapt<TaktBomMaterialCostItemQueryDto>();
        if (!normalized.CostingDateStart.HasValue || !normalized.CostingDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择核算月份后再重算");
        }
        var startMonth = new DateTime(
            normalized.CostingDateStart.Value.Year,
            normalized.CostingDateStart.Value.Month,
            1);
        var endMonth = new DateTime(
            normalized.CostingDateEnd.Value.Year,
            normalized.CostingDateEnd.Value.Month,
            1);
        if (startMonth != endMonth)
        {
            throw new TaktBusinessException("重算仅支持单个核算月份，请缩小日期范围");
        }
        var lastDay = DateTime.DaysInMonth(startMonth.Year, startMonth.Month);
        normalized.CostingDateStart = startMonth;
        normalized.CostingDateEnd = new DateTime(startMonth.Year, startMonth.Month, lastDay, 23, 59, 59, 999);
        normalized.PageIndex = 1;
        normalized.PageSize = 1;
        return new TaktBomMaterialCostItemRecalculatePreparedQueryDto
        {
            Query = normalized,
            ProcessedMonth = $"{startMonth.Year:D4}-{startMonth.Month:D2}",
        };
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemRecalculateModelAverageResultDto> RecalculateBomMaterialCostItemModelMonthlyAverageAsync(
        TaktBomMaterialCostItemQueryDto queryDto,
        bool forceRecalculate = false,
        int processRecordCount = 5000)
    {
        EnsureThreeLayerContext();
        if (processRecordCount < 0)
        {
            throw new TaktBusinessException("处理记录数不能为负数（0 表示全部）");
        }
        var prepared = PrepareRecalculateModelAverageQuery(queryDto);
        await ApplyModelCodeProductScopeAsync(prepared.Query);
        var periodKey = prepared.ProcessedMonth;
        var filterModel = prepared.Query.ModelCode?.Trim();
        var itemRows = await LoadItemsForRecalculateQueryAsync(prepared.Query);
        var groupedKeys = itemRows
            .Where(r => !string.IsNullOrWhiteSpace(r.PlantCode) && !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(
                r =>
                {
                    var plant = r.PlantCode!.Trim();
                    var product = r.ProductCode!.Trim();
                    var period = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(r.CostingDate);
                    return $"{plant}|{product}|{period}";
                },
                StringComparer.OrdinalIgnoreCase)
            .Select(g =>
            {
                var latest = g.OrderByDescending(x => x.CostingDate).First();
                return (
                    PlantCode: latest.PlantCode!.Trim(),
                    ProductCode: latest.ProductCode!.Trim(),
                    CostingDate: latest.CostingDate,
                    Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(latest.CostingDate));
            })
            .Where(k => string.Equals(k.Period, periodKey, StringComparison.Ordinal))
            .OrderBy(k => k.PlantCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(k => k.ProductCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var plantCodes = groupedKeys
            .Select(k => k.PlantCode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var materialPlants = plantCodes.Count == 0
            ? new List<TaktMaterialPlant>()
            : await _materialPlantRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && plantCodes.Contains(x.PlantCode));
        var destinations = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialCode != null);
        var syncKeys = new List<(string PlantCode, string ProductCode, DateTime CostingDate)>();
        var skippedCount = 0;
        foreach (var key in groupedKeys)
        {
            if (!TaktBomMaterialCostItemLineCostHelper.IsFertPlantProduct(
                    materialPlants, key.PlantCode, key.ProductCode))
            {
                skippedCount = checked(skippedCount + 1);
                continue;
            }
            if (!string.IsNullOrWhiteSpace(filterModel))
            {
                var resolvedModel = ResolveModelCodeFromDestinations(destinations, key.ProductCode);
                if (!string.Equals(resolvedModel, filterModel, StringComparison.OrdinalIgnoreCase))
                {
                    skippedCount = checked(skippedCount + 1);
                    continue;
                }
            }
            syncKeys.Add((key.PlantCode, key.ProductCode, key.CostingDate));
        }
        var totalFertGroupCount = syncKeys.Count;
        if (processRecordCount > 0 && syncKeys.Count > processRecordCount)
        {
            syncKeys = syncKeys.Take(processRecordCount).ToList();
        }
        if (syncKeys.Count > 0)
        {
            await SyncBomMaterialCostFromItemsBatchAsync(syncKeys, destinations, materialPlants);
        }
        return new TaktBomMaterialCostItemRecalculateModelAverageResultDto
        {
            ScannedRowCount = itemRows.Count,
            RefreshedGroupCount = syncKeys.Count,
            SkippedGroupCount = skippedCount + Math.Max(0, totalFertGroupCount - syncKeys.Count),
            ResetGroupCount = forceRecalculate ? syncKeys.Count : 0,
            ProcessedMonthCount = 1,
            ProcessedMonth = prepared.ProcessedMonth,
        };
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemRecalculateModelAverageResultDto?> RunScheduledBomMaterialCostSumAsync(
        bool force = false,
        DateTime? asOfDate = null,
        int nthWorkingDay = 3)
    {
        _ = force;
        _ = nthWorkingDay;
        var query = BuildScheduledCurrentMonthQuery(asOfDate);
        return await RecalculateBomMaterialCostItemModelMonthlyAverageAsync(
            query,
            forceRecalculate: false,
            processRecordCount: 0);
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemRecalculateModelAverageResultDto?> RunScheduledBomMaterialCostRecalculateAsync(
        bool force = false,
        DateTime? asOfDate = null,
        int nthWorkingDay = 3)
    {
        _ = force;
        _ = nthWorkingDay;
        var query = BuildScheduledCurrentMonthQuery(asOfDate);
        return await RecalculateBomMaterialCostItemModelMonthlyAverageAsync(
            query,
            forceRecalculate: true,
            processRecordCount: 0);
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostRefreshModelResultDto> RefreshBomMaterialCostModelFieldsAsync(
        TaktBomMaterialCostRefreshModelQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.CostingPeriod);
        var plant = queryDto.PlantCode.Trim();
        var periodKey = queryDto.CostingPeriod.Trim();
        var filterModel = string.IsNullOrWhiteSpace(queryDto.ModelCode) ? null : queryDto.ModelCode.Trim();
        var headerExp = Expressionable.Create<TaktBomMaterialCost>();
        headerExp = headerExp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.CostingPeriod == periodKey);
        if (filterModel != null)
        {
            headerExp = headerExp.And(x => x.ModelCode == filterModel);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(headerExp.ToExpression());
        var destinations = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialCode != null);
        var materialPlants = await _materialPlantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant);
        var modelCodeUpdated = 0;
        var materialTypeUpdated = 0;
        var touchedGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers)
        {
            var previousModel = header.ModelCode?.Trim() ?? string.Empty;
            var previousType = string.IsNullOrWhiteSpace(header.MaterialType)
                ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
                : header.MaterialType.Trim();
            if (!string.IsNullOrWhiteSpace(previousModel))
            {
                touchedGroups.Add($"{previousType}|{previousModel}");
            }
            var resolvedModel = ResolveModelCodeFromDestinations(destinations, header.ProductCode);
            var resolvedType = TaktBomMaterialCostItemLineCostHelper.ResolveMaterialTypeFromPlant(
                materialPlants, plant, header.ProductCode);
            if (string.IsNullOrWhiteSpace(resolvedType))
            {
                resolvedType = previousType;
            }
            var nextModel = string.IsNullOrWhiteSpace(resolvedModel) ? previousModel : resolvedModel.Trim();
            var nextType = resolvedType.Trim();
            var modelChanged = !string.IsNullOrWhiteSpace(resolvedModel)
                && !string.Equals(resolvedModel, previousModel, StringComparison.OrdinalIgnoreCase);
            var typeChanged = !string.Equals(nextType, previousType, StringComparison.OrdinalIgnoreCase);
            if (!modelChanged && !typeChanged)
            {
                if (!string.IsNullOrWhiteSpace(nextModel))
                {
                    touchedGroups.Add($"{nextType}|{nextModel}");
                }
                continue;
            }
            if (modelChanged)
            {
                var clash = await _bomMaterialCostRepository.FirstAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && x.ModelCode == resolvedModel
                        && x.ProductCode == header.ProductCode
                        && x.CostingPeriod == periodKey
                        && x.Id != header.Id);
                if (clash != null)
                {
                    ThrowBusinessException(
                        $"产品 {header.ProductCode} 在期间 {periodKey} 已存在机种 {resolvedModel} 的主表行，无法更新机种编码");
                }
                header.ModelCode = resolvedModel;
                modelCodeUpdated = checked(modelCodeUpdated + 1);
            }
            if (typeChanged)
            {
                header.MaterialType = nextType;
                materialTypeUpdated = checked(materialTypeUpdated + 1);
            }
            await _bomMaterialCostRepository.UpdateAsync(header);
            if (!string.IsNullOrWhiteSpace(header.ModelCode))
            {
                touchedGroups.Add($"{header.MaterialType?.Trim() ?? nextType}|{header.ModelCode.Trim()}");
            }
        }
        if (filterModel != null && touchedGroups.Count == 0)
        {
            var fallbackType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
            touchedGroups.Add($"{fallbackType}|{filterModel}");
        }
        var averageUpdated = 0;
        foreach (var key in touchedGroups.OrderBy(k => k, StringComparer.OrdinalIgnoreCase))
        {
            var sep = key.IndexOf('|');
            if (sep <= 0 || sep >= key.Length - 1)
            {
                continue;
            }
            var mt = key[..sep];
            var model = key[(sep + 1)..];
            averageUpdated = checked(
                averageUpdated + await RefreshModelMonthlyAverageForPeriodAsync(plant, mt, model, periodKey));
        }
        return new TaktBomMaterialCostRefreshModelResultDto
        {
            ScannedRowCount = headers.Count,
            ModelCodeUpdatedCount = modelCodeUpdated,
            MaterialTypeUpdatedCount = materialTypeUpdated,
            AverageUpdatedCount = averageUpdated,
            ModelGroupCount = touchedGroups.Count,
            CostingPeriod = periodKey,
        };
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostRefreshModelResultDto?> RunScheduledBomModelAvgCostAsync(
        DateTime? asOfDate = null)
    {
        EnsureThreeLayerContext();
        var asOf = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(asOfDate ?? DateTime.Today);
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(asOf);
        var periodHeaders = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.CostingPeriod == periodKey);
        if (periodHeaders.Count == 0)
        {
            return null;
        }
        var plantCodes = periodHeaders
            .Select(x => x.PlantCode?.Trim() ?? string.Empty)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var scanned = 0;
        var modelUpdated = 0;
        var typeUpdated = 0;
        var averageUpdated = 0;
        var modelGroups = 0;
        foreach (var plant in plantCodes)
        {
            var result = await RefreshBomMaterialCostModelFieldsAsync(
                new TaktBomMaterialCostRefreshModelQueryDto
                {
                    PlantCode = plant,
                    CostingPeriod = periodKey,
                });
            scanned = checked(scanned + result.ScannedRowCount);
            modelUpdated = checked(modelUpdated + result.ModelCodeUpdatedCount);
            typeUpdated = checked(typeUpdated + result.MaterialTypeUpdatedCount);
            averageUpdated = checked(averageUpdated + result.AverageUpdatedCount);
            modelGroups = checked(modelGroups + result.ModelGroupCount);
        }
        return new TaktBomMaterialCostRefreshModelResultDto
        {
            ScannedRowCount = scanned,
            ModelCodeUpdatedCount = modelUpdated,
            MaterialTypeUpdatedCount = typeUpdated,
            AverageUpdatedCount = averageUpdated,
            ModelGroupCount = modelGroups,
            CostingPeriod = periodKey,
        };
    }

    /// <summary>
    /// 构建定时任务目标月查询：仅判定日所在自然月
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>当月查询</returns>
    private TaktBomMaterialCostItemQueryDto BuildScheduledCurrentMonthQuery(DateTime? asOfDate)
    {
        EnsureThreeLayerContext();
        var asOf = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(asOfDate ?? DateTime.Today);
        var costingStart = new DateTime(asOf.Year, asOf.Month, 1);
        var costingEnd = new DateTime(
            asOf.Year,
            asOf.Month,
            DateTime.DaysInMonth(asOf.Year, asOf.Month),
            23,
            59,
            59,
            999);
        return new TaktBomMaterialCostItemQueryDto
        {
            CostingDateStart = costingStart,
            CostingDateEnd = costingEnd,
        };
    }

    /// <summary>
    /// 按重算查询加载明细（年分表优先；含工厂/产品/产品集过滤）
    /// </summary>
    /// <param name="query">已规范化单月查询</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadItemsForRecalculateQueryAsync(
        TaktBomMaterialCostItemQueryDto query)
    {
        ArgumentNullException.ThrowIfNull(query);
        var start = query.CostingDateStart
            ?? throw new TaktBusinessException("请选择核算月份后再重算");
        var end = query.CostingDateEnd
            ?? throw new TaktBusinessException("请选择核算月份后再重算");
        var yearTable = await ResolveBomItemPhysicalTableAsync(start.Year);
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.CostingDate >= start
            && x.CostingDate <= end);
        if (!string.IsNullOrWhiteSpace(query.PlantCode))
        {
            var plant = query.PlantCode.Trim();
            exp = exp.And(x => x.PlantCode == plant);
        }
        if (!string.IsNullOrWhiteSpace(query.ProductCode))
        {
            var product = query.ProductCode.Trim();
            exp = exp.And(x => x.ProductCode == product);
        }
        else if (query.ProductCodes != null && query.ProductCodes.Count > 0)
        {
            var codes = query.ProductCodes
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (codes.Count > 0)
            {
                exp = exp.And(x => codes.Contains(x.ProductCode));
            }
        }
        return await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
    }

    /// <summary>
    /// 批量：明细回算主表（同月同产品 Upsert）
    /// </summary>
    /// <param name="keys">工厂+产品+核算日</param>
    /// <param name="destinations">型号目的地（可空则内查）</param>
    /// <param name="materialPlants">工厂物料（可空则按工厂内查）</param>
    /// <returns>任务</returns>
    private async Task SyncBomMaterialCostFromItemsBatchAsync(
        IEnumerable<(string PlantCode, string ProductCode, DateTime CostingDate)> keys,
        IReadOnlyList<TaktModelDestination>? destinations = null,
        IReadOnlyList<TaktMaterialPlant>? materialPlants = null)
    {
        ArgumentNullException.ThrowIfNull(keys);
        var distinct = keys
            .Where(k => !string.IsNullOrWhiteSpace(k.PlantCode) && !string.IsNullOrWhiteSpace(k.ProductCode))
            .Select(k => (
                PlantCode: k.PlantCode.Trim(),
                ProductCode: k.ProductCode.Trim(),
                Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(k.CostingDate),
                CostingDate: k.CostingDate))
            .GroupBy(k => $"{k.PlantCode}|{k.ProductCode}|{k.Period}", StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(x => x.CostingDate).First())
            .ToList();
        var destList = destinations
            ?? await _modelDestinationRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.MaterialCode != null);
        foreach (var key in distinct)
        {
            await SyncBomMaterialCostFromItemsAsync(
                key.PlantCode,
                key.ProductCode,
                key.CostingDate,
                destList,
                materialPlants);
        }
    }

    /// <summary>
    /// 单组：明细回算主表（同工厂+产品+核算月 Upsert；无明细外键）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品</param>
    /// <param name="costingDate">核算日</param>
    /// <param name="destinations">型号目的地</param>
    /// <param name="materialPlants">工厂物料（可空）</param>
    /// <returns>任务</returns>
    private async Task SyncBomMaterialCostFromItemsAsync(
        string plantCode,
        string productCode,
        DateTime costingDate,
        IReadOnlyList<TaktModelDestination> destinations,
        IReadOnlyList<TaktMaterialPlant>? materialPlants)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentNullException.ThrowIfNull(destinations);
        var plant = plantCode.Trim();
        var product = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode.Trim());
        if (string.IsNullOrWhiteSpace(product))
        {
            product = productCode.Trim();
        }
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(costingDate);
        var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodKey);
        var yearTable = await ResolveBomItemPhysicalTableAsync(monthStart.Year);
        var monthItems = await _bomMaterialCostItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.CostingDate >= monthStart
                && x.CostingDate <= monthEnd
                && x.ProductCode != null,
            yearTable);
        var productItems = monthItems
            .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, product))
            .ToList();
        var modelCode = ResolveModelCodeFromDestinations(destinations, product);
        var plants = materialPlants
            ?? await _materialPlantRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant);
        var materialType = TaktBomMaterialCostItemLineCostHelper.ResolveMaterialTypeFromPlant(plants, plant, product);
        if (string.IsNullOrWhiteSpace(materialType))
        {
            materialType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        }
        var existing = await FindHeaderByMonthKeyAsync(plant, modelCode, product, periodKey);
        if (productItems.Count == 0)
        {
            if (existing == null)
            {
                return;
            }
            existing.ProductMonthlyCost = 0;
            existing.ModelMonthlyAverageCost = 0;
            existing.CostingDate = costingDate;
            existing.CostingPeriod = periodKey;
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                existing.ModelCode = modelCode;
            }
            existing.MaterialType = materialType;
            await _bomMaterialCostRepository.UpdateAsync(existing);
            await RefreshModelMonthlyAverageForPeriodAsync(
                plant,
                existing.MaterialType,
                existing.ModelCode,
                periodKey);
            return;
        }
        var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
            productItems, plant, product, periodKey);
        var latestCostingDate = TaktBomMaterialCostItemLineCostHelper.ResolveLatestCostingDate(
            productItems, plant, product, periodKey) ?? costingDate;
        var productMonthlyCost = TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(snapshot);
        var currency = snapshot
            .Select(TaktBomMaterialCostItemLineCostHelper.ResolveCurrency)
            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? productItems
                .Select(x => x.MovingPriceCurrencyCode)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? string.Empty;
        var productDescription = snapshot.FirstOrDefault()?.ProductDescription
            ?? productItems.OrderByDescending(x => x.CostingDate).First().ProductDescription
            ?? string.Empty;
        if (existing == null)
        {
            existing = new TaktBomMaterialCost
            {
                PlantCode = plant,
                ModelCode = modelCode,
                MaterialType = materialType,
                ProductCode = product,
                ProductDescription = productDescription,
                ProductMonthlyCost = productMonthlyCost,
                ModelMonthlyAverageCost = 0,
                CurrencyCode = currency,
                CostingPeriod = periodKey,
                CostingDate = latestCostingDate,
            };
            existing = await _bomMaterialCostRepository.CreateAsync(existing);
        }
        else
        {
            existing.ModelCode = string.IsNullOrWhiteSpace(modelCode) ? existing.ModelCode : modelCode;
            existing.MaterialType = materialType;
            existing.ProductCode = product;
            existing.ProductDescription = productDescription;
            existing.ProductMonthlyCost = productMonthlyCost;
            existing.CurrencyCode = currency;
            existing.CostingPeriod = periodKey;
            existing.CostingDate = latestCostingDate;
            await _bomMaterialCostRepository.UpdateAsync(existing);
        }
        var effectiveModel = string.IsNullOrWhiteSpace(existing.ModelCode) ? modelCode : existing.ModelCode;
        var effectiveType = string.IsNullOrWhiteSpace(existing.MaterialType) ? materialType : existing.MaterialType;
        await RefreshModelMonthlyAverageForPeriodAsync(plant, effectiveType, effectiveModel, periodKey);
    }

    /// <summary>
    /// 查找同月主表行（工厂+产品+期间；机种空时放宽）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="productCode">产品</param>
    /// <param name="periodKey">期间 yyyy-MM</param>
    /// <returns>主表实体</returns>
    private async Task<TaktBomMaterialCost?> FindHeaderByMonthKeyAsync(
        string plantCode,
        string modelCode,
        string productCode,
        string periodKey)
    {
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.CostingPeriod == periodKey
                && x.ProductCode != null);
        return list.FirstOrDefault(x =>
            TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, productCode)
            && (string.IsNullOrWhiteSpace(modelCode)
                || string.Equals(x.ModelCode, modelCode, StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrWhiteSpace(x.ModelCode)));
    }

    /// <summary>
    /// 刷新同工厂+物料类型+机种+核算期间下全部主表行的机种月平均成本
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <param name="periodKey">期间</param>
    /// <returns>实际更新的行数</returns>
    private async Task<int> RefreshModelMonthlyAverageForPeriodAsync(
        string plantCode,
        string materialType,
        string modelCode,
        string periodKey)
    {
        if (string.IsNullOrWhiteSpace(modelCode) || string.IsNullOrWhiteSpace(materialType))
        {
            return 0;
        }
        var mt = materialType.Trim();
        var headers = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.MaterialType == mt
                && x.ModelCode == modelCode
                && x.CostingPeriod == periodKey);
        if (headers.Count == 0)
        {
            return 0;
        }
        var costs = headers
            .Where(h => h.ProductMonthlyCost > 0)
            .Select(h => h.ProductMonthlyCost)
            .ToList();
        var average = TaktBomMaterialCostItemModelEnrichmentHelper.ComputeModelMonthlyAverageFromProductCosts(costs);
        var updated = 0;
        foreach (var header in headers)
        {
            if (header.ModelMonthlyAverageCost == average)
            {
                continue;
            }
            header.ModelMonthlyAverageCost = average;
            await _bomMaterialCostRepository.UpdateAsync(header);
            updated = checked(updated + 1);
        }
        return updated;
    }

    /// <summary>
    /// 从已加载的型号目的地列表解析产品对应机种
    /// </summary>
    /// <param name="destinations">型号目的地</param>
    /// <param name="productCode">产品编码</param>
    /// <returns>机种编码；未匹配为空</returns>
    private static string ResolveModelCodeFromDestinations(
        IReadOnlyList<TaktModelDestination> destinations,
        string? productCode)
    {
        ArgumentNullException.ThrowIfNull(destinations);
        if (string.IsNullOrWhiteSpace(productCode))
        {
            return string.Empty;
        }
        var match = destinations.FirstOrDefault(x =>
            TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.MaterialCode, productCode));
        return match?.ModelCode?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 加载转置/月度涨跌用成本汇总行（MaterialType 有值才过滤；空=本表全类型）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>成本汇总行</returns>
    private async Task<List<TaktBomMaterialCost>> LoadTransposedCostHeadersAsync(TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode);
        if (materialType != null)
        {
            var type = materialType;
            exp = exp.And(x => x.MaterialType == type);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            var plantCode = queryDto.PlantCode.Trim();
            exp = exp.And(x => x.PlantCode == plantCode);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            var modelCode = queryDto.ModelCode.Trim();
            exp = exp.And(x => x.ModelCode == modelCode);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var productCode = queryDto.ProductCode.Trim();
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(productCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords.Trim();
            exp = exp.And(x =>
                (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords)));
        }
        if (queryDto.CostingDateStart.HasValue)
        {
            exp = exp.And(x => x.CostingDate >= queryDto.CostingDateStart);
        }
        if (queryDto.CostingDateEnd.HasValue)
        {
            exp = exp.And(x => x.CostingDate <= queryDto.CostingDateEnd);
        }
        var rows = await _bomMaterialCostRepository.GetListForExportAsync(exp.ToExpression(), MaxAnalysisRowLoad);
        if (rows.Count >= MaxAnalysisRowLoad)
        {
            throw new TaktBusinessException(
                $"分析数据行数为 {rows.Count}，达到上限 {MaxAnalysisRowLoad}，请缩小筛选（工厂/机种/产品/核算期间）");
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var productCode = queryDto.ProductCode.Trim();
            rows = rows
                .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode))
                .ToList();
        }
        return rows;
    }

    /// <summary>
    /// 加载差异分析用明细行
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="rangeStart">核算日起</param>
    /// <param name="rangeEnd">核算日止</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadVarianceItemRowsAsync(
        string plantCode,
        string productCode,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var normalizedProduct = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode);
        Expression<Func<TaktBomMaterialCostItem, bool>> predicate = x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.CostingDate >= rangeStart
            && x.CostingDate <= rangeEnd
            && (x.ProductCode == productCode
                || x.ProductCode == normalizedProduct
                || (x.ProductCode != null && x.ProductCode.Contains(productCode)));
        var rows = await GetBomItemListForRangeAsync(predicate, rangeStart, rangeEnd, MaxAnalysisRowLoad);
        if (rows.Count >= MaxAnalysisRowLoad)
        {
            throw new TaktBusinessException(
                $"差异分析明细行数为 {rows.Count}，达到上限 {MaxAnalysisRowLoad}，请缩小期间范围");
        }
        return rows
            .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode))
            .ToList();
    }

    /// <summary>
    /// 构建机种名称查找表（型号目的地 ModelCode → ModelName，仅展示名）
    /// </summary>
    /// <returns>机种名称字典</returns>
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

    /// <summary>
    /// 转置产品目录分组键（SAP 18/10 位数字码归一化后再分组，避免同一产品拆行）
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <returns>分组键</returns>
    private static string NormalizeProductCatalogKey(string productCode)
    {
        var trimmed = productCode.Trim();
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        return string.IsNullOrEmpty(normalized) ? trimmed : normalized;
    }

    /// <summary>
    /// 克隆转置查询并将起始日提前一月，便于环比取上月成本（不改变展示用 CostingDateStart）
    /// </summary>
    /// <param name="queryDto">原查询</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <returns>加载用查询</returns>
    private static TaktBomMaterialCostAnalysisTransposedQueryDto CloneTransposedQueryForMomLoad(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto,
        string? focusPeriod)
    {
        var loadQuery = new TaktBomMaterialCostAnalysisTransposedQueryDto
        {
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
            KeyWords = queryDto.KeyWords,
            PlantCode = queryDto.PlantCode,
            ModelCode = queryDto.ModelCode,
            ProductCode = queryDto.ProductCode,
            MaterialType = queryDto.MaterialType,
            CostingDateStart = queryDto.CostingDateStart,
            CostingDateEnd = queryDto.CostingDateEnd,
            FocusPeriod = queryDto.FocusPeriod,
            TrendFilter = queryDto.TrendFilter,
            SortBy = queryDto.SortBy,
        };
        if (string.IsNullOrWhiteSpace(focusPeriod)
            || !DateTime.TryParseExact(
                focusPeriod.Trim() + "-01",
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out var focusMonth))
        {
            return loadQuery;
        }
        var momStart = new DateTime(focusMonth.Year, focusMonth.Month, 1).AddMonths(-1);
        if (!loadQuery.CostingDateStart.HasValue || loadQuery.CostingDateStart.Value > momStart)
        {
            loadQuery.CostingDateStart = momStart;
        }
        return loadQuery;
    }

    /// <summary>
    /// 构建成本汇总行的期间列顺序（yyyy-MM）
    /// </summary>
    /// <param name="rows">已加载汇总行</param>
    /// <param name="start">核算日起（可空）</param>
    /// <param name="end">核算日止（可空）</param>
    /// <param name="includeExtraPeriodsFromData">起止都有值时，是否把数据中落在区间外的期间并入并排序</param>
    /// <returns>升序期间键列表；无起止则仅用数据中出现的期间</returns>
    private static List<string> BuildCostHeaderPeriodOrder(
        IReadOnlyList<TaktBomMaterialCost> rows,
        DateTime? start,
        DateTime? end,
        bool includeExtraPeriodsFromData = true)
    {
        var periods = rows
            .Select(r => r.CostingPeriod)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
        if (start.HasValue && end.HasValue && start.Value <= end.Value)
        {
            var cursor = new DateTime(start.Value.Year, start.Value.Month, 1);
            var endMonth = new DateTime(end.Value.Year, end.Value.Month, 1);
            var rangePeriods = new List<string>();
            while (cursor <= endMonth)
            {
                rangePeriods.Add(TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(cursor));
                cursor = cursor.AddMonths(1);
            }
            if (includeExtraPeriodsFromData)
            {
                foreach (var period in periods)
                {
                    if (!rangePeriods.Contains(period, StringComparer.Ordinal))
                    {
                        rangePeriods.Add(period);
                    }
                }
                rangePeriods.Sort(StringComparer.Ordinal);
            }
            return rangePeriods;
        }
        return periods;
    }

    /// <summary>
    /// 按主表产品目录项构建转置行（PeriodCosts 取自 TaktBomMaterialCost.ProductMonthlyCost）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="catalogProductCode">主表产品编码</param>
    /// <param name="catalogDescription">主表产品描述</param>
    /// <param name="productGroups">主表按产品编码分组</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间（可选）</param>
    /// <returns>转置行</returns>
    private static TaktBomMaterialCostAnalysisTransposedDto BuildTransposedRowForCatalogItem(
        string plantCode,
        string catalogProductCode,
        string catalogDescription,
        IReadOnlyDictionary<string, List<TaktBomMaterialCost>> productGroups,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        List<TaktBomMaterialCost>? matchedRows = null;
        foreach (var (productCode, rows) in productGroups)
        {
            if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(productCode, catalogProductCode))
            {
                continue;
            }
            matchedRows = rows;
            break;
        }
        if (matchedRows == null || matchedRows.Count == 0)
        {
            return new TaktBomMaterialCostAnalysisTransposedDto
            {
                PlantCode = plantCode,
                ModelCode = string.Empty,
                ProductCode = catalogProductCode,
                ProductDescription = catalogDescription,
                PeriodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal),
                Trend = "none",
            };
        }
        var description = matchedRows.OrderByDescending(r => r.CostingDate).FirstOrDefault()?.ProductDescription?.Trim();
        if (string.IsNullOrWhiteSpace(description))
        {
            description = catalogDescription;
        }
        var row = BuildTransposedRowFromCostHeaders(
            plantCode,
            catalogProductCode,
            description ?? catalogProductCode,
            matchedRows,
            periodOrder);
        ApplyFocusPeriodTrend(row, focusPeriod, matchedRows);
        return row;
    }

    /// <summary>
    /// 构建单产品转置行（汇总表 ProductMonthlyCost）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="productDescription">产品描述</param>
    /// <param name="productRows">产品汇总行</param>
    /// <param name="periodOrder">期间顺序</param>
    /// <returns>转置行</returns>
    private static TaktBomMaterialCostAnalysisTransposedDto BuildTransposedRowFromCostHeaders(
        string plantCode,
        string productCode,
        string productDescription,
        List<TaktBomMaterialCost> productRows,
        IReadOnlyList<string> periodOrder)
    {
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var header = productRows
                .Where(r => string.Equals(r.CostingPeriod, period, StringComparison.Ordinal))
                .OrderByDescending(r => r.CostingDate)
                .FirstOrDefault();
            if (header != null && header.ProductMonthlyCost > 0m)
            {
                periodCosts[period] = header.ProductMonthlyCost;
            }
        }
        var latest = productRows.OrderByDescending(r => r.CostingDate).FirstOrDefault();
        var resolvedPlant = !string.IsNullOrWhiteSpace(plantCode)
            ? plantCode
            : latest?.PlantCode ?? string.Empty;
        return new TaktBomMaterialCostAnalysisTransposedDto
        {
            PlantCode = resolvedPlant,
            ModelCode = latest?.ModelCode?.Trim() ?? string.Empty,
            ProductCode = productCode,
            ProductDescription = productDescription,
            PeriodCosts = periodCosts,
            CurrencyCode = latest?.CurrencyCode ?? string.Empty,
            Trend = "none",
        };
    }

    /// <summary>
    /// 按 FocusPeriod 填充单行环比字段（上月成本可来自加载扩窗，不必出现在展示列）
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <param name="productRows">该产品主表行（含扩窗上月）</param>
    private static void ApplyFocusPeriodTrend(
        TaktBomMaterialCostAnalysisTransposedDto row,
        string? focusPeriod,
        IReadOnlyList<TaktBomMaterialCost> productRows)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(comparePeriod + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var compareMonth))
        {
            return;
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        if (!TryResolveProductMonthlyCost(productRows, basePeriod, out var baseCost)
            || !TryResolveProductMonthlyCost(productRows, comparePeriod, out var compareCost))
        {
            row.Trend = "none";
            return;
        }
        row.VarianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareCost - baseCost);
        if (baseCost != 0m)
        {
            row.VariancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(
                row.VarianceAmount.Value / baseCost);
        }
        if (compareCost > baseCost)
        {
            row.Trend = "up";
        }
        else if (compareCost < baseCost)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 按涨跌筛选过滤转置行
    /// </summary>
    /// <param name="rows">转置行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>过滤后列表</returns>
    private static List<TaktBomMaterialCostAnalysisTransposedDto> FilterTransposedRowsByTrend(
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
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
    /// 转置分析行全量排序（分页前）：productCode（默认）/ trend / varianceDesc
    /// </summary>
    /// <param name="rows">已筛选行</param>
    /// <param name="sortBy">排序码</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomMaterialCostAnalysisTransposedDto> OrderTransposedRows(
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        string? sortBy)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        var mode = (sortBy ?? string.Empty).Trim().ToLowerInvariant();
        IOrderedEnumerable<TaktBomMaterialCostAnalysisTransposedDto> ordered = mode switch
        {
            "trend" => rows
                .OrderBy(r => TrendRank(r.Trend))
                .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m)),
            "variancedesc" => rows
                .OrderByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
                .ThenBy(r => TrendRank(r.Trend)),
            _ => rows.OrderBy(r => r.ProductCode, StringComparer.Ordinal), // productCode 默认
        };
        return ordered
            .ThenBy(r => r.ModelCode, StringComparer.Ordinal)
            .ThenBy(r => r.ProductCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 解析某产品某月 ProductMonthlyCost（取该月最新核算日）
    /// </summary>
    /// <param name="productRows">产品主表行</param>
    /// <param name="period">yyyy-MM</param>
    /// <param name="cost">成本</param>
    /// <returns>是否存在该月行</returns>
    private static bool TryResolveProductMonthlyCost(
        IReadOnlyList<TaktBomMaterialCost> productRows,
        string period,
        out decimal cost)
    {
        cost = 0m;
        var header = productRows
            .Where(r => string.Equals(r.CostingPeriod, period, StringComparison.Ordinal))
            .OrderByDescending(r => r.CostingDate)
            .FirstOrDefault();
        if (header == null)
        {
            return false;
        }
        cost = header.ProductMonthlyCost;
        return true;
    }

    /// <summary>
    /// 构建机种汇总（各月成品平均成本）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <param name="modelName">机种名称</param>
    /// <param name="rows">全部转置行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>机种汇总</returns>
    private static TaktBomMaterialCostAnalysisModelSummaryDto BuildModelSummary(
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        var averagePeriodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var costs = rows
                .Where(r => r.PeriodCosts.TryGetValue(period, out var cost) && cost > 0m)
                .Select(r => r.PeriodCosts[period])
                .ToList();
            if (costs.Count > 0)
            {
                averagePeriodCosts[period] = TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
            }
        }
        return new TaktBomMaterialCostAnalysisModelSummaryDto
        {
            ModelCode = modelCode,
            ModelName = modelName,
            ProductCount = rows.Count,
            AveragePeriodCosts = averagePeriodCosts,
        };
    }

    /// <summary>
    /// 构建组件差异行
    /// </summary>
    /// <param name="baseRow">基准行</param>
    /// <param name="compareRow">对比行</param>
    /// <returns>差异行 DTO</returns>
    private static TaktBomMaterialCostAnalysisVarianceLineDto BuildVarianceLine(
        TaktBomMaterialCostItem? baseRow,
        TaktBomMaterialCostItem? compareRow)
    {
        var baseUnitPrice = baseRow != null
            ? TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(baseRow)
            : 0m;
        var compareUnitPrice = compareRow != null
            ? TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(compareRow)
            : 0m;
        var baseQty = baseRow?.ComponentQuantity ?? 0m;
        var compareQty = compareRow?.ComponentQuantity ?? 0m;
        // 移动价格口径：单价=MAP÷价格单位（已除单位）；行金额=数量×单价
        var baseCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(baseQty * baseUnitPrice);
        var compareCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareQty * compareUnitPrice);
        var unitPriceVariance = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareUnitPrice - baseUnitPrice);
        var quantityVariance = compareQty - baseQty;
        var priceEffect = TaktBomMaterialCostItemLineCostHelper.RoundCost(unitPriceVariance * baseQty);
        var quantityEffect = TaktBomMaterialCostItemLineCostHelper.RoundCost(quantityVariance * baseUnitPrice);
        var varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareCost - baseCost);
        decimal? variancePercent = null;
        if (baseCost != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(varianceAmount / baseCost);
        }
        var changeType = ResolveVarianceChangeType(baseRow, compareRow, unitPriceVariance, quantityVariance);
        return new TaktBomMaterialCostAnalysisVarianceLineDto
        {
            BomItemCode = compareRow?.BomItemCode ?? baseRow?.BomItemCode ?? string.Empty,
            ComponentCode = compareRow?.ComponentCode ?? baseRow?.ComponentCode ?? string.Empty,
            ComponentDescription = compareRow?.ComponentDescription ?? baseRow?.ComponentDescription ?? string.Empty,
            PurchaseType = compareRow?.PurchaseType ?? baseRow?.PurchaseType ?? string.Empty,
            CurrencyCode = compareRow != null
                ? TaktBomMaterialCostItemLineCostHelper.ResolveCurrency(compareRow)
                : (baseRow != null ? TaktBomMaterialCostItemLineCostHelper.ResolveCurrency(baseRow) : string.Empty),
            BaseCost = baseCost,
            CompareCost = compareCost,
            VarianceAmount = varianceAmount,
            VariancePercent = variancePercent,
            BaseUnitPrice = baseUnitPrice,
            CompareUnitPrice = compareUnitPrice,
            UnitPriceVariance = unitPriceVariance,
            BaseQuantity = baseQty,
            CompareQuantity = compareQty,
            QuantityVariance = quantityVariance,
            PriceEffectAmount = priceEffect,
            QuantityEffectAmount = quantityEffect,
            ChangeType = changeType,
        };
    }

    /// <summary>
    /// 解析组件变动类型
    /// </summary>
    /// <param name="baseRow">基准行</param>
    /// <param name="compareRow">对比行</param>
    /// <param name="unitPriceVariance">单价差异</param>
    /// <param name="quantityVariance">数量差异</param>
    /// <returns>变动类型码</returns>
    private static string ResolveVarianceChangeType(
        TaktBomMaterialCostItem? baseRow,
        TaktBomMaterialCostItem? compareRow,
        decimal unitPriceVariance,
        decimal quantityVariance)
    {
        if (baseRow == null && compareRow != null)
        {
            return "new";
        }
        if (baseRow != null && compareRow == null)
        {
            return "removed";
        }
        var hasPrice = unitPriceVariance != 0m;
        var hasQty = quantityVariance != 0m;
        if (hasPrice && hasQty)
        {
            return "mixed";
        }
        if (hasPrice)
        {
            return "price";
        }
        if (hasQty)
        {
            return "quantity";
        }
        return "unchanged";
    }

    /// <summary>
    /// 解析月度涨跌分析涉及的产品编码列表
    /// </summary>
    /// <param name="rows">已加载汇总行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="productCode">单产品编码；为空表示机种下全部物料</param>
    /// <returns>产品编码列表</returns>
    private static List<string> ResolveProductCodesInScope(
        IReadOnlyList<TaktBomMaterialCost> rows,
        string plantCode,
        string modelCode,
        string? productCode)
    {
        if (!string.IsNullOrWhiteSpace(productCode))
        {
            return new List<string> { productCode.Trim() };
        }
        return rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(r.ModelCode, modelCode, StringComparison.OrdinalIgnoreCase))
            .Select(r => r.ProductCode)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList()!;
    }

    /// <summary>
    /// 取产品在指定期间的汇总月成本
    /// </summary>
    /// <param name="rows">汇总行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="period">期间 yyyy-MM</param>
    /// <returns>产品月成本</returns>
    private static decimal ResolveProductMonthlyCostFromHeaders(
        IReadOnlyList<TaktBomMaterialCost> rows,
        string plantCode,
        string modelCode,
        string productCode,
        string period)
    {
        var header = rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(r.ModelCode, modelCode, StringComparison.OrdinalIgnoreCase)
                && TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode)
                && string.Equals(r.CostingPeriod, period, StringComparison.Ordinal))
            .OrderByDescending(r => r.CostingDate)
            .FirstOrDefault();
        return header?.ProductMonthlyCost ?? 0m;
    }

    /// <summary>
    /// 将 yyyy-MM 起止解析为核算日期范围
    /// </summary>
    /// <param name="periodStart">起始年月</param>
    /// <param name="periodEnd">结束年月</param>
    /// <returns>起止日期（可空）</returns>
    private static (DateTime? Start, DateTime? End) ResolvePeriodRangeBounds(string? periodStart, string? periodEnd)
    {
        DateTime? start = null;
        DateTime? end = null;
        if (!string.IsNullOrWhiteSpace(periodStart))
        {
            var (rangeStart, _) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodStart.Trim());
            start = rangeStart;
        }
        if (!string.IsNullOrWhiteSpace(periodEnd))
        {
            var (_, rangeEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodEnd.Trim());
            end = rangeEnd;
        }
        return (start, end);
    }

    /// <summary>
    /// 计算环比涨跌指标
    /// </summary>
    /// <param name="currentCost">当月成本</param>
    /// <param name="previousCost">上月成本</param>
    /// <returns>差额、百分比、涨跌码</returns>
    private static (decimal? VarianceAmount, decimal? VariancePercent, string Trend) ComputeMonthOverMonthTrend(
        decimal currentCost,
        decimal? previousCost)
    {
        if (!previousCost.HasValue)
        {
            return (null, null, "none");
        }
        var varianceAmount = currentCost - previousCost.Value;
        decimal? variancePercent = null;
        if (previousCost.Value != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(
                varianceAmount / previousCost.Value);
        }
        string trend;
        if (currentCost > previousCost.Value)
        {
            trend = "up";
        }
        else if (currentCost < previousCost.Value)
        {
            trend = "down";
        }
        else
        {
            trend = "flat";
        }
        return (varianceAmount, variancePercent, trend);
    }

    /// <summary>
    /// 机种合并组件中间结果
    /// </summary>
    private sealed class MergedBomComponent
    {
        /// <summary>
        /// 组件编码
        /// </summary>
        public string ComponentCode { get; set; } = string.Empty;

        /// <summary>
        /// 组件描述
        /// </summary>
        public string ComponentDescription { get; set; } = string.Empty;

        /// <summary>
        /// 共用产品编码集合
        /// </summary>
        public HashSet<string> ProductCodes { get; } = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 机种编码 → 产品编码集合写入查询（明细行无 ModelCode；未显式指定产品时缩小列表/导出范围）
    /// </summary>
    /// <param name="queryDto">明细查询条件（含 ModelCode 时回填 ProductCodes）</param>
    /// <returns>异步任务</returns>
    private async Task ApplyModelCodeProductScopeAsync(TaktBomMaterialCostItemQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        if (string.IsNullOrWhiteSpace(queryDto.ModelCode)
            || !string.IsNullOrWhiteSpace(queryDto.ProductCode)
            || (queryDto.ProductCodes != null && queryDto.ProductCodes.Count > 0))
        {
            return;
        }
        var plantCode = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(plantCode))
        {
            return;
        }
        var productCodes = await LoadModelProductCodesAsync(plantCode, queryDto.ModelCode.Trim());
        queryDto.ProductCodes = productCodes.Count > 0
            ? productCodes
            : new List<string> { "__no_model_product__" };
    }

    /// <summary>
    /// 从主表取机种下产品编码（可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <param name="materialType">物料类型（本表 MaterialType；空=不按类型过滤）</param>
    /// <returns>产品编码列表</returns>
    private async Task<List<string>> LoadModelProductCodesAsync(
        string plantCode,
        string modelCode,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null,
        string? materialType = null)
    {
        var type = NormalizeMaterialTypeFilter(materialType);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ModelCode == modelCode);
        if (type != null)
        {
            var mt = type;
            exp = exp.And(x => x.MaterialType == mt);
        }
        if (costingMonthStart.HasValue)
        {
            var start = costingMonthStart.Value;
            exp = exp.And(x => x.CostingDate >= start);
        }
        if (costingMonthEnd.HasValue)
        {
            var endExclusive = costingMonthEnd.Value.AddMonths(1);
            exp = exp.And(x => x.CostingDate < endExclusive);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .Select(h => h.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 加载机种产品 BOM 明细（仅 ProductionRelated=X 且 PurchaseType=F；可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月起（月初，含）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月）</param>
    /// <returns>过滤后的明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd)
    {
        var allItems = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        // 10/18 位 SAP 码互认：展开查询变体后再 Contains，避免明细表空结果
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return allItems;
        }
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktBomMaterialCostItem>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.ProductCode));
            if (costingMonthStart.HasValue)
            {
                var start = costingMonthStart.Value;
                exp = exp.And(x => x.CostingDate >= start);
            }
            if (costingExclusiveEnd.HasValue)
            {
                var endExclusive = costingExclusiveEnd.Value;
                exp = exp.And(x => x.CostingDate < endExclusive);
            }
            string? yearTable;
            try
            {
                yearTable = await ResolveBomItemPhysicalTableAsync(
                    TaktYearShardTableHelper.RequireSingleYear(costingMonthStart, costingMonthEnd));
            }
            catch (ArgumentException ex)
            {
                throw new TaktBusinessException(ex.Message);
            }
            var part = await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
            allItems.AddRange(TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(part));
        }
        return allItems;
    }


    /// <summary>
    /// 转置行全量合计：各期间成本 + 环比差额（分页前）
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumTransposedRowGrandTotals(
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        return SumPeriodAndVarianceGrandTotals(
            periodOrder,
            rows.Select(r => (PeriodMap: (IReadOnlyDictionary<string, decimal>)r.PeriodCosts, r.VarianceAmount)));
    }


    /// <summary>
    /// 对各行期间字典与环比差额做全量求和
    /// </summary>
    /// <param name="periodOrder">期间列</param>
    /// <param name="rows">期间映射 + 环比差额</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumPeriodAndVarianceGrandTotals(
        IReadOnlyList<string> periodOrder,
        IEnumerable<(IReadOnlyDictionary<string, decimal> PeriodMap, decimal? VarianceAmount)> rows)
    {
        var rowList = rows as IReadOnlyList<(IReadOnlyDictionary<string, decimal> PeriodMap, decimal? VarianceAmount)>
            ?? rows.ToList();
        var periodCostTotals = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            decimal sum = 0m;
            var hasValue = false;
            foreach (var (periodMap, _) in rowList)
            {
                if (!periodMap.TryGetValue(period, out var value))
                {
                    continue;
                }
                sum += value;
                hasValue = true;
            }
            if (hasValue)
            {
                periodCostTotals[period] = TaktBomMaterialCostItemLineCostHelper.RoundCost(sum);
            }
        }
        decimal varianceSum = 0m;
        var hasVariance = false;
        foreach (var (_, varianceAmount) in rowList)
        {
            if (varianceAmount == null)
            {
                continue;
            }
            varianceSum += varianceAmount.Value;
            hasVariance = true;
        }
        decimal? varianceAmountTotal = hasVariance
            ? TaktBomMaterialCostItemLineCostHelper.RoundCost(varianceSum)
            : null;
        return (periodCostTotals, varianceAmountTotal);
    }

    // ========================================
    // 按年分表路由（{base}_{yyyy}）
    // ========================================

    /// <summary>
    /// 生成 BOM 成本明细年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
    /// 生成移动价格年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildMovingPriceYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(MovingPriceYearShardBaseTable, year);

    /// <summary>
    /// 解析 BOM 成本明细物理表：年分表存在则用之，否则 null（回退实体基表，兼容 SAP 同步）
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 解析移动价格物理表：年分表存在则用之，否则 null（回退基表）
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveMovingPricePhysicalTableAsync(int year)
    {
        var table = BuildMovingPriceYearTable(year);
        return await _materialMovingPriceRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 按 Id 在近年分表中定位 BOM 成本明细（跳过未建年分表，最后查基表）
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>实体与物理表名（基表时 Table 为 null）</returns>
    private async Task<(TaktBomMaterialCostItem? Entity, string? Table)> FindBomItemByIdAsync(long id)
    {
        var now = DateTime.Now.Year;
        for (var y = now + 1; y >= now - YearShardProbeYears + 1; y--)
        {
            var table = await ResolveBomItemPhysicalTableAsync(y);
            if (table == null)
            {
                continue;
            }
            var entity = await _bomMaterialCostItemRepository.GetByIdAsync(id, table);
            if (entity != null)
            {
                return (entity, table);
            }
        }
        var baseEntity = await _bomMaterialCostItemRepository.GetByIdAsync(id);
        return (baseEntity, null);
    }

    /// <summary>
    /// 年分表（或基表）内唯一性校验
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="yearTable">物理表；null 表示基表</param>
    /// <param name="excludeId">排除 Id</param>
    private async Task EnsureBomItemUniqueAsync(
        TaktBomMaterialCostItem entity,
        string? yearTable,
        long? excludeId = null)
    {
        var existing = await _bomMaterialCostItemRepository.FirstAsync(
            x => x.PlantCode == entity.PlantCode
                && x.ProductCode == entity.ProductCode
                && x.SequenceCode == entity.SequenceCode
                && x.BomLevel == entity.BomLevel
                && x.BomItemCode == entity.BomItemCode
                && x.ComponentCode == entity.ComponentCode
                && x.ComponentQuantity == entity.ComponentQuantity
                && x.BatchIndicator == entity.BatchIndicator
                && x.ProductionRelated == entity.ProductionRelated
                && x.PurchaseType == entity.PurchaseType
                && x.SpecialProcurementType == entity.SpecialProcurementType
                && x.CostingDate == entity.CostingDate,
            yearTable);
        if (existing != null && (!excludeId.HasValue || existing.Id != excludeId.Value))
        {
            throw new TaktBusinessException("BOM物料成本明细的PlantCode、ProductCode、SequenceCode、BomLevel、BomItemCode、ComponentCode、ComponentQuantity、BatchIndicator、ProductionRelated、PurchaseType、SpecialProcurementType、CostingDate已存在");
        }
    }

    /// <summary>
    /// 按年分表查询 BOM 成本明细（可跨年合并；年分表未建时回退基表）
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="start">起</param>
    /// <param name="end">止</param>
    /// <param name="maxRows">总行上限（可选）</param>
    /// <returns>明细列表</returns>
    private async Task<List<TaktBomMaterialCostItem>> GetBomItemListForRangeAsync(
        Expression<Func<TaktBomMaterialCostItem, bool>> predicate,
        DateTime? start,
        DateTime? end,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYears(start, end);
        var result = new List<TaktBomMaterialCostItem>();
        var yearsNeedBase = new List<int>();
        foreach (var year in years)
        {
            var table = await ResolveBomItemPhysicalTableAsync(year);
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
                var part = await _bomMaterialCostItemRepository.GetListForExportAsync(predicate, remaining, table);
                result.AddRange(part);
            }
            else
            {
                var part = await _bomMaterialCostItemRepository.GetListAsync(predicate, table);
                result.AddRange(part);
            }
        }
        if (yearsNeedBase.Count == 0)
        {
            // 年分表与基表合并：SAP 同步常写基表，年分表可能仅部分数据；按 Id 去重
            if (!maxRows.HasValue || result.Count < maxRows.Value)
            {
                List<TaktBomMaterialCostItem> baseFallback;
                if (maxRows.HasValue)
                {
                    baseFallback = await _bomMaterialCostItemRepository.GetListForExportAsync(
                        predicate, maxRows.Value - result.Count);
                }
                else
                {
                    baseFallback = await _bomMaterialCostItemRepository.GetListAsync(predicate);
                }
                var yearSet = years.ToHashSet();
                var seenIds = result.Select(r => r.Id).ToHashSet();
                foreach (var row in baseFallback.Where(r => yearSet.Contains(r.CostingDate.Year)))
                {
                    if (!seenIds.Add(row.Id))
                    {
                        continue;
                    }
                    result.Add(row);
                    if (maxRows.HasValue && result.Count >= maxRows.Value)
                    {
                        break;
                    }
                }
            }
            return result;
        }
        if (maxRows.HasValue && result.Count >= maxRows.Value)
        {
            return result;
        }
        List<TaktBomMaterialCostItem> basePart;
        if (maxRows.HasValue)
        {
            var remaining = maxRows.Value - result.Count;
            basePart = await _bomMaterialCostItemRepository.GetListForExportAsync(predicate, remaining);
        }
        else
        {
            basePart = await _bomMaterialCostItemRepository.GetListAsync(predicate);
        }
        if (yearsNeedBase.Count == years.Count)
        {
            result.AddRange(basePart);
        }
        else
        {
            var yearSet = yearsNeedBase.ToHashSet();
            result.AddRange(basePart.Where(r => yearSet.Contains(r.CostingDate.Year)));
        }
        return result;
    }

    /// <summary>
    /// 按年分表查询移动价格（可跨年合并；年分表未建时回退基表）
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="valuationPeriodStart">评估期间起（yyyy-MM）</param>
    /// <param name="valuationPeriodEnd">评估期间止（yyyy-MM）</param>
    /// <param name="maxRows">总行上限（可选）</param>
    /// <returns>移动价格列表</returns>
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
}
