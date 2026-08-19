// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseTrendPriceService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格月推移 / 机种推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格月推移 / 机种推移分析服务（读采购价格本表；与 CRUD 服务分离）
/// </summary>
public class TaktPurchaseTrendPriceService : TaktServiceBase, ITaktPurchaseTrendPriceService
{
    /// <summary>物料/供应商名称按编码分批查询，避免超长 IN 列表</summary>
    private const int MaterialNameLookupBatchSize = 500;

    /// <summary>
    /// 机种价格推移默认：领涨 / 领跌各取前 N 个物料（环比差额排序）
    /// </summary>
    private const int ModelTrendLeadingMaterialCount = 50;

    private readonly ITaktCompanyRepository<TaktPurchasePrice> _purchasePriceRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceItem> _purchasePriceItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceRepository">采购价格仓储</param>
    /// <param name="purchasePriceItemRepository">采购价格明细仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="supplierRepository">供应商仓储</param>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="companyRepository">公司仓储（读取 RelatedPlant）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseTrendPriceService(
        ITaktCompanyRepository<TaktPurchasePrice> purchasePriceRepository,
        ITaktCompanyRepository<TaktPurchasePriceItem> purchasePriceItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceRepository = purchasePriceRepository;
        _purchasePriceItemRepository = purchasePriceItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _supplierRepository = supplierRepository;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _companyRepository = companyRepository;
    }

    /// <summary>
    /// 推移查询栏工厂选项（级联第 1 级）：仅当前公司 RelatedPlant，且须存在于采购价格本表 PlantCode
    /// </summary>
    /// <returns>下拉选项（通常 0～1 项；DictValue=PlantCode）</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendPlantOptionsAsync()
    {
        EnsureThreeLayerContext();
        // 仅当前公司关联工厂：TaktCompany.RelatedPlant ∩ 采购价格本表 PlantCode
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
        var pricePlants = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == relatedPlant);
        if (pricePlants.Count == 0)
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
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendPriceTypeOptionsAsync(string plantCode)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _purchasePriceRepository.GetListAsync(
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
    /// 推移查询栏：按工厂+条件类型去重供应商（级联第 3 级；优先与同厂供应商主数据交叉）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendSupplierOptionsAsync(
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
        // 本表按工厂+条件类型去重；再与同工厂供应商主数据交叉，排除其它工厂供应商编码
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType == type
                && x.SupplierCode != null
                && x.SupplierCode != string.Empty);
        var codes = list
            .GroupBy(e => e.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g.Key)
            .Where(c => !string.IsNullOrEmpty(c))
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        if (codes.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var plantSupplierCodes = await LoadPlantScopedSupplierCodesAsync(plant, codes);
        // 主数据同厂有交集则收紧；无交集（PlantCode 未对齐）回退本表工厂价目供应商
        var scopedCodes = plantSupplierCodes.Count > 0
            ? codes.Where(plantSupplierCodes.Contains).ToList()
            : codes;
        if (scopedCodes.Count == 0)
        {
            scopedCodes = codes;
        }
        var nameLookup = await LoadSupplierNameLookupAsync(plant, scopedCodes);
        return scopedCodes
            .Select(c =>
            {
                nameLookup.TryGetValue(c, out var name);
                var label = string.IsNullOrWhiteSpace(name) ? c : $"{c} - {name}";
                return new TaktSelectOption
                {
                    DictValue = c,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型+供应商去重物料（级联第 4 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="supplierCode">供应商编码</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendMaterialOptionsAsync(
        string plantCode,
        string? priceType = null,
        string? supplierCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        var supplier = supplierCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(supplier))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType == type
                && x.SupplierCode == supplier
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
    /// 采购价格月推移转置分析（工厂×物料×供应商×月份）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktPurchasePriceMonthlyTrendResultDto> GetPurchasePriceMonthlyTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildPurchasePriceMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktPurchasePriceMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktPurchasePriceMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            MaterialCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <summary>
    /// 导出采购价格月推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceMonthlyTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildPurchasePriceMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "materialCode", "materialDescription", "supplierCode", "supplierName", "currencyCode", "unit",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "物料编码", "物料描述", "供应商编码", "供应商名称", "币种", "单位",
        };
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
                ["materialCode"] = row.MaterialCode,
                ["materialDescription"] = row.MaterialDescription,
                ["supplierCode"] = row.SupplierCode,
                ["supplierName"] = row.SupplierName,
                ["currencyCode"] = row.CurrencyCode,
                ["unit"] = row.Unit,
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
                if (!row.PeriodUnitPrices.TryGetValue(period, out var price))
                {
                    dict[$"period_{period}"] = null;
                    continue;
                }
                var isCarried = row.PeriodPriceSourcePeriods.TryGetValue(period, out var source)
                    && !string.IsNullOrWhiteSpace(source)
                    && !string.Equals(source, period, StringComparison.Ordinal);
                dict[$"period_{period}"] = isCarried
                    ? $"{price.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture)}*"
                    : price;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "采购价格推移清单",
            fileName ?? $"采购价格推移清单_{query.PlantCode}.xlsx");
    }

    /// <summary>
    /// 采购机种价格推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktPurchasePriceModelTrendResultDto> GetPurchasePriceModelTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var monthly = await BuildPurchasePriceMonthlyTrendAnalysisAsync(queryDto);
        var orderedRows = ApplyModelTrendLeadingDefault(monthly.OrderedRows, queryDto.TrendFilter);
        if (orderedRows.Count == 0)
        {
            return new TaktPurchasePriceModelTrendResultDto
            {
                Paged = TaktPagedResult<TaktPurchasePriceModelTrendDto>.Create(
                    new List<TaktPurchasePriceModelTrendDto>(), 0, pageIndex, pageSize),
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
        var pageRows = EnrichPurchasePriceModelTrendRows(pageMonthly, usage);
        return new TaktPurchasePriceModelTrendResultDto
        {
            Paged = TaktPagedResult<TaktPurchasePriceModelTrendDto>.Create(
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
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceModelTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildPurchasePriceModelTrendAnalysisAsync(query);
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
    private async Task<PurchasePriceModelTrendAnalysisBuilt> BuildPurchasePriceModelTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto queryDto)
    {
        var monthly = await BuildPurchasePriceMonthlyTrendAnalysisAsync(queryDto);
        var orderedRows = ApplyModelTrendLeadingDefault(monthly.OrderedRows, queryDto.TrendFilter);
        if (orderedRows.Count == 0)
        {
            return PurchasePriceModelTrendAnalysisBuilt.Empty();
        }
        var plantCode = queryDto.PlantCode.Trim();
        var materialType = RequireModelTrendMaterialType(queryDto.MaterialType);
        var usage = await LoadBomMaterialUsageLookupAsync(
            plantCode,
            orderedRows.Select(r => r.MaterialCode).ToList(),
            materialType);
        var enriched = EnrichPurchasePriceModelTrendRows(orderedRows, usage);
        return new PurchasePriceModelTrendAnalysisBuilt
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
    private static List<TaktPurchasePriceModelTrendDto> EnrichPurchasePriceModelTrendRows(
        IReadOnlyList<TaktPurchasePriceMonthlyTrendDto> monthlyRows,
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
            return new TaktPurchasePriceModelTrendDto
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

    /// <summary>
    /// 构建采购价格月推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<PurchasePriceMonthlyTrendAnalysisBuilt> BuildPurchasePriceMonthlyTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto queryDto)
    {
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
            return PurchasePriceMonthlyTrendAnalysisBuilt.Empty();
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masters.Select(m => m.Id).ToList();
        var itemExp = BuildPurchasePriceTrendItemExpression(masterIds);
        var items = await _purchasePriceItemRepository.GetListAsync(itemExp);
        if (items.Count == 0)
        {
            return PurchasePriceMonthlyTrendAnalysisBuilt.Empty();
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
            .Select(g => BuildPurchasePriceMonthlyTrendRow(
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
        return new PurchasePriceMonthlyTrendAnalysisBuilt
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
        TaktPurchasePriceMonthlyTrendQueryDto queryDto)
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
    private static TaktPurchasePriceMonthlyTrendDto BuildPurchasePriceMonthlyTrendRow(
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
        var row = new TaktPurchasePriceMonthlyTrendDto
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
        List<TaktPurchasePriceMonthlyTrendDto> rows)
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
    private static void ApplyPurchasePriceFocusTrend(TaktPurchasePriceMonthlyTrendDto row, string? focusPeriod)
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
    private static List<TaktPurchasePriceMonthlyTrendDto> FilterPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceMonthlyTrendDto> rows,
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
    /// 机种价格推移：空或 leading 时默认取领涨/领跌各前 N 条（按环比差额）
    /// </summary>
    /// <param name="orderedRows">已排序全量行</param>
    /// <param name="trendFilter">涨跌筛选码</param>
    /// <returns>应用默认领涨领跌后的行</returns>
    private static List<TaktPurchasePriceMonthlyTrendDto> ApplyModelTrendLeadingDefault(
        IReadOnlyList<TaktPurchasePriceMonthlyTrendDto> orderedRows,
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
    private static List<TaktPurchasePriceMonthlyTrendDto> TakeLeadingPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceMonthlyTrendDto> rows,
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
    /// 涨跌优先排序
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <returns>排序后行</returns>
    private static List<TaktPurchasePriceMonthlyTrendDto> OrderPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceMonthlyTrendDto> rows)
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
        /// <summary>单例</summary>
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
        /// <summary>采购价格主表</summary>
        public TaktPurchasePrice Master { get; init; } = null!;

        /// <summary>采购价格明细</summary>
        public TaktPurchasePriceItem Item { get; init; } = null!;
    }

    /// <summary>
    /// 采购价格月推移内存构建结果
    /// </summary>
    private sealed class PurchasePriceMonthlyTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktPurchasePriceMonthlyTrendDto> OrderedRows { get; init; } = new();

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
        public static PurchasePriceMonthlyTrendAnalysisBuilt Empty() => new();
    }

    /// <summary>
    /// 采购机种推移内存构建结果
    /// </summary>
    private sealed class PurchasePriceModelTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktPurchasePriceModelTrendDto> OrderedRows { get; init; } = new();

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
        public static PurchasePriceModelTrendAnalysisBuilt Empty() => new();
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
