// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格月推移 / 机种推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格月推移 / 机种推移分析服务（读销售价格本表；与 CRUD 服务分离）
/// </summary>
public class TaktSalesPriceTrendService : TaktServiceBase, ITaktSalesPriceTrendService
{
    /// <summary>物料/客户名称按编码分批查询，避免超长 IN 列表</summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktSalesPrice> _salesPriceRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceItem> _salesPriceItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktCustomer> _customerRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceRepository">销售价格仓储</param>
    /// <param name="salesPriceItemRepository">销售价格明细仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="customerRepository">客户仓储</param>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceTrendService(
        ITaktCompanyRepository<TaktSalesPrice> salesPriceRepository,
        ITaktCompanyRepository<TaktSalesPriceItem> salesPriceItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktCustomer> customerRepository,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceRepository = salesPriceRepository;
        _salesPriceItemRepository = salesPriceItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _customerRepository = customerRepository;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendPlantOptionsAsync()
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

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendPriceTypeOptionsAsync(string plantCode)
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

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendCustomerOptionsAsync(
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

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendMaterialOptionsAsync(
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

    /// <inheritdoc />
    public async Task<TaktSalesPriceMonthlyTrendResultDto> GetSalesPriceMonthlyTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildSalesPriceMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktSalesPriceMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktSalesPriceMonthlyTrendDto>.Create(
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

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceMonthlyTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildSalesPriceMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "materialCode", "materialDescription", "customerCode", "customerName", "currencyCode", "unit",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "物料编码", "物料描述", "客户编码", "客户名称", "币种", "单位",
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
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
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
            sheetName ?? "销售价格推移清单",
            fileName ?? $"销售价格推移清单_{query.PlantCode}.xlsx");
    }

    /// <inheritdoc />
    public async Task<TaktSalesPriceModelTrendResultDto> GetSalesPriceModelTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var monthly = await BuildSalesPriceMonthlyTrendAnalysisAsync(queryDto);
        if (monthly.OrderedRows.Count == 0)
        {
            return new TaktSalesPriceModelTrendResultDto
            {
                Paged = TaktPagedResult<TaktSalesPriceModelTrendDto>.Create(
                    new List<TaktSalesPriceModelTrendDto>(), 0, pageIndex, pageSize),
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
        var pageRows = EnrichSalesPriceModelTrendRows(pageMonthly, usage);
        return new TaktSalesPriceModelTrendResultDto
        {
            Paged = TaktPagedResult<TaktSalesPriceModelTrendDto>.Create(
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

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceModelTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildSalesPriceModelTrendAnalysisAsync(query);
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
            sheetName ?? "销售机种价格推移清单",
            fileName ?? $"销售机种价格推移清单_{query.PlantCode}.xlsx");
    }

    /// <summary>
    /// 构建销售机种价格推移全量结果（导出用；BOM 用 DISTINCT 轻量查询）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>构建结果</returns>
    private async Task<SalesPriceModelTrendAnalysisBuilt> BuildSalesPriceModelTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto queryDto)
    {
        var monthly = await BuildSalesPriceMonthlyTrendAnalysisAsync(queryDto);
        if (monthly.OrderedRows.Count == 0)
        {
            return SalesPriceModelTrendAnalysisBuilt.Empty();
        }
        var plantCode = queryDto.PlantCode.Trim();
        var usage = await LoadBomMaterialUsageLookupAsync(
            plantCode,
            monthly.OrderedRows.Select(r => r.MaterialCode).ToList());
        var enriched = EnrichSalesPriceModelTrendRows(monthly.OrderedRows, usage);
        return new SalesPriceModelTrendAnalysisBuilt
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
    private static List<TaktSalesPriceModelTrendDto> EnrichSalesPriceModelTrendRows(
        IReadOnlyList<TaktSalesPriceMonthlyTrendDto> monthlyRows,
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
            return new TaktSalesPriceModelTrendDto
            {
                PlantCode = row.PlantCode,
                MaterialCode = row.MaterialCode,
                MaterialDescription = row.MaterialDescription,
                CustomerCode = row.CustomerCode,
                CustomerName = row.CustomerName,
                CurrencyCode = row.CurrencyCode,
                Unit = row.Unit,
                PeriodUnitPrices = row.PeriodUnitPrices,
                PeriodPriceSourcePeriods = row.PeriodPriceSourcePeriods, // 含缺月回填来源月
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
    /// 构建销售价格月推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<SalesPriceMonthlyTrendAnalysisBuilt> BuildSalesPriceMonthlyTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var onlyEnabled = queryDto.OnlyEnabled ?? true;
        var customerFilter = string.IsNullOrWhiteSpace(queryDto.CustomerCode) ? null : queryDto.CustomerCode.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var masterExp = BuildSalesPriceTrendMasterExpression(
            plantCode,
            onlyEnabled,
            customerFilter,
            materialFilter,
            queryDto.PriceType);
        var masters = await _salesPriceRepository.GetListAsync(masterExp);
        if (masters.Count == 0)
        {
            return SalesPriceMonthlyTrendAnalysisBuilt.Empty();
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masters.Select(m => m.Id).ToList();
        var itemExp = BuildSalesPriceTrendItemExpression(masterIds);
        var items = await _salesPriceItemRepository.GetListAsync(itemExp);
        if (items.Count == 0)
        {
            return SalesPriceMonthlyTrendAnalysisBuilt.Empty();
        }
        var sourceRows = items
            .Where(i => masterById.ContainsKey(i.SalesPriceId))
            .Select(i => new SalesPriceTrendSourceRow
            {
                Master = masterById[i.SalesPriceId],
                Item = i,
            })
            .ToList();
        var (rangeStart, rangeEnd, periodOrder) = ResolveSalesPriceTrendRange(queryDto);
        var focusPeriod = ResolveSalesPriceFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var allRows = sourceRows
            .GroupBy(
                r => new SalesPriceTrendRowKey(
                    r.Master.PlantCode.Trim(),
                    r.Master.MaterialCode.Trim(),
                    r.Master.CustomerCode?.Trim() ?? string.Empty),
                SalesPriceTrendRowKeyComparer.Instance)
            .Select(g => BuildSalesPriceMonthlyTrendRow(
                g.Key,
                g.ToList(),
                periodOrder,
                focusPeriod,
                rangeStart,
                rangeEnd))
            .ToList();
        await FillSalesPriceTrendDisplayNamesAsync(plantCode, allRows);
        var filtered = FilterSalesPriceTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderSalesPriceTrendRows(filtered);
        return new SalesPriceMonthlyTrendAnalysisBuilt
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
    /// 构建销售价格推移主表筛选条件
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="onlyEnabled">仅启用主表（兼容保留）</param>
    /// <param name="customerFilter">客户编码</param>
    /// <param name="materialFilter">物料编码包含</param>
    /// <param name="priceType">价格类型</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktSalesPrice, bool>> BuildSalesPriceTrendMasterExpression(
        string plantCode,
        bool onlyEnabled,
        string? customerFilter,
        string? materialFilter,
        string? priceType)
    {
        var exp = Expressionable.Create<TaktSalesPrice>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        // 当前实体无 PriceStatus；OnlyEnabled 保留 API 兼容，暂不按状态过滤
        _ = onlyEnabled;
        if (!string.IsNullOrWhiteSpace(materialFilter))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialFilter));
        }
        if (!string.IsNullOrWhiteSpace(customerFilter))
        {
            exp = exp.And(x => x.CustomerCode == customerFilter);
        }
        if (!string.IsNullOrWhiteSpace(priceType))
        {
            var priceTypeFilter = priceType.Trim();
            exp = exp.And(x => x.PriceType == priceTypeFilter);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 构建销售价格推移明细筛选条件
    /// </summary>
    /// <param name="masterIds">主表 ID 列表</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktSalesPriceItem, bool>> BuildSalesPriceTrendItemExpression(
        IReadOnlyList<long> masterIds)
    {
        var exp = Expressionable.Create<TaktSalesPriceItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && masterIds.Contains(x.SalesPriceId)
            && x.IsObsolete == 0);
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析销售价格推移分析日期区间与期间列
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起止与期间列顺序</returns>
    private static (DateTime RangeStart, DateTime RangeEnd, List<string> PeriodOrder) ResolveSalesPriceTrendRange(
        TaktSalesPriceMonthlyTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizeSalesPricePeriodBounds(
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
    private static (DateTime? Start, DateTime? End) NormalizeSalesPricePeriodBounds(
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
    private static string? ResolveSalesPriceFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 构建单行销售价格月推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键明细</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="rangeStart">分析区间起</param>
    /// <param name="rangeEnd">分析区间止</param>
    /// <returns>转置行</returns>
    private static TaktSalesPriceMonthlyTrendDto BuildSalesPriceMonthlyTrendRow(
        SalesPriceTrendRowKey key,
        IReadOnlyList<SalesPriceTrendSourceRow> groupRows,
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
            ReferenceCode = key.CustomerCode,
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
        var row = new TaktSalesPriceMonthlyTrendDto
        {
            PlantCode = key.PlantCode,
            MaterialCode = key.MaterialCode,
            CustomerCode = key.CustomerCode,
            MaterialDescription = string.Empty,
            CustomerName = string.Empty,
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
            periodUnitPrices[period] = RoundSalesPriceUnitPrice(point.UnitPrice);
            // 当月有价=yyyy-MM；缺月回填=最近价格日期 yyyy-MM-dd（与移动价 * 说明一致）
            periodPriceSourcePeriods[period] = TaktPriceTrendAnalysisHelper.ResolvePeriodPriceSourceLabel(point);
            if (!string.IsNullOrWhiteSpace(point.Unit))
            {
                row.Unit = point.Unit;
            }
        }
        ApplySalesPriceFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 按关注月计算环比涨跌
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplySalesPriceFocusTrend(TaktSalesPriceMonthlyTrendDto row, string? focusPeriod)
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
        row.VarianceAmount = RoundSalesPriceUnitPrice(comparePrice - basePrice);
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
    /// <param name="trendFilter">筛选码</param>
    /// <returns>筛选后行</returns>
    private static List<TaktSalesPriceMonthlyTrendDto> FilterSalesPriceTrendRows(
        IReadOnlyList<TaktSalesPriceMonthlyTrendDto> rows,
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
    private static List<TaktSalesPriceMonthlyTrendDto> OrderSalesPriceTrendRows(
        IReadOnlyList<TaktSalesPriceMonthlyTrendDto> rows)
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
            .ThenBy(r => r.CustomerCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 回填物料描述 / 客户名称
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="rows">推移行</param>
    private async Task FillSalesPriceTrendDisplayNamesAsync(
        string plantCode,
        IReadOnlyList<TaktSalesPriceMonthlyTrendDto> rows)
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
        var customerCodes = rows
            .Select(r => r.CustomerCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var materialNames = await LoadMaterialNameLookupAsync(plantCode, materialCodes);
        var customerNames = await LoadCustomerNameLookupAsync(plantCode, customerCodes);
        foreach (var row in rows)
        {
            if (materialNames.TryGetValue(row.MaterialCode, out var materialDescription))
            {
                row.MaterialDescription = materialDescription;
            }
            if (customerNames.TryGetValue(row.CustomerCode, out var customerName))
            {
                row.CustomerName = customerName;
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
    /// 加载工厂客户名称字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="customerCodes">客户编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadCustomerNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> customerCodes)
    {
        if (customerCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = customerCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var customers = await _customerRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && batch.Contains(x.CustomerCode));
            foreach (var group in customers
                .Where(c => !string.IsNullOrWhiteSpace(c.CustomerCode))
                .GroupBy(c => c.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.CustomerName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 单价四舍五入至 5 位
    /// </summary>
    /// <param name="value">单价</param>
    /// <returns>四舍五入后单价</returns>
    private static decimal RoundSalesPriceUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 销售价格推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="MaterialCode">物料编码</param>
    /// <param name="CustomerCode">客户编码</param>
    private sealed record SalesPriceTrendRowKey(string PlantCode, string MaterialCode, string CustomerCode);

    /// <summary>
    /// 销售价格推移行键比较器
    /// </summary>
    private sealed class SalesPriceTrendRowKeyComparer : IEqualityComparer<SalesPriceTrendRowKey>
    {
        /// <summary>单例</summary>
        public static SalesPriceTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(SalesPriceTrendRowKey? x, SalesPriceTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.MaterialCode, y.MaterialCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.CustomerCode, y.CustomerCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public int GetHashCode(SalesPriceTrendRowKey obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.PlantCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.MaterialCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.CustomerCode));
    }

    /// <summary>
    /// 销售价格推移源行（主表 + 明细）
    /// </summary>
    private sealed class SalesPriceTrendSourceRow
    {
        /// <summary>销售价格主表</summary>
        public TaktSalesPrice Master { get; init; } = null!;

        /// <summary>销售价格明细</summary>
        public TaktSalesPriceItem Item { get; init; } = null!;
    }

    /// <summary>
    /// 销售价格月推移内存构建结果
    /// </summary>
    private sealed class SalesPriceMonthlyTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktSalesPriceMonthlyTrendDto> OrderedRows { get; init; } = new();

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
        public static SalesPriceMonthlyTrendAnalysisBuilt Empty() => new();
    }

    /// <summary>
    /// 销售机种推移内存构建结果
    /// </summary>
    private sealed class SalesPriceModelTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktSalesPriceModelTrendDto> OrderedRows { get; init; } = new();

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
        public static SalesPriceModelTrendAnalysisBuilt Empty() => new();
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
    /// DISTINCT 产品→机种（成本汇总表回退；支持 SAP 物料码归一匹配）
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
