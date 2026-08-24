// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购机种价格推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购机种价格推移分析服务（月推移 + BOM 机种/产品组；与 CRUD 服务分离）
/// </summary>
public class TaktPurchaseModelTrendService : TaktServiceBase, ITaktPurchaseModelTrendService
{
    /// <summary>物料 BOM 按编码分批查询，避免超长 IN 列表</summary>
    private const int MaterialNameLookupBatchSize = 500;

    /// <summary>
    /// 机种价格推移默认：领涨 / 领跌各取前 N 个物料（环比差额排序）
    /// </summary>
    private const int ModelTrendLeadingMaterialCount = 50;

    private readonly ITaktCompanyRepository<TaktPurchasePrice> _purchasePriceRepository;
    private readonly ITaktPurchasePriceTrendMonthlyAnalysisBuilder _monthlyAnalysisBuilder;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    public TaktPurchaseModelTrendService(
        ITaktCompanyRepository<TaktPurchasePrice> purchasePriceRepository,
        ITaktPurchasePriceTrendMonthlyAnalysisBuilder monthlyAnalysisBuilder,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceRepository = purchasePriceRepository;
        _monthlyAnalysisBuilder = monthlyAnalysisBuilder;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
    }

    public async Task<List<TaktSelectOption>> GetPurchaseModelTrendPlantOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode != null
                && x.PlantCode != string.Empty);
        return list
            .GroupBy(e => e.PlantCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption { DictValue = g.Key, DictLabel = g.Key })
            .ToList();
    }

    public async Task<List<TaktSelectOption>> GetPurchaseModelTrendPriceTypeOptionsAsync(string plantCode)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant)) return new List<TaktSelectOption>();
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant && x.PriceType != null && x.PriceType != string.Empty);
        return list.GroupBy(e => e.PriceType.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption { DictValue = g.Key, DictLabel = g.Key }).ToList();
    }

    public async Task<List<TaktSelectOption>> GetPurchaseModelTrendSupplierOptionsAsync(string plantCode, string? priceType = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type)) return new List<TaktSelectOption>();
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant && x.PriceType == type
                && x.SupplierCode != null && x.SupplierCode != string.Empty);
        return list.GroupBy(e => e.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption { DictValue = g.Key, DictLabel = g.Key }).ToList();
    }

    public async Task<List<TaktSelectOption>> GetPurchaseModelTrendMaterialOptionsAsync(
        string plantCode, string? priceType = null, string? supplierCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        var supplier = supplierCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(supplier))
            return new List<TaktSelectOption>();
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant && x.PriceType == type && x.SupplierCode == supplier
                && x.MaterialCode != null && x.MaterialCode != string.Empty);
        return list.GroupBy(e => e.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var description = g.Select(x => x.MaterialDescription).FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : g.Key + " - " + description;
                return new TaktSelectOption { DictValue = g.Key, DictLabel = label };
            }).ToList();
    }

    /// <summary>
    /// 采购机种价格推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktPurchaseModelTrendResultDto> GetPurchaseModelTrendAnalysisAsync(
        TaktPurchaseModelTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var monthly = await _monthlyAnalysisBuilder.BuildAsync(ToPriceTrendQuery(queryDto));
        var orderedRows = ApplyModelTrendLeadingDefault(monthly.OrderedRows, queryDto.TrendFilter);
        if (orderedRows.Count == 0)
        {
            return new TaktPurchaseModelTrendResultDto
            {
                Paged = TaktPagedResult<TaktPurchaseModelTrendDto>.Create(
                    new List<TaktPurchaseModelTrendDto>(), 0, pageIndex, pageSize),
                PeriodOrder = monthly.PeriodOrder,
                MaterialCount = 0,
                BasePeriod = monthly.BasePeriod,
                ComparePeriod = monthly.ComparePeriod,
            };
        }
        var materialType = RequireModelTrendMaterialType(queryDto.MaterialType);
        var pageMonthly = orderedRows.Skip(skip).Take(pageSize).ToList();
        var usage = await LoadBomMaterialUsageLookupAsync(
            queryDto.PlantCode.Trim(),
            pageMonthly.Select(r => r.MaterialCode).ToList(),
            materialType);
        var pageRows = EnrichPurchaseModelTrendRows(pageMonthly, usage);
        return new TaktPurchaseModelTrendResultDto
        {
            Paged = TaktPagedResult<TaktPurchaseModelTrendDto>.Create(
                pageRows, orderedRows.Count, pageIndex, pageSize),
            PeriodOrder = monthly.PeriodOrder,
            MaterialCount = orderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? monthly.BasePeriod,
            ComparePeriod = monthly.ComparePeriod,
            UpCount = monthly.UpCount,
            DownCount = monthly.DownCount,
            FlatCount = monthly.FlatCount,
            NoneCount = monthly.NoneCount,
        };
    }

    /// <summary>
    /// 导出采购机种价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseModelTrendAnalysisAsync(
        TaktPurchaseModelTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildPurchaseModelTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "materialCode", "modelGroup", "productGroup", "materialText",
            "supplierCode", "supplierName",
        };
        var columnLabels = new List<string>
        {
            "物料编码", "机种组", "产品组", "物料文本", "供应商编码", "供应商名称",
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
                ["supplierCode"] = row.SupplierCode,
                ["supplierName"] = row.SupplierName,
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
            sheetName ?? "采购机种价格推移清单",
            fileName ?? $"采购机种价格推移清单_{query.PlantCode}.xlsx");
    }

    /// <summary>
    /// 构建采购机种价格推移全量结果（导出用）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>构建结果</returns>
    private async Task<PurchaseModelTrendAnalysisBuilt> BuildPurchaseModelTrendAnalysisAsync(
        TaktPurchaseModelTrendQueryDto queryDto)
    {
        var monthly = await _monthlyAnalysisBuilder.BuildAsync(ToPriceTrendQuery(queryDto));
        var orderedRows = ApplyModelTrendLeadingDefault(monthly.OrderedRows, queryDto.TrendFilter);
        if (orderedRows.Count == 0)
        {
            return PurchaseModelTrendAnalysisBuilt.Empty();
        }
        var plantCode = queryDto.PlantCode.Trim();
        var materialType = RequireModelTrendMaterialType(queryDto.MaterialType);
        var usage = await LoadBomMaterialUsageLookupAsync(
            plantCode,
            orderedRows.Select(r => r.MaterialCode).ToList(),
            materialType);
        var enriched = EnrichPurchaseModelTrendRows(orderedRows, usage);
        return new PurchaseModelTrendAnalysisBuilt
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
    /// <returns>机种推移行</returns>
    private static List<TaktPurchaseModelTrendDto> EnrichPurchaseModelTrendRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> monthlyRows,
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
            return new TaktPurchaseModelTrendDto
            {
                PlantCode = row.PlantCode,
                MaterialCode = row.MaterialCode,
                MaterialDescription = row.MaterialDescription,
                SupplierCode = row.SupplierCode,
                SupplierName = row.SupplierName,
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
    private static List<TaktPurchasePriceTrendDto> ApplyModelTrendLeadingDefault(
        IReadOnlyList<TaktPurchasePriceTrendDto> orderedRows,
        string? trendFilter)
    {
        if (!ShouldApplyModelTrendLeadingDefault(trendFilter))
        {
            return orderedRows.ToList();
        }
        return TakeLeadingPurchasePriceTrendRows(orderedRows, ModelTrendLeadingMaterialCount);
    }

    /// <summary>
    /// 是否应用机种推移默认领涨/领跌截取
    /// </summary>
    /// <param name="trendFilter">涨跌筛选码</param>
    /// <returns>true=截取领涨领跌各 N</returns>
    private static bool ShouldApplyModelTrendLeadingDefault(string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return true;
        }
        return string.Equals(trendFilter.Trim(), "leading", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 取领涨 / 领跌各前 N 条（涨：差额降序；跌：差额升序）
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <param name="takeEach">涨、跌各自条数上限</param>
    /// <returns>领涨后接领跌</returns>
    private static List<TaktPurchasePriceTrendDto> TakeLeadingPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows,
        int takeEach)
    {
        var limit = Math.Max(0, takeEach);
        var up = rows
            .Where(r => r.Trend == "up")
            .OrderByDescending(r => r.VarianceAmount ?? 0m)
            .ThenBy(r => r.MaterialCode, StringComparer.Ordinal)
            .ThenBy(r => r.SupplierCode, StringComparer.Ordinal)
            .Take(limit);
        var down = rows
            .Where(r => r.Trend == "down")
            .OrderBy(r => r.VarianceAmount ?? 0m)
            .ThenBy(r => r.MaterialCode, StringComparer.Ordinal)
            .ThenBy(r => r.SupplierCode, StringComparer.Ordinal)
            .Take(limit);
        return up.Concat(down).ToList();
    }
    /// <summary>
    /// 按物料加载 BOM 使用关系：Component → Product → Model
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码清单</param>
    /// <param name="materialType">产品物料类型（空则默认 FERT）</param>
    /// <returns>物料 → 产品/机种</returns>
    private async Task<Dictionary<string, BomMaterialUsageInfo>> LoadBomMaterialUsageLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes,
        string? materialType = null)
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
        var resolvedType = ResolveBomUsageMaterialType(materialType);
        // 机种/产品组按查询物料类型过滤（空则 FERT）
        await FilterBomUsageProductsByMaterialTypeAsync(
            plantCode,
            resolvedType,
            materialToProducts,
            allProducts);
        var productToModels = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var productList = allProducts.ToList();
        await FillProductModelsFromModelDestinationAsync(productList, productToModels);
        var missingProducts = productList
            .Where(p => !productToModels.TryGetValue(p, out var models) || models.Count == 0)
            .ToList();
        if (missingProducts.Count > 0)
        {
            await FillProductModelPairsByCodesAsync(plantCode, missingProducts, productToModels, resolvedType);
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
    /// 产品→机种：型号目的地
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
    /// DISTINCT 产品→机种（成本汇总表回退）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="productToModels">产品→机种</param>
    /// <param name="materialType">产品物料类型</param>
    private async Task FillProductModelPairsByCodesAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        Dictionary<string, HashSet<string>> productToModels,
        string materialType)
    {
        if (productCodes.Count == 0)
        {
            return;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(materialType);
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
              AND UPPER(LTRIM(RTRIM(ISNULL(material_type, '')))) = @materialType
              AND LEN(LTRIM(RTRIM(ISNULL(model_code, '')))) > 0
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["materialType"] = materialType.Trim().ToUpperInvariant(),
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
    /// 机种推移必填产品物料类型
    /// </summary>
    /// <param name="materialType">查询物料类型</param>
    /// <returns>规范化类型码</returns>
    private string RequireModelTrendMaterialType(string? materialType)
    {
        var type = materialType?.Trim();
        if (string.IsNullOrWhiteSpace(type))
        {
            ThrowBusinessExceptionLocalized("validation.required", "MaterialType");
        }
        return type!;
    }

    /// <summary>
    /// BOM 产品组过滤用物料类型（空则默认 FERT）
    /// </summary>
    /// <param name="materialType">查询类型</param>
    /// <returns>类型码</returns>
    private static string ResolveBomUsageMaterialType(string? materialType)
    {
        var type = materialType?.Trim();
        return string.IsNullOrWhiteSpace(type)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : type;
    }

    /// <summary>
    /// 仅保留 BOM 成本汇总中指定 MaterialType 的产品（机种推移口径）
    /// </summary>
    private async Task FilterBomUsageProductsByMaterialTypeAsync(
        string plantCode,
        string materialType,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        if (allProducts.Count == 0)
        {
            return;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(materialType);
        const string script = """
            SELECT DISTINCT
              LTRIM(RTRIM(product_code)) AS ProductCode
            FROM takt_logistics_manufacturing_bom_material_cost
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND UPPER(LTRIM(RTRIM(ISNULL(material_type, '')))) = @materialType
              AND LEN(LTRIM(RTRIM(ISNULL(product_code, '')))) > 0
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["materialType"] = materialType.Trim().ToUpperInvariant(),
        };
        var rows = await _bomMaterialCostRepository.QueryReadOnlySqlAsync(script, parameters);
        var typedProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var product = ReadSqlString(row, "ProductCode");
            if (!string.IsNullOrWhiteSpace(product))
            {
                typedProducts.Add(product);
            }
        }
        var excluded = allProducts
            .Where(p => !typedProducts.Any(f => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(f, p)))
            .ToList();
        foreach (var product in excluded)
        {
            allProducts.Remove(product);
        }
        foreach (var pair in materialToProducts)
        {
            pair.Value.RemoveWhere(p =>
                !typedProducts.Any(f => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(f, p)));
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

    private static TaktPurchasePriceTrendQueryDto ToPriceTrendQuery(TaktPurchaseModelTrendQueryDto queryDto)
    {
        return new TaktPurchasePriceTrendQueryDto
        {
            PlantCode = queryDto.PlantCode,
            PeriodDateStart = queryDto.PeriodDateStart,
            PeriodDateEnd = queryDto.PeriodDateEnd,
            FocusPeriod = queryDto.FocusPeriod,
            MaterialCode = queryDto.MaterialCode,
            SupplierCode = queryDto.SupplierCode,
            PriceType = queryDto.PriceType,
            OnlyEnabled = queryDto.OnlyEnabled,
            TrendFilter = queryDto.TrendFilter,
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
        };
    }

    private sealed class PurchaseModelTrendAnalysisBuilt
    {
        public List<TaktPurchaseModelTrendDto> OrderedRows { get; init; } = new();
        public List<string> PeriodOrder { get; init; } = new();
        public string? BasePeriod { get; init; }
        public string? ComparePeriod { get; init; }
        public int UpCount { get; init; }
        public int DownCount { get; init; }
        public int FlatCount { get; init; }
        public int NoneCount { get; init; }
        public static PurchaseModelTrendAnalysisBuilt Empty() => new();
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
}
