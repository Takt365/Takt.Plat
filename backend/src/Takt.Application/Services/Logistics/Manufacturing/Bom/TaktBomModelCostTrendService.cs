// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomModelCostTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 机种成本推移分析服务（读 BOM 成本本表；与明细 CRUD 服务分离）
/// </summary>
public class TaktBomModelCostTrendService : TaktServiceBase, ITaktBomModelCostTrendService
{
    /// <summary>BOM 成本明细按年分表基表名（与 SugarTable 一致）</summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomModelCostTrendService(
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

    /// <summary>
    /// 规范化物料类型筛选（空=不按类型过滤）
    /// </summary>
    /// <param name="materialType">查询传入类型</param>
    /// <returns>非空类型码；空则 null</returns>
    private static string? NormalizeMaterialTypeFilter(string? materialType)
    {
        var trimmed = materialType?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    /// <summary>
    /// 构建机种名称查找表（型号目的地 ModelCode → ModelName）
    /// </summary>
    /// <returns>机种编码→名称</returns>
    private async Task<Dictionary<string, string>> BuildBomModelCostTrendModelNameLookupAsync()
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
    /// 按产品解析机种（型号目的地优先，其次 BOM 成本汇总）
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>机种编码；未命中为 null</returns>
    private async Task<string?> ResolveModelCodeByProductAsync(string productCode, string? plantCode = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        EnsureThreeLayerContext();
        var trimmedProductCode = productCode.Trim();
        var destinations = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode);
        var match = destinations.FirstOrDefault(d =>
            TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(d.MaterialCode, trimmedProductCode));
        if (match != null && !string.IsNullOrWhiteSpace(match.ModelCode))
        {
            return match.ModelCode.Trim();
        }
        var trimmedPlant = plantCode?.Trim();
        var costRows = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.CostingDate,
            true);
        var modelCode = costRows
            .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, trimmedProductCode))
            .Select(x => x.ModelCode?.Trim())
            .FirstOrDefault(code => !string.IsNullOrWhiteSpace(code));
        return string.IsNullOrWhiteSpace(modelCode) ? null : modelCode;
    }

    /// <summary>
    /// 机种选项（分析：工厂 + 期间最后月头表机种去重；❌ 非 CRUD 主数据 TaktModelDestination）
    /// </summary>
    /// <param name="queryDto">工厂与 FocusPeriod（yyyy-MM）</param>
    /// <returns>下拉选项 DictValue=ModelCode</returns>
    public async Task<List<TaktSelectOption>> GetBomModelCostTrendModelOptionsAsync(
        TaktBomModelCostTrendOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        if (!TryParsePeriodMonth(queryDto.FocusPeriod, out var lastMonth))
        {
            return new List<TaktSelectOption>();
        }
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var models = await LoadLastMonthModelCodesAsync(queryDto.PlantCode.Trim(), lastMonth, materialType);
        return models
            .Select(code => new TaktSelectOption
            {
                DictValue = code,
                DictLabel = code,
            })
            .ToList();
    }

    /// <summary>
    /// 物料/组件选项（工厂 + 期间最后月 + ProductionRelated=X + PurchaseType=F + 未删除去重；支持 keyword 远程搜索）
    /// </summary>
    /// <param name="queryDto">工厂、FocusPeriod、可选 Keyword</param>
    /// <returns>下拉选项 DictValue=ComponentCode</returns>
    public async Task<List<TaktSelectOption>> GetBomModelCostTrendComponentOptionsAsync(
        TaktBomModelCostTrendOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        if (!TryParsePeriodMonth(queryDto.FocusPeriod, out var lastMonth))
        {
            return new List<TaktSelectOption>();
        }
        var plant = queryDto.PlantCode.Trim();
        // 工厂 + 期间最后月 + X + F + is_deleted=0 去重（可选 keyword 远程过滤）
        var options = await LoadLastMonthComponentOptionsAsync(plant, lastMonth, queryDto.Keyword);
        return options
            .Select(o => new TaktSelectOption
            {
                DictValue = o.Code,
                DictLabel = string.IsNullOrWhiteSpace(o.Description) ? o.Code : $"{o.Code} {o.Description}",
            })
            .ToList();
    }

    /// <summary>
    /// 解析 yyyy-MM 为当月首日
    /// </summary>
    /// <param name="period">yyyy-MM</param>
    /// <param name="monthStart">月初</param>
    /// <returns>是否解析成功</returns>
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

    /// <summary>
    /// 期间最后月头表机种去重（按 MaterialType，默认 FERT）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="lastMonth">期间最后月月初</param>
    /// <param name="materialType">物料类型</param>
    /// <returns>机种编码列表</returns>
    private async Task<List<string>> LoadLastMonthModelCodesAsync(
        string plantCode,
        DateTime lastMonth,
        string? materialType = null)
    {
        var type = NormalizeMaterialTypeFilter(materialType);
        var periodKey = lastMonth.ToString("yyyy-MM");
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
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
            .ToList();
    }

    /// <summary>
    /// 期间最后月 X+F 组件去重（工厂+核算月+X+F+未删除；SQL GROUP BY；可选关键字；不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="lastMonth">期间最后月月初</param>
    /// <param name="keyword">远程搜索关键字（编码/描述；可空=全量去重）</param>
    /// <returns>组件编码+描述（按编码排序）</returns>
    private async Task<List<(string Code, string Description)>> LoadLastMonthComponentOptionsAsync(
        string plantCode,
        DateTime lastMonth,
        string? keyword = null)
    {
        var yearTable = await ResolveBomItemPhysicalTableAsync(lastMonth.Year);
        var rows = await QueryDistinctComponentOptionsAsync(
            yearTable ?? BomItemYearShardBaseTable,
            plantCode,
            lastMonth,
            keyword);
        // 年分表无数据时回退基表（与明细加载一致）
        if (rows.Count == 0 && yearTable != null)
        {
            rows = await QueryDistinctComponentOptionsAsync(
                BomItemYearShardBaseTable,
                plantCode,
                lastMonth,
                keyword);
        }
        return rows
            .OrderBy(r => r.Code, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 指定物理表查询组件去重 options（不截断；前端 virtual + remote-search）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="lastMonth">核算月月初</param>
    /// <param name="keyword">关键字</param>
    /// <returns>组件编码+描述</returns>
    private async Task<List<(string Code, string Description)>> QueryDistinctComponentOptionsAsync(
        string tableName,
        string plantCode,
        DateTime lastMonth,
        string? keyword)
    {
        var monthEndExclusive = lastMonth.AddMonths(1);
        var kw = keyword?.Trim();
        var sql = new StringBuilder();
        sql.Append(
            """
            SELECT
              LTRIM(RTRIM(component_code)) AS ComponentCode,
              MAX(LTRIM(RTRIM(ISNULL(component_description, N'')))) AS ComponentDescription
            FROM 
            """);
        sql.Append(tableName.Trim());
        sql.Append(
            """
             WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND production_related = N'X'
              AND purchase_type = N'F'
              AND UPPER(LTRIM(RTRIM(ISNULL(pcb_sect_indicator, N'')))) <> N'X'
              AND component_code IS NOT NULL
              AND LTRIM(RTRIM(component_code)) <> N''
              AND costing_date >= @costingStart
              AND costing_date < @costingEndExclusive
            """);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["costingStart"] = lastMonth,
            ["costingEndExclusive"] = monthEndExclusive,
        };
        if (!string.IsNullOrEmpty(kw))
        {
            sql.Append(
                """
                  AND (
                    component_code LIKE @keywordLike
                    OR component_description LIKE @keywordLike
                  )
                """);
            parameters["keywordLike"] = "%" + EscapeSqlLikePattern(kw) + "%";
        }
        sql.Append(
            """
             GROUP BY LTRIM(RTRIM(component_code))
            ORDER BY ComponentCode
            """);
        var script = sql.ToString();
        TaktSqlExecutorValidator.Validate(script);
        var raw = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        var result = new List<(string Code, string Description)>(raw.Count);
        foreach (var row in raw)
        {
            var code = ReadSqlString(row, "ComponentCode");
            if (string.IsNullOrWhiteSpace(code))
            {
                continue;
            }
            result.Add((code, ReadSqlString(row, "ComponentDescription")));
        }
        return result;
    }

    /// <summary>
    /// 转义 LIKE 通配符（字面匹配用户输入）
    /// </summary>
    /// <param name="value">原始关键字</param>
    /// <returns>转义后</returns>
    private static string EscapeSqlLikePattern(string value) =>
        value
            .Replace("[", "[[]", StringComparison.Ordinal)
            .Replace("%", "[%]", StringComparison.Ordinal)
            .Replace("_", "[_]", StringComparison.Ordinal);

    /// <summary>
    /// 读取只读 SQL 行字符串列
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="column">列名</param>
    /// <returns>Trim 后文本</returns>
    private static string ReadSqlString(IReadOnlyDictionary<string, object> row, string column)
    {
        if (!row.TryGetValue(column, out var value) || value == null || value is DBNull)
        {
            return string.Empty;
        }
        return Convert.ToString(value)?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 机种成本推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    public async Task<TaktBomModelCostTrendResultDto> GetBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildModelCostTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        var (periodCostTotals, varianceAmountTotal) = SumModelCostTrendRowGrandTotals(
            built.OrderedRows, built.PeriodOrder);
        return new TaktBomModelCostTrendResultDto
        {
            Paged = TaktPagedResult<TaktBomModelCostTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            ProductCodes = built.ProductCodes,
            ModelPeriodMaterialCosts = built.ModelPeriodMaterialCosts,
            ModelTrend = built.ModelTrend,
            ModelBasePeriod = built.ModelBasePeriod,
            ModelComparePeriod = built.ModelComparePeriod,
            ModelVarianceAmount = built.ModelVarianceAmount,
            ModelVariancePercent = built.ModelVariancePercent,
            ComponentCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
    }

    /// <summary>
    /// 导出机种成本推移分析
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildModelCostTrendAnalysisAsync(query);
        var isDetail = IsModelCostDetailMergeMode(query.MergeMode);
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "productCodes", "componentCode", "componentDescription",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种组", "产品组", "组件", "组件描述",
        };
        if (isDetail)
        {
            columnKeys.AddRange(new[]
            {
                "componentQuantity", "batchIndicator", "productionRelated", "purchaseType",
                "specialProcurementType", "profitCenterCode",
            });
            columnLabels.AddRange(new[]
            {
                "组件数量", "批量标识", "生产相关", "采购类型",
                "特殊采购类", "利润中心",
            });
        }
        else
        {
            columnKeys.AddRange(new[] { "productionRelated", "purchaseType" });
            columnLabels.AddRange(new[] { "生产相关", "采购类型" });
        }
        columnKeys.AddRange(new[] { "productCount", "currencyCode" });
        columnLabels.AddRange(new[] { "产品数", "币种" });
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });

        var exportRows = built.OrderedRows
            .Select(row =>
            {
                var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["plantCode"] = row.PlantCode,
                    ["modelCode"] = row.ModelCode,
                    ["componentCode"] = row.ComponentCode,
                    ["componentDescription"] = row.ComponentDescription,
                    ["componentQuantity"] = row.ComponentQuantity,
                    ["batchIndicator"] = row.BatchIndicator,
                    ["productionRelated"] = row.ProductionRelated,
                    ["purchaseType"] = row.PurchaseType,
                    ["specialProcurementType"] = row.SpecialProcurementType,
                    ["profitCenterCode"] = row.ProfitCenterCode,
                    ["productCodes"] = row.ProductCodes,
                    ["productCount"] = row.ProductCount,
                    ["currencyCode"] = row.CurrencyCode,
                    ["basePeriod"] = row.BasePeriod,
                    ["comparePeriod"] = row.ComparePeriod,
                    ["varianceAmount"] = row.VarianceAmount,
                    ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(row.VariancePercent),
                    ["trend"] = row.Trend,
                };
                foreach (var period in built.PeriodOrder)
                {
                    dict[$"period_{period}"] = row.PeriodMaterialCosts.TryGetValue(period, out var cost)
                        ? cost
                        : null;
                }
                return (IReadOnlyDictionary<string, object?>)dict;
            })
            .ToList();

        var defaultSheet = isDetail ? "DTA 差异组件推移表" : "DTA BOM通用组件成本推移表";
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? defaultSheet,
            fileName ?? $"{defaultSheet}.xlsx");
    }

    /// <summary>
    /// 从主表取机种下产品编码（可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <returns>产品编码列表</returns>
    private async Task<List<string>> LoadModelProductCodesAsync(
        string plantCode,
        string modelCode,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null)
    {
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ModelCode == modelCode);
        // 与唯一键 CostingPeriod（yyyy-MM）对齐，避免仅靠 CostingDate 漏月
        if (costingMonthStart.HasValue)
        {
            var startPeriod = costingMonthStart.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(startPeriod) >= 0);
        }
        if (costingMonthEnd.HasValue)
        {
            var endPeriod = costingMonthEnd.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(endPeriod) <= 0);
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
    /// 加载机种产品 BOM 明细（全量展开后 Filter：生产相关=X、PCB SECT 标识为空、采购类型=F；可按核算月过滤；不截断）
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
            var part = await GetBomItemListForRangeAsync(
                exp.ToExpression(),
                costingMonthStart,
                costingMonthEnd);
            allItems.AddRange(TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(part));
        }
        return allItems;
    }

    /// <summary>
    /// 按工厂+期间直查明细（全量展开后 Filter：生产相关=X、PCB SECT 标识为空、采购类型=F；不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingMonthStart">核算月起</param>
    /// <param name="costingMonthEnd">核算月止</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForPlantPeriodAsync(
        string plantCode,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd)
    {
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ComponentCode != null
            && x.ComponentCode != "");
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
        var part = await GetBomItemListForRangeAsync(
            exp.ToExpression(),
            costingMonthStart,
            costingMonthEnd);
        return TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(part).ToList();
    }

    /// <summary>
    /// 按物料/组件编码直查明细（全量展开后 Filter：生产相关=X、PCB SECT 标识为空、采购类型=F；不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="componentCodes">组件编码</param>
    /// <param name="costingMonthStart">核算月起</param>
    /// <param name="costingMonthEnd">核算月止</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsByComponentsAsync(
        string plantCode,
        IReadOnlyList<string> componentCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd)
    {
        var seedItems = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 100;
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        var codes = componentCodes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (codes.Count == 0)
        {
            return new List<TaktBomMaterialCostItem>();
        }
        for (var i = 0; i < codes.Count; i += chunkSize)
        {
            var chunk = codes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktBomMaterialCostItem>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.ComponentCode));
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
            var part = await GetBomItemListForRangeAsync(
                exp.ToExpression(),
                costingMonthStart,
                costingMonthEnd);
            seedItems.AddRange(part);
        }
        // 按组件命中的产品拉全量展开，再 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F）
        var productCodes = seedItems
            .Where(x => !string.IsNullOrWhiteSpace(x.ProductCode))
            .Select(x => x.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (productCodes.Count == 0)
        {
            return new List<TaktBomMaterialCostItem>();
        }
        var filtered = await LoadBomCostItemsForProductsAsync(
            plantCode, productCodes, costingMonthStart, costingMonthEnd);
        var codeSet = new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase);
        return filtered
            .Where(x => !string.IsNullOrWhiteSpace(x.ComponentCode) && codeSet.Contains(x.ComponentCode.Trim()))
            .ToList();
    }

    /// <summary>
    /// 按产品编码列表加载机种元数据（仅命中产品，避免全厂头表）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月起</param>
    /// <param name="costingMonthEnd">核算月止</param>
    /// <param name="materialType">物料类型（本表 MaterialType；空=默认 FERT）</param>
    /// <returns>产品 → 元数据</returns>
    private async Task<Dictionary<string, ModelProductMeta>> LoadProductMetaByProductCodesAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd,
        string? materialType = null)
    {
        var type = NormalizeMaterialTypeFilter(materialType);
        var map = new Dictionary<string, ModelProductMeta>(StringComparer.OrdinalIgnoreCase);
        const int chunkSize = 200;
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return map;
        }
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktBomMaterialCost>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.ProductCode));
            if (type != null)
            {
                var mt = type;
                exp = exp.And(x => x.MaterialType == mt);
            }
            if (costingMonthStart.HasValue)
            {
                var startPeriod = costingMonthStart.Value.ToString("yyyy-MM");
                exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(startPeriod) >= 0);
            }
            if (costingMonthEnd.HasValue)
            {
                var endPeriod = costingMonthEnd.Value.ToString("yyyy-MM");
                exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(endPeriod) <= 0);
            }
            var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
            foreach (var header in headers
                .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
                .OrderByDescending(h => h.CostingDate)
                .ThenByDescending(h => h.Id))
            {
                var code = header.ProductCode.Trim();
                if (map.ContainsKey(code))
                {
                    continue;
                }
                map[code] = new ModelProductMeta
                {
                    ModelCode = header.ModelCode?.Trim() ?? string.Empty,
                    Description = header.ProductDescription?.Trim() ?? string.Empty,
                    CurrencyCode = header.CurrencyCode?.Trim() ?? string.Empty,
                };
            }
        }
        return map;
    }

    /// <summary>
    /// 归一化移动价格期间上下界（存当月首日）
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizeMovingPricePeriodBounds(
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
    /// 构建核算期间列顺序（有起止则连续月序，否则取明细 CostingDate 出现的月）
    /// </summary>
    /// <param name="costItems">BOM 成本明细</param>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <returns>yyyy-MM 列表</returns>
    private static List<string> BuildCostingPeriodOrder(
        IReadOnlyList<TaktBomMaterialCostItem> costItems,
        DateTime? periodStart,
        DateTime? periodEnd)
    {
        if (periodStart.HasValue && periodEnd.HasValue)
        {
            var order = new List<string>();
            for (var cursor = periodStart.Value; cursor <= periodEnd.Value; cursor = cursor.AddMonths(1))
            {
                order.Add(cursor.ToString("yyyy-MM"));
            }
            return order;
        }
        return costItems
            .Select(r => new DateTime(r.CostingDate.Year, r.CostingDate.Month, 1).ToString("yyyy-MM"))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 解析关注期间；未指定时取期间列最后一月
    /// </summary>
    /// <param name="focusPeriod">查询关注期间</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>yyyy-MM 或 null</returns>
    private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 机种成本推移：主表产品元数据（机种 / 描述 / 币种）
    /// </summary>
    private sealed class ModelProductMeta
    {
        /// <summary>机种编码</summary>
        public string ModelCode { get; set; } = string.Empty;

        /// <summary>产品描述</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>币种</summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 解析多选编码（逗号/分号分隔；合并单值与多值字段；去空白去重）
    /// </summary>
    /// <param name="multiCodes">多选逗号串</param>
    /// <param name="singleCode">兼容单值</param>
    /// <returns>编码列表（可空表示不过滤）</returns>
    private static List<string>? ParseMultiCodes(string? multiCodes, string? singleCode)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void AddRaw(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return;
            }
            foreach (var part in raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    set.Add(part);
                }
            }
        }
        AddRaw(multiCodes);
        AddRaw(singleCode);
        return set.Count == 0 ? null : set.OrderBy(c => c, StringComparer.Ordinal).ToList();
    }

    /// <summary>
    /// 产品编码允许集（含 18 位/10 位变体），供明细 ProductCode 匹配
    /// </summary>
    /// <param name="productMeta">头表产品元数据</param>
    /// <returns>允许的产品编码集合</returns>
    private static HashSet<string> BuildAllowedProductCodeSet(
        IReadOnlyDictionary<string, ModelProductMeta> productMeta)
    {
        var allowed = new HashSet<string>(productMeta.Keys, StringComparer.OrdinalIgnoreCase);
        foreach (var code in productMeta.Keys)
        {
            foreach (var v in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(code))
            {
                allowed.Add(v);
            }
        }
        return allowed;
    }

    /// <summary>
    /// 按最后月命中明细收缩产品元数据（物料过滤后仅保留仍有组件的产品）
    /// </summary>
    /// <param name="productMeta">候选产品</param>
    /// <param name="hitProductCodes">最后月明细中的产品编码</param>
    /// <returns>收缩后的元数据</returns>
    private static Dictionary<string, ModelProductMeta> FilterProductMetaByHitProducts(
        IReadOnlyDictionary<string, ModelProductMeta> productMeta,
        IReadOnlyCollection<string> hitProductCodes)
    {
        var hits = hitProductCodes is HashSet<string> set
            ? set
            : hitProductCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return productMeta
            .Where(kv => hits.Contains(kv.Key)
                || TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(kv.Key)
                    .Any(v => hits.Contains(v)))
            .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 从主表加载产品编码及描述/币种/机种（可按机种列表、核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCodes">机种列表（null/空=工厂下全部机种产品）</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <param name="materialType">物料类型（本表 MaterialType；空=默认 FERT）</param>
    /// <returns>产品编码 → 元数据</returns>
    private async Task<Dictionary<string, ModelProductMeta>> LoadProductMetaAsync(
        string plantCode,
        IReadOnlyList<string>? modelCodes = null,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null,
        string? materialType = null)
    {
        var type = NormalizeMaterialTypeFilter(materialType);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        if (type != null)
        {
            var mt = type;
            exp = exp.And(x => x.MaterialType == mt);
        }
        if (modelCodes is { Count: 1 })
        {
            var model = modelCodes[0];
            exp = exp.And(x => x.ModelCode == model);
        }
        else if (modelCodes is { Count: > 1 })
        {
            var models = modelCodes.ToList();
            exp = exp.And(x => models.Contains(x.ModelCode));
        }
        if (costingMonthStart.HasValue)
        {
            var startPeriod = costingMonthStart.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(startPeriod) >= 0);
        }
        if (costingMonthEnd.HasValue)
        {
            var endPeriod = costingMonthEnd.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(endPeriod) <= 0);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        var map = new Dictionary<string, ModelProductMeta>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .OrderByDescending(h => h.CostingDate)
            .ThenByDescending(h => h.Id))
        {
            var code = header.ProductCode.Trim();
            if (map.ContainsKey(code))
            {
                continue;
            }
            map[code] = new ModelProductMeta
            {
                ModelCode = header.ModelCode?.Trim() ?? string.Empty,
                Description = header.ProductDescription?.Trim() ?? string.Empty,
                CurrencyCode = header.CurrencyCode?.Trim() ?? string.Empty,
            };
        }
        return map;
    }

    /// <summary>
    /// 从主表加载机种下产品编码及描述/币种（可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <returns>产品编码 → 元数据</returns>
    private Task<Dictionary<string, ModelProductMeta>> LoadModelProductMetaAsync(
        string plantCode,
        string modelCode,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null)
    {
        IReadOnlyList<string>? models = string.IsNullOrWhiteSpace(modelCode)
            ? null
            : new[] { modelCode.Trim() };
        return LoadProductMetaAsync(plantCode, models, costingMonthStart, costingMonthEnd);
    }

    /// <summary>
    /// 是否差异组件（detail）合并模式
    /// </summary>
    /// <param name="mergeMode">查询 MergeMode</param>
    /// <returns>true=detail</returns>
    private static bool IsModelCostDetailMergeMode(string? mergeMode)
    {
        return string.Equals(mergeMode?.Trim(), "detail", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 机种分析合并键。
    /// summary：Plant+ComponentCode+ProductionRelated+PurchaseType（跨机种合并一行）；
    /// detail：Plant+ModelCode+ComponentCode+ProductionRelated+PurchaseType（按机种看月度有无）
    /// </summary>
    /// <param name="item">明细行</param>
    /// <param name="modelCode">机种编码（detail 必填）</param>
    /// <param name="detailMode">是否差异组件推移</param>
    /// <returns>稳定键</returns>
    private static string BuildModelCostMergeKey(
        TaktBomMaterialCostItem item,
        string modelCode,
        bool detailMode)
    {
        if (detailMode)
        {
            return string.Join(
                "|",
                item.PlantCode?.Trim() ?? string.Empty,
                modelCode?.Trim() ?? string.Empty,
                item.ComponentCode?.Trim() ?? string.Empty,
                item.ProductionRelated?.Trim() ?? string.Empty,
                item.PurchaseType?.Trim() ?? string.Empty);
        }
        return string.Join(
            "|",
            item.PlantCode?.Trim() ?? string.Empty,
            item.ComponentCode?.Trim() ?? string.Empty,
            item.ProductionRelated?.Trim() ?? string.Empty,
            item.PurchaseType?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// 构建机种成本推移：
    /// 统一口径（机种/物料空与非空相同）：工厂 + 期间最后月 + MaterialType(默认 FERT) + 机种(可空) + 物料(可空)。
    /// 空=该维不过滤；❌ 禁止因物料非空改成全期间产品集。
    /// 期间明细仅用于各月成本列；ProductCount 固定取最后月命中产品。
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>排序后的全量行与汇总</returns>
    private async Task<ModelCostTrendAnalysisBuilt> BuildModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var detailMode = IsModelCostDetailMergeMode(queryDto.MergeMode);
        var modelFilters = ParseMultiCodes(queryDto.ModelCodes, queryDto.ModelCode);
        var componentFilters = ParseMultiCodes(queryDto.ComponentCodes, queryDto.ComponentCode);
        var (periodStart, periodEnd) = NormalizeMovingPricePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);

        var modelNameLookup = await BuildBomModelCostTrendModelNameLookupAsync();
        if (!periodStart.HasValue && !periodEnd.HasValue)
        {
            return ModelCostTrendAnalysisBuilt.Empty(new List<string>());
        }
        var rangeStart = periodStart ?? periodEnd!.Value;
        var rangeEnd = periodEnd ?? periodStart!.Value;
        var lastMonth = rangeEnd;

        List<string>? explicitModels = modelFilters is { Count: > 0 } ? modelFilters : null;
        List<string>? explicitComponents = componentFilters is { Count: > 0 } ? componentFilters : null;

        // ① 产品宇宙：工厂 + 最后月 + MaterialType + 机种(可空=最后月全部机种产品)
        var productMeta = await LoadProductMetaAsync(
            plantCode, explicitModels, lastMonth, lastMonth, materialType);
        if (productMeta.Count == 0)
        {
            return ModelCostTrendAnalysisBuilt.Empty(new List<string>());
        }
        var scopedProductCodes = productMeta.Keys.OrderBy(c => c, StringComparer.Ordinal).ToList();
        var allowedScoped = BuildAllowedProductCodeSet(productMeta);

        // ② 最后月明细：物料(可空) 过滤；组件清单与 ProductCount 均锚定于此
        var lastMonthItems = await LoadBomCostItemsForProductsAsync(
            plantCode, scopedProductCodes, lastMonth, lastMonth);
        lastMonthItems = lastMonthItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode)
                && allowedScoped.Contains(r.ProductCode.Trim())
                && !string.IsNullOrWhiteSpace(r.ComponentCode))
            .ToList();
        if (explicitComponents is { Count: > 0 })
        {
            var componentSet = explicitComponents.ToHashSet(StringComparer.OrdinalIgnoreCase);
            lastMonthItems = lastMonthItems
                .Where(r => componentSet.Contains(r.ComponentCode.Trim()))
                .ToList();
        }
        if (lastMonthItems.Count == 0)
        {
            return ModelCostTrendAnalysisBuilt.Empty(scopedProductCodes);
        }

        var hitProductCodes = lastMonthItems
            .Select(r => r.ProductCode.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        productMeta = FilterProductMetaByHitProducts(productMeta, hitProductCodes);
        var productCodes = productMeta.Keys.OrderBy(c => c, StringComparer.Ordinal).ToList();
        var allowedHit = BuildAllowedProductCodeSet(productMeta);
        var effectiveComponents = lastMonthItems
            .Select(r => r.ComponentCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // ③ 期间明细：仅各月成本；产品/组件范围与最后月口径一致
        var costItems = await LoadBomCostItemsForProductsAsync(
            plantCode, productCodes, rangeStart, rangeEnd);
        costItems = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode)
                && allowedHit.Contains(r.ProductCode.Trim())
                && !string.IsNullOrWhiteSpace(r.ComponentCode)
                && effectiveComponents.Contains(r.ComponentCode.Trim()))
            .ToList();
        if (costItems.Count == 0)
        {
            return ModelCostTrendAnalysisBuilt.Empty(productCodes);
        }

        var periodOrder = BuildCostingPeriodOrder(costItems, periodStart, periodEnd);
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);

        Dictionary<string, decimal> modelPeriodCosts;
        string modelTrend;
        string? modelBasePeriod;
        string? modelComparePeriod;
        decimal? modelVarianceAmount;
        decimal? modelVariancePercent;
        // 仅单机种筛选时汇总机种月材料成本；多机种/全机种不展示机种合计
        if (detailMode && modelFilters is { Count: 1 })
        {
            modelPeriodCosts = BuildModelPeriodMaterialCosts(productCodes, costItems, periodOrder);
            ApplyUnitPriceFocusTrend(
                modelPeriodCosts,
                focusPeriod,
                out modelTrend,
                out modelBasePeriod,
                out modelComparePeriod,
                out modelVarianceAmount,
                out modelVariancePercent);
        }
        else
        {
            modelPeriodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
            modelTrend = "none";
            modelBasePeriod = null;
            modelComparePeriod = null;
            modelVarianceAmount = null;
            modelVariancePercent = null;
        }

        string ResolveItemModel(TaktBomMaterialCostItem item)
        {
            var product = item.ProductCode?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(product))
            {
                return string.Empty;
            }
            if (productMeta.TryGetValue(product, out var meta)
                && !string.IsNullOrWhiteSpace(meta.ModelCode))
            {
                return meta.ModelCode.Trim();
            }
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(product))
            {
                if (productMeta.TryGetValue(variant, out meta)
                    && !string.IsNullOrWhiteSpace(meta.ModelCode))
                {
                    return meta.ModelCode.Trim();
                }
            }
            return string.Empty;
        }

        // ProductCount / 产品组：按合并键取最后月命中产品及该组件在各产品中的数量
        var lastMonthProductsByMergeKey = lastMonthItems
            .GroupBy(r => BuildModelCostMergeKey(r, ResolveItemModel(r), detailMode), StringComparer.Ordinal)
            .ToDictionary(
                g => g.Key,
                g => g.Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
                    .GroupBy(r => r.ProductCode.Trim(), StringComparer.OrdinalIgnoreCase)
                    .Select(pg =>
                    {
                        var picked = pg
                            .OrderByDescending(r => r.CostingDate)
                            .ThenByDescending(r => r.Id)
                            .First();
                        return (
                            ProductCode: picked.ProductCode.Trim(),
                            Quantity: picked.ComponentQuantity);
                    })
                    .OrderBy(x => x.ProductCode, StringComparer.Ordinal)
                    .ToList(),
                StringComparer.Ordinal);

        var mergeGroups = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(r => BuildModelCostMergeKey(r, ResolveItemModel(r), detailMode), StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .ToList();

        var allRows = mergeGroups
            .Select(group =>
            {
                var groupItems = group.ToList();
                var modelCodesInGroup = groupItems
                    .Select(ResolveItemModel)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(m => m, StringComparer.Ordinal)
                    .ToList();
                var rowModel = detailMode
                    ? (modelCodesInGroup.FirstOrDefault() ?? string.Empty)
                    : string.Join(",", modelCodesInGroup);
                var rowModelName = detailMode
                    ? (!string.IsNullOrWhiteSpace(rowModel)
                        && modelNameLookup.TryGetValue(rowModel, out var mn)
                        && !string.IsNullOrWhiteSpace(mn)
                            ? mn.Trim()
                            : rowModel)
                    : string.Join(",", modelCodesInGroup.Select(m =>
                        modelNameLookup.TryGetValue(m, out var name) && !string.IsNullOrWhiteSpace(name)
                            ? name.Trim()
                            : m));
                var productSet = lastMonthProductsByMergeKey.TryGetValue(group.Key, out var products)
                    ? products
                    : new List<(string ProductCode, decimal Quantity)>();
                return BuildModelMergeKeyMaterialCostRow(
                    plantCode,
                    rowModel,
                    rowModelName,
                    groupItems,
                    productSet,
                    periodOrder,
                    focusPeriod,
                    detailMode);
            })
            .Where(r => detailMode
                ? r.PeriodChangeTypes.Count > 0
                : r.PeriodMaterialCosts.Count > 0)
            .ToList();

        if (detailMode)
        {
            // 仅保留跨月有无差异（新增/剔除/部分月缺失）的组件清单
            allRows = allRows.Where(HasModelComponentPresenceVariance).ToList();
        }

        var filtered = FilterModelCostTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderModelCostTrendRows(filtered, queryDto.SortBy);
        return new ModelCostTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            ProductCodes = productCodes,
            ModelPeriodMaterialCosts = modelPeriodCosts,
            ModelTrend = modelTrend,
            ModelBasePeriod = modelBasePeriod,
            ModelComparePeriod = modelComparePeriod,
            ModelVarianceAmount = modelVarianceAmount,
            ModelVariancePercent = modelVariancePercent,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// 机种成本推移内存构建结果
    /// </summary>
    private sealed class ModelCostTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktBomModelCostTrendDto> OrderedRows { get; init; } = new();

        /// <summary>期间列顺序</summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>机种下产品编码</summary>
        public List<string> ProductCodes { get; init; } = new();

        /// <summary>机种各月材料成本</summary>
        public Dictionary<string, decimal> ModelPeriodMaterialCosts { get; init; } = new(StringComparer.Ordinal);

        /// <summary>机种环比涨跌</summary>
        public string ModelTrend { get; init; } = "none";

        /// <summary>机种环比基准月</summary>
        public string? ModelBasePeriod { get; init; }

        /// <summary>机种环比对比月</summary>
        public string? ModelComparePeriod { get; init; }

        /// <summary>机种环比差额</summary>
        public decimal? ModelVarianceAmount { get; init; }

        /// <summary>机种环比变动率</summary>
        public decimal? ModelVariancePercent { get; init; }

        /// <summary>基准期间</summary>
        public string? BasePeriod { get; init; }

        /// <summary>对比期间</summary>
        public string? ComparePeriod { get; init; }

        /// <summary>上涨行数</summary>
        public int UpCount { get; init; }

        /// <summary>下跌行数</summary>
        public int DownCount { get; init; }

        /// <summary>持平行数</summary>
        public int FlatCount { get; init; }

        /// <summary>无趋势行数</summary>
        public int NoneCount { get; init; }

        /// <summary>
        /// 空结果
        /// </summary>
        /// <param name="productCodes">产品编码</param>
        /// <returns>空构建结果</returns>
        public static ModelCostTrendAnalysisBuilt Empty(List<string> productCodes) => new()
        {
            ProductCodes = productCodes,
        };
    }

    /// <summary>
    /// 机种各月材料成本 = 各产品当月材料成本（&gt;0）算术平均
    /// </summary>
    /// <param name="productCodes">产品组</param>
    /// <param name="costItems">明细</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间 → 机种月成本</returns>
    private static Dictionary<string, decimal> BuildModelPeriodMaterialCosts(
        IReadOnlyList<string> productCodes,
        IReadOnlyList<TaktBomMaterialCostItem> costItems,
        IReadOnlyList<string> periodOrder)
    {
        var byProduct = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => r.ProductCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<TaktBomMaterialCostItem>)g.ToList(),
                StringComparer.OrdinalIgnoreCase);
        var result = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var productCosts = new List<decimal>();
            foreach (var productCode in productCodes)
            {
                if (!byProduct.TryGetValue(productCode, out var productItems))
                {
                    continue;
                }
                var monthCost = SumMaterialCostByLineKeyForPeriod(productItems, period);
                if (monthCost is > 0m)
                {
                    productCosts.Add(monthCost.Value);
                }
            }
            var average = TaktBomMaterialCostItemModelEnrichmentHelper
                .ComputeModelMonthlyAverageFromProductCosts(productCosts);
            if (average > 0m || productCosts.Count > 0)
            {
                result[period] = average;
            }
        }
        return result;
    }

    /// <summary>
    /// 构建合并键 × 月材料成本分析行。
    /// 各月材料成本 = 该组件在当月明细中的单行成本（取核算日最新一行），仅列表展示；
    /// ❌ 禁止按产品/机种数量加总或平均（与产品出现次数无关）。
    /// ProductCodes =「产品:组件数量」列表（英文逗号分隔）；ProductCount 取最后月命中产品数，不参与成本计算。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="modelName">机种名称</param>
    /// <param name="keyItems">同合并键期间明细（成本列）</param>
    /// <param name="lastMonthProductQuantities">最后月命中产品及组件数量（ProductCodes / ProductCount 口径）</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注月</param>
    /// <param name="detailMode">差异组件明细字段是否填充</param>
    /// <returns>分析行</returns>
    private static TaktBomModelCostTrendDto BuildModelMergeKeyMaterialCostRow(
        string plantCode,
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        IReadOnlyList<(string ProductCode, decimal Quantity)> lastMonthProductQuantities,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod,
        bool detailMode)
    {
        var identity = keyItems
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .First();
        var productQtys = lastMonthProductQuantities
            .Where(x => !string.IsNullOrWhiteSpace(x.ProductCode))
            .GroupBy(x => x.ProductCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(x => x.Quantity).First())
            .OrderBy(x => x.ProductCode, StringComparer.Ordinal)
            .ToList();
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var currencyCode = string.Empty;
        foreach (var period in periodOrder)
        {
            var monthCost = ResolveSingleLineMaterialCostForPeriod(keyItems, period);
            if (monthCost == null)
            {
                continue;
            }
            periodCosts[period] = monthCost.Value;
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                var picked = keyItems
                    .Where(r => ToPeriodKey(r.CostingDate) == period)
                    .OrderByDescending(r => r.CostingDate)
                    .ThenByDescending(r => r.Id)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(picked?.MovingPriceCurrencyCode))
                {
                    currencyCode = picked.MovingPriceCurrencyCode.Trim();
                }
            }
        }

        var row = new TaktBomModelCostTrendDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ModelName = modelName,
            ComponentCode = identity.ComponentCode?.Trim() ?? string.Empty,
            ComponentDescription = identity.ComponentDescription?.Trim() ?? string.Empty,
            ComponentQuantity = detailMode ? identity.ComponentQuantity : null,
            BatchIndicator = detailMode ? identity.BatchIndicator?.Trim() : null,
            ProductionRelated = identity.ProductionRelated?.Trim(),
            PurchaseType = identity.PurchaseType?.Trim() ?? string.Empty,
            SpecialProcurementType = detailMode ? identity.SpecialProcurementType?.Trim() : null,
            ProfitCenterCode = detailMode ? identity.ProfitCenterCode?.Trim() : null,
            ProductCodes = FormatProductGroupWithQuantities(productQtys),
            ProductCount = productQtys.Count,
            CurrencyCode = currencyCode,
            PeriodMaterialCosts = periodCosts,
            PeriodChangeTypes = BuildPeriodChangeTypes(periodOrder, periodCosts),
        };
        ApplyUnitPriceFocusTrend(
            row.PeriodMaterialCosts,
            focusPeriod,
            out var trend,
            out var basePeriod,
            out var comparePeriod,
            out var varianceAmount,
            out var variancePercent);
        // detail：关注月有、基准月无 → 新增；关注月无、基准月有 → 剔除
        if (detailMode
            && !string.IsNullOrWhiteSpace(comparePeriod)
            && !string.IsNullOrWhiteSpace(basePeriod))
        {
            var hasCompare = periodCosts.ContainsKey(comparePeriod);
            var hasBase = periodCosts.ContainsKey(basePeriod);
            if (hasCompare && !hasBase)
            {
                trend = "new";
                varianceAmount = periodCosts[comparePeriod];
                variancePercent = null;
            }
            else if (!hasCompare && hasBase)
            {
                trend = "removed";
                varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(-periodCosts[basePeriod]);
                variancePercent = null;
            }
        }
        row.Trend = trend;
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.VarianceAmount = varianceAmount;
        row.VariancePercent = variancePercent;
        return row;
    }

    /// <summary>
    /// 产品组展示：产品编码:组件数量，英文逗号分隔（如 8Y00000154:1,09VRS7TS04:1）
    /// </summary>
    /// <param name="productQuantities">产品与数量</param>
    /// <returns>产品组文本</returns>
    private static string FormatProductGroupWithQuantities(
        IReadOnlyList<(string ProductCode, decimal Quantity)> productQuantities)
    {
        if (productQuantities == null || productQuantities.Count == 0)
        {
            return string.Empty;
        }
        return string.Join(",", productQuantities.Select(x =>
            $"{x.ProductCode}:{FormatComponentQuantityDisplay(x.Quantity)}"));
    }

    /// <summary>
    /// 组件数量展示（整数不带小数；否则最多 5 位小数去尾零）
    /// </summary>
    /// <param name="quantity">数量</param>
    /// <returns>展示文本</returns>
    private static string FormatComponentQuantityDisplay(decimal quantity)
    {
        if (quantity == decimal.Truncate(quantity))
        {
            return decimal.Truncate(quantity).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        return quantity.ToString("0.#####", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 某月同合并键取组件单价（核算日最新一行：MovingAveragePrice÷MovingPriceUnit；
    /// 不用组件数量——合并键跨产品不含数量，避免不同 BOM 用量把同价放大成「假涨价」）
    /// </summary>
    /// <param name="keyItems">同合并键明细</param>
    /// <param name="periodKey">yyyy-MM</param>
    /// <returns>单位单价；无数据返回 null</returns>
    private static decimal? ResolveSingleLineMaterialCostForPeriod(
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
        // 跨产品合并行展示「单价」而非行成本（数量×单价），与 ApplyUnitPriceFocusTrend 口径一致
        return TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(picked);
    }

    /// <summary>
    /// 按展示期间顺序生成各月存在/价格变动码（先有无物料，再对比价格）
    /// </summary>
    /// <param name="periodOrder">展示期间列 yyyy-MM</param>
    /// <param name="periodCosts">有数据的月材料成本</param>
    /// <returns>期间 → present / absent / new / removed / up / down / flat</returns>
    private static Dictionary<string, string> BuildPeriodChangeTypes(
        IReadOnlyList<string> periodOrder,
        IReadOnlyDictionary<string, decimal> periodCosts)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = 0; i < periodOrder.Count; i++)
        {
            var period = periodOrder[i];
            var hasCurrent = periodCosts.ContainsKey(period);
            var hasPrevious = i > 0 && periodCosts.ContainsKey(periodOrder[i - 1]);
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
            var currentCost = periodCosts[period];
            var previousCost = periodCosts[periodOrder[i - 1]];
            if (currentCost > previousCost)
            {
                result[period] = "up";
            }
            else if (currentCost < previousCost)
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
    /// 差异组件：跨月存在有无变化（新增/剔除，或有月有成本有月无）
    /// </summary>
    /// <param name="row">分析行</param>
    /// <returns>是否纳入差异清单</returns>
    private static bool HasModelComponentPresenceVariance(TaktBomModelCostTrendDto row)
    {
        var types = row.PeriodChangeTypes?.Values.ToList() ?? new List<string>();
        if (types.Count == 0)
        {
            return false;
        }
        if (types.Any(t => t is "new" or "removed"))
        {
            return true;
        }
        var hasPresent = types.Any(t => t is "present" or "up" or "down" or "flat" or "new");
        var hasAbsent = types.Any(t => t is "absent" or "removed");
        return hasPresent && hasAbsent;
    }

    /// <summary>
    /// 按涨跌筛选过滤机种合并分析行
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>过滤后列表</returns>
    private static List<TaktBomModelCostTrendDto> FilterModelCostTrendRows(
        IReadOnlyList<TaktBomModelCostTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down" or "new" or "removed").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 产品分析行全量合计
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumModelCostTrendRowGrandTotals(
        IReadOnlyList<TaktBomModelCostTrendDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        return SumPeriodAndVarianceGrandTotals(
            periodOrder,
            rows.Select(r => (PeriodMap: (IReadOnlyDictionary<string, decimal>)r.PeriodMaterialCosts, r.VarianceAmount)));
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

    /// <summary>
    /// 机种合并分析行全量排序（分页前）：productCountDesc / productCountAsc / trend
    /// </summary>
    /// <param name="rows">行</param>
    /// <param name="sortBy">排序码</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomModelCostTrendDto> OrderModelCostTrendRows(
        IReadOnlyList<TaktBomModelCostTrendDto> rows,
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
        IOrderedEnumerable<TaktBomModelCostTrendDto> ordered = mode switch
        {
            "productcountasc" => rows.OrderBy(r => r.ProductCount),
            "trend" => rows
                .OrderBy(r => TrendRank(r.Trend))
                .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m)),
            "variancedesc" => rows
                .OrderByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
                .ThenBy(r => TrendRank(r.Trend)),
            _ => rows.OrderByDescending(r => r.ProductCount), // productCountDesc 默认
        };
        return ordered
            .ThenBy(r => TrendRank(r.Trend))
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
            .ThenBy(r => r.ComponentCode, StringComparer.Ordinal)
            .ThenBy(r => r.ProductionRelated, StringComparer.Ordinal)
            .ThenBy(r => r.PurchaseType, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 按关注期间对材料成本字典应用环比涨跌
    /// </summary>
    /// <param name="periodUnitPrices">各月材料成本</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="trend">涨跌</param>
    /// <param name="basePeriod">基准月</param>
    /// <param name="comparePeriod">对比月</param>
    /// <param name="varianceAmount">差额</param>
    /// <param name="variancePercent">变动率（百分点）</param>
    private static void ApplyUnitPriceFocusTrend(
        IReadOnlyDictionary<string, decimal> periodUnitPrices,
        string? focusPeriod,
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
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return;
        }
        basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        if (!periodUnitPrices.TryGetValue(basePeriod, out var basePrice)
            || !periodUnitPrices.TryGetValue(comparePeriod, out var comparePrice))
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
 /// 解析 BOM 成本明细物理表：年分表存在则用之，否则 null（回退实体基表，兼容 同步）
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
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
        var seenIds = new HashSet<long>();
        bool TryAppend(IEnumerable<TaktBomMaterialCostItem> rows)
        {
            foreach (var row in rows)
            {
                if (!seenIds.Add(row.Id))
                {
                    continue;
                }
                result.Add(row);
                if (maxRows.HasValue && result.Count >= maxRows.Value)
                {
                    return false;
                }
            }
            return true;
        }
        foreach (var year in years)
        {
            if (maxRows.HasValue && result.Count >= maxRows.Value)
            {
                break;
            }
            var table = await ResolveBomItemPhysicalTableAsync(year);
            if (table == null)
            {
                continue;
            }
            if (maxRows.HasValue)
            {
                var remaining = maxRows.Value - result.Count;
                var part = await _bomMaterialCostItemRepository.GetListForExportAsync(predicate, remaining, table);
                if (!TryAppend(part))
                {
                    break;
                }
            }
            else
            {
                var part = await _bomMaterialCostItemRepository.GetListAsync(predicate, table);
                TryAppend(part);
            }
        }
 // 年分表与基表合并： 同步常写基表，年分表可能仅部分数据；按 Id 去重
        if (!maxRows.HasValue || result.Count < maxRows.Value)
        {
            List<TaktBomMaterialCostItem> basePart;
            if (maxRows.HasValue)
            {
                basePart = await _bomMaterialCostItemRepository.GetListForExportAsync(
                    predicate, maxRows.Value - result.Count);
            }
            else
            {
                basePart = await _bomMaterialCostItemRepository.GetListAsync(predicate);
            }
            var yearSet = years.ToHashSet();
            TryAppend(basePart.Where(r => yearSet.Contains(r.CostingDate.Year)));
        }
        return result;
    }
    /// <summary>
    /// 核算日 → yyyy-MM
    /// </summary>
    /// <param name="costingDate">核算日</param>
    /// <returns>期间键</returns>
    private static string ToPeriodKey(DateTime costingDate)
        => new DateTime(costingDate.Year, costingDate.Month, 1).ToString("yyyy-MM");

    /// <summary>
    /// 某月内按 BOM 行键去重后汇总材料成本（同键取核算日最新一行）
    /// </summary>
    /// <param name="items">明细（产品或合并键子集）</param>
    /// <param name="periodKey">yyyy-MM</param>
    /// <returns>有数据时返回汇总成本，否则 null</returns>
    private static decimal? SumMaterialCostByLineKeyForPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> items,
        string periodKey)
    {
        var periodRows = items.Where(r => ToPeriodKey(r.CostingDate) == periodKey).ToList();
        if (periodRows.Count == 0)
        {
            return null;
        }
        var picked = periodRows
            .GroupBy(BuildBomLineTrendKey, StringComparer.Ordinal)
            .Select(g => g
                .OrderByDescending(r => r.CostingDate)
                .ThenByDescending(r => r.Id)
                .First())
            .ToList();
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(
            picked.Sum(TaktBomMaterialCostItemLineCostHelper.CalculateLineCost));
    }

    /// <summary>
    /// BOM 明细行键（对齐表唯一键，不含 CostingDate）
    /// </summary>
    /// <param name="item">明细行</param>
    /// <returns>稳定键</returns>
    private static string BuildBomLineTrendKey(TaktBomMaterialCostItem item)
    {
        return TaktBomMaterialCostItemLineCostHelper.BuildComponentKey(item);
    }


}
