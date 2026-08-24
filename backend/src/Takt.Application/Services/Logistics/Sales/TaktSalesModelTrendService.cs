// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售机种销售推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售机种销售推移分析服务（月推移 + BOM 机种/产品组；与 CRUD 服务分离）
/// </summary>
public class TaktSalesModelTrendService : TaktServiceBase, ITaktSalesModelTrendService
{
    /// <summary>物料 BOM 按编码分批查询，避免超长 IN 列表</summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktSalesPrice> _salesPriceRepository;
    private readonly ITaktSalesPriceTrendMonthlyAnalysisBuilder _monthlyAnalysisBuilder;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceRepository">销售价格仓储</param>
    /// <param name="monthlyAnalysisBuilder">月推移分析构建器</param>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesModelTrendService(
        ITaktCompanyRepository<TaktSalesPrice> salesPriceRepository,
        ITaktSalesPriceTrendMonthlyAnalysisBuilder monthlyAnalysisBuilder,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceRepository = salesPriceRepository;
        _monthlyAnalysisBuilder = monthlyAnalysisBuilder;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
    }

    /// <summary>
    /// 推移查询栏：销售价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesModelTrendPlantOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceRepository.GetListAsync(
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

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesModelTrendPriceTypeOptionsAsync(string plantCode)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _salesPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType != null
                && x.PriceType != string.Empty);
        return list
            .GroupBy(e => e.PriceType.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重客户（级联第 3 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesModelTrendCustomerOptionsAsync(
        string plantCode,
        string? priceType = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _salesPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType == type
                && x.CustomerCode != null
                && x.CustomerCode != string.Empty);
        return list
            .GroupBy(e => e.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型+客户去重物料（级联第 4 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="customerCode">客户编码</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesModelTrendMaterialOptionsAsync(
        string plantCode,
        string? priceType = null,
        string? customerCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        var customer = customerCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(customer))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _salesPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType == type
                && x.CustomerCode == customer
                && x.MaterialCode != null
                && x.MaterialCode != string.Empty);
        return list
            .GroupBy(e => e.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var description = g.Select(x => x.MaterialDescription)
                    .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 销售机种销售推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktSalesModelTrendResultDto> GetSalesModelTrendAnalysisAsync(
        TaktSalesModelTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var monthly = await _monthlyAnalysisBuilder.BuildAsync(ToMonthlyQuery(queryDto));
        if (monthly.OrderedRows.Count == 0)
        {
            return new TaktSalesModelTrendResultDto
            {
                Paged = TaktPagedResult<TaktSalesModelTrendDto>.Create(
                    new List<TaktSalesModelTrendDto>(), 0, pageIndex, pageSize),
                PeriodOrder = monthly.PeriodOrder,
                MaterialCount = 0,
                BasePeriod = monthly.BasePeriod,
                ComparePeriod = monthly.ComparePeriod,
            };
        }
        var pageMonthly = monthly.OrderedRows.Skip(skip).Take(pageSize).ToList();
        var usage = await LoadBomMaterialUsageLookupAsync(
            queryDto.PlantCode.Trim(),
            pageMonthly.Select(r => r.MaterialCode).ToList());
        var pageRows = EnrichSalesModelTrendRows(pageMonthly, usage);
        return new TaktSalesModelTrendResultDto
        {
            Paged = TaktPagedResult<TaktSalesModelTrendDto>.Create(
                pageRows, monthly.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = monthly.PeriodOrder,
            MaterialCount = monthly.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? monthly.BasePeriod,
            ComparePeriod = monthly.ComparePeriod,
            UpCount = monthly.UpCount,
            DownCount = monthly.DownCount,
            FlatCount = monthly.FlatCount,
            NoneCount = monthly.NoneCount,
        };
    }

    /// <summary>
    /// 导出销售机种销售推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesModelTrendAnalysisAsync(
        TaktSalesModelTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildSalesModelTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "materialCode", "modelGroup", "productGroup", "materialText",
            "customerCode", "customerName",
        };
        var columnLabels = new List<string>
        {
            "物料编码", "机种组", "产品组", "物料文本", "客户编码", "客户名称",
        };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "trend", "varianceAmount", "variancePercent" });
        columnLabels.AddRange(new[] { "涨跌", "差额", "变动率" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["materialCode"] = row.MaterialCode,
                ["modelGroup"] = row.ModelGroup,
                ["productGroup"] = row.ProductGroup,
                ["materialText"] = row.MaterialText,
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
                ["trend"] = row.Trend,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent,
            };
            foreach (var period in built.PeriodOrder)
            {
                dict[$"period_{period}"] = row.PeriodUnitPrices.TryGetValue(period, out var price)
                    ? price
                    : null;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "销售机种销售推移清单",
            fileName ?? $"销售机种销售推移清单_{query.PlantCode}.xlsx");
    }

    /// <summary>
    /// 构建销售机种销售推移全量结果（导出用；BOM 用 DISTINCT 轻量查询）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>构建结果</returns>
    private async Task<SalesModelTrendAnalysisBuilt> BuildSalesModelTrendAnalysisAsync(
        TaktSalesModelTrendQueryDto queryDto)
    {
        var monthly = await _monthlyAnalysisBuilder.BuildAsync(ToMonthlyQuery(queryDto));
        if (monthly.OrderedRows.Count == 0)
        {
            return SalesModelTrendAnalysisBuilt.Empty();
        }
        var plantCode = queryDto.PlantCode.Trim();
        var usage = await LoadBomMaterialUsageLookupAsync(
            plantCode,
            monthly.OrderedRows.Select(r => r.MaterialCode).ToList());
        var enriched = EnrichSalesModelTrendRows(monthly.OrderedRows, usage);
        return new SalesModelTrendAnalysisBuilt
        {
            OrderedRows = enriched,
            PeriodOrder = monthly.PeriodOrder,
            BasePeriod = monthly.BasePeriod,
            ComparePeriod = monthly.ComparePeriod,
            UpCount = monthly.UpCount,
            DownCount = monthly.DownCount,
            FlatCount = monthly.FlatCount,
            NoneCount = monthly.NoneCount,
        };
    }

    /// <summary>
    /// 月推移行附加 BOM 机种/产品组
    /// </summary>
    /// <param name="monthlyRows">月推移行</param>
    /// <param name="usage">物料 BOM 使用关系</param>
    /// <returns>机种销售推移行</returns>
    private static List<TaktSalesModelTrendDto> EnrichSalesModelTrendRows(
        IReadOnlyList<TaktSalesPriceTrendDto> monthlyRows,
        IReadOnlyDictionary<string, BomMaterialUsageInfo> usage)
    {
        return monthlyRows.Select(row =>
        {
            usage.TryGetValue(row.MaterialCode, out var info);
            var productCodes = info?.ProductCodes ?? new List<string>();
            var modelCodes = info?.ModelCodes ?? new List<string>();
            var bomText = info?.ComponentDescription ?? string.Empty;
            var materialText = !string.IsNullOrWhiteSpace(row.MaterialDescription)
                ? row.MaterialDescription
                : bomText;
            return new TaktSalesModelTrendDto
            {
                PlantCode = row.PlantCode,
                MaterialCode = row.MaterialCode,
                MaterialDescription = row.MaterialDescription,
                CustomerCode = row.CustomerCode,
                CustomerName = row.CustomerName,
                CurrencyCode = row.CurrencyCode,
                Unit = row.Unit,
                PeriodUnitPrices = row.PeriodUnitPrices,
                PeriodPriceSourcePeriods = row.PeriodPriceSourcePeriods,
                Trend = row.Trend,
                BasePeriod = row.BasePeriod,
                ComparePeriod = row.ComparePeriod,
                VarianceAmount = row.VarianceAmount,
                VariancePercent = row.VariancePercent,
                ProductCodes = productCodes,
                ModelCodes = modelCodes,
                ProductGroup = string.Join(", ", productCodes),
                ModelGroup = string.Join(", ", modelCodes),
                MaterialText = materialText,
            };
        }).ToList();
    }

    /// <summary>
    /// 机种销售推移查询映射为月推移查询
    /// </summary>
    /// <param name="queryDto">机种销售推移查询</param>
    /// <returns>月推移查询</returns>
    private static TaktSalesPriceTrendQueryDto ToMonthlyQuery(TaktSalesModelTrendQueryDto queryDto)
    {
        return new TaktSalesPriceTrendQueryDto
        {
            PlantCode = queryDto.PlantCode,
            PeriodDateStart = queryDto.PeriodDateStart,
            PeriodDateEnd = queryDto.PeriodDateEnd,
            FocusPeriod = queryDto.FocusPeriod,
            MaterialCode = queryDto.MaterialCode,
            CustomerCode = queryDto.CustomerCode,
            PriceType = queryDto.PriceType,
            OnlyEnabled = queryDto.OnlyEnabled,
            TrendFilter = queryDto.TrendFilter,
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
        };
    }

    /// <summary>
    /// 销售机种销售推移内存构建结果
    /// </summary>
    private sealed class SalesModelTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktSalesModelTrendDto> OrderedRows { get; init; } = new();

        /// <summary>期间列顺序</summary>
        public List<string> PeriodOrder { get; init; } = new();

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

        /// <summary>空结果</summary>
        public static SalesModelTrendAnalysisBuilt Empty() => new();
    }

    /// <summary>
    /// BOM 物料使用信息（产品组 / 机种组）
    /// </summary>
    private sealed class BomMaterialUsageInfo
    {
        /// <summary>产品编码列表</summary>
        public List<string> ProductCodes { get; init; } = new();

        /// <summary>机种编码列表</summary>
        public List<string> ModelCodes { get; init; } = new();

        /// <summary>组件描述（物料文本回退）</summary>
        public string ComponentDescription { get; init; } = string.Empty;
    }

    /// <summary>
    /// 按物料加载 BOM 使用关系：Component → Product（明细 DISTINCT）→ Model（汇总 DISTINCT）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码清单</param>
    /// <returns>物料 → 产品/机种</returns>
    private async Task<Dictionary<string, BomMaterialUsageInfo>> LoadBomMaterialUsageLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes)
    {
        var result = new Dictionary<string, BomMaterialUsageInfo>(StringComparer.OrdinalIgnoreCase);
        var codes = materialCodes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (codes.Count == 0)
        {
            return result;
        }
        var materialToProducts = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var allProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (codes.Count <= MaterialNameLookupBatchSize)
        {
            await FillComponentProductPairsByCodesAsync(plantCode, codes, materialToProducts, allProducts);
        }
        else
        {
            await FillComponentProductPairsForPlantAsync(
                plantCode,
                new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase),
                materialToProducts,
                allProducts);
        }
        var productToModels = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var productList = allProducts.ToList();
        await FillProductModelsFromModelDestinationAsync(productList, productToModels);
        var missingProducts = productList
            .Where(p => !productToModels.TryGetValue(p, out var models) || models.Count == 0)
            .ToList();
        if (missingProducts.Count > 0)
        {
            await FillProductModelPairsByCodesAsync(plantCode, missingProducts, productToModels);
        }
        foreach (var material in codes)
        {
            if (!materialToProducts.TryGetValue(material, out var products) || products.Count == 0)
            {
                result[material] = new BomMaterialUsageInfo();
                continue;
            }
            var modelSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var product in products)
            {
                if (!productToModels.TryGetValue(product, out var models))
                {
                    continue;
                }
                foreach (var model in models)
                {
                    modelSet.Add(model);
                }
            }
            result[material] = new BomMaterialUsageInfo
            {
                ProductCodes = products.OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToList(),
                ModelCodes = modelSet.OrderBy(m => m, StringComparer.OrdinalIgnoreCase).ToList(),
            };
        }
        return result;
    }

    /// <summary>
    /// DISTINCT 组件→产品（指定组件编码）
    /// </summary>
    private async Task FillComponentProductPairsByCodesAsync(
        string plantCode,
        IReadOnlyList<string> componentCodes,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        if (componentCodes.Count == 0)
        {
            return;
        }
        var sql = new StringBuilder();
        sql.Append(
            """
            SELECT DISTINCT
              LTRIM(RTRIM(component_code)) AS ComponentCode,
              LTRIM(RTRIM(product_code)) AS ProductCode
            FROM takt_logistics_manufacturing_bom_material_cost_item
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND component_code IN (
            """);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
        };
        for (var i = 0; i < componentCodes.Count; i++)
        {
            var name = $"c{i}";
            if (i > 0)
            {
                sql.Append(',');
            }
            sql.Append('@').Append(name);
            parameters[name] = componentCodes[i];
        }
        sql.Append(')');
        var script = sql.ToString();
        TaktSqlExecutorValidator.Validate(script);
        var rows = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        AddComponentProductPairs(rows, materialToProducts, allProducts);
    }

    /// <summary>
    /// 工厂级 DISTINCT 组件→产品，仅保留目标物料
    /// </summary>
    private async Task FillComponentProductPairsForPlantAsync(
        string plantCode,
        HashSet<string> targetComponents,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        const string script = """
            SELECT DISTINCT
              LTRIM(RTRIM(component_code)) AS ComponentCode,
              LTRIM(RTRIM(product_code)) AS ProductCode
            FROM takt_logistics_manufacturing_bom_material_cost_item
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
        };
        var rows = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        foreach (var row in rows)
        {
            var component = ReadSqlString(row, "ComponentCode");
            if (string.IsNullOrWhiteSpace(component) || !targetComponents.Contains(component))
            {
                continue;
            }
            var product = ReadSqlString(row, "ProductCode");
            if (string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            if (!materialToProducts.TryGetValue(component, out var products))
            {
                products = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                materialToProducts[component] = products;
            }
            products.Add(product);
            allProducts.Add(product);
        }
    }

    /// <summary>
    /// 产品→机种：型号目的地（MaterialCode=产品编码，与 ResolveModelCodeByProductAsync 同口径）
    /// </summary>
    /// <param name="productCodes">产品编码</param>
    /// <param name="productToModels">产品→机种集合</param>
    private async Task FillProductModelsFromModelDestinationAsync(
        IReadOnlyList<string> productCodes,
        Dictionary<string, HashSet<string>> productToModels)
    {
        if (productCodes.Count == 0)
        {
            return;
        }
        var destinations = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialCode != null);
        if (destinations.Count == 0)
        {
            return;
        }
        foreach (var product in productCodes)
        {
            if (string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            foreach (var dest in destinations)
            {
                if (string.IsNullOrWhiteSpace(dest.ModelCode))
                {
                    continue;
                }
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(dest.MaterialCode, product))
                {
                    continue;
                }
                if (!productToModels.TryGetValue(product, out var models))
                {
                    models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    productToModels[product] = models;
                }
                models.Add(dest.ModelCode.Trim());
            }
        }
    }

    /// <summary>
    /// DISTINCT 产品→机种（成本汇总表回退；支持物料码归一匹配）
    /// </summary>
    private async Task FillProductModelPairsByCodesAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        Dictionary<string, HashSet<string>> productToModels)
    {
        if (productCodes.Count == 0)
        {
            return;
        }
        var targetProducts = new HashSet<string>(productCodes, StringComparer.OrdinalIgnoreCase);
        const string script = """
            SELECT DISTINCT
              LTRIM(RTRIM(product_code)) AS ProductCode,
              LTRIM(RTRIM(model_code)) AS ModelCode
            FROM takt_logistics_manufacturing_bom_material_cost
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND LEN(LTRIM(RTRIM(ISNULL(model_code, '')))) > 0
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
        };
        var rows = await _bomMaterialCostRepository.QueryReadOnlySqlAsync(script, parameters);
        foreach (var row in rows)
        {
            var storedProduct = ReadSqlString(row, "ProductCode");
            var model = ReadSqlString(row, "ModelCode");
            if (string.IsNullOrWhiteSpace(storedProduct) || string.IsNullOrWhiteSpace(model))
            {
                continue;
            }
            foreach (var requested in targetProducts)
            {
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(storedProduct, requested))
                {
                    continue;
                }
                if (!productToModels.TryGetValue(requested, out var models))
                {
                    models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    productToModels[requested] = models;
                }
                models.Add(model);
            }
        }
    }

    /// <summary>
    /// 解析 DISTINCT 组件-产品行
    /// </summary>
    private static void AddComponentProductPairs(
        IReadOnlyList<Dictionary<string, object>> rows,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        foreach (var row in rows)
        {
            var component = ReadSqlString(row, "ComponentCode");
            var product = ReadSqlString(row, "ProductCode");
            if (string.IsNullOrWhiteSpace(component) || string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            if (!materialToProducts.TryGetValue(component, out var products))
            {
                products = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                materialToProducts[component] = products;
            }
            products.Add(product);
            allProducts.Add(product);
        }
    }

    /// <summary>
    /// 读取只读 SQL 行字符串列
    /// </summary>
    private static string ReadSqlString(Dictionary<string, object> row, string column)
    {
        if (!row.TryGetValue(column, out var value) || value == null)
        {
            return string.Empty;
        }
        return Convert.ToString(value)?.Trim() ?? string.Empty;
    }
}
