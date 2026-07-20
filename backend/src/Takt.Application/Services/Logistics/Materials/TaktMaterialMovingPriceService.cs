// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceService.cs
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using System.Text;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 移动价格应用服务
/// </summary>
public class TaktMaterialMovingPriceService : TaktServiceBase, ITaktMaterialMovingPriceService
{
    /// <summary>物料名称按编码分批查询，避免超长 IN 列表</summary>
    private const int MaterialNameLookupBatchSize = 500;

    /// <summary>移动价格按年分表基表名</summary>
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    /// <summary>按 Id 探测年分表年数</summary>
    private const int YearShardProbeYears = 6;

    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingPriceRepository">移动价格仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储（产品→机种）</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialMovingPriceService(
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialMovingPriceRepository = materialMovingPriceRepository;
        _materialPlantRepository = materialPlantRepository;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取移动价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialMovingPriceDto>> GetMaterialMovingPriceListAsync(TaktMaterialMovingPriceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        string? yearTable;
        try
        {
            yearTable = await ResolveMovingPricePhysicalTableAsync(
                TaktYearShardTableHelper.RequireSingleYear(queryDto.PeriodDateStart, queryDto.PeriodDateEnd));
        }
        catch (ArgumentException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
        var (data, total) = await _materialMovingPriceRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.CreatedAt,
            true,
            yearTable);
        return TaktPagedResult<TaktMaterialMovingPriceDto>.Create(
            data.Adapt<List<TaktMaterialMovingPriceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialMovingPriceDto?> GetMaterialMovingPriceByIdAsync(long id)
    {
        var (entity, _) = await FindMovingPriceByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialMovingPriceDto>();
    }

    /// <summary>
    /// 获取物料移动价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialMovingPriceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var yearTable = await ResolveMovingPricePhysicalTableAsync(DateTime.Now.Year);
        var list = await _materialMovingPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false,
            yearTable);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建移动价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialMovingPriceDto> CreateMaterialMovingPriceAsync(TaktMaterialMovingPriceCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialMovingPrice>();
        var yearTable = await ResolveMovingPricePhysicalTableAsync(entity.PeriodDate.Year);
        await EnsureMovingPriceUniqueAsync(entity, yearTable);
        entity = await _materialMovingPriceRepository.CreateAsync(entity, yearTable);
        return await GetMaterialMovingPriceByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialMovingPriceDto>();
    }

    /// <summary>
    /// 更新移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialMovingPriceDto> UpdateMaterialMovingPriceAsync(long id, TaktMaterialMovingPriceUpdateDto dto)
    {
        var (entity, yearTable) = await FindMovingPriceByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("移动价格不存在");
        }
        var originalYear = entity.PeriodDate.Year;
        dto.Adapt(entity);
        if (entity.PeriodDate.Year != originalYear)
        {
            throw new TaktBusinessException("按年分表后不可跨年修改期间，请删除后重建");
        }
        await EnsureMovingPriceUniqueAsync(entity, yearTable, id);
        await _materialMovingPriceRepository.UpdateAsync(entity, yearTable);
        return await GetMaterialMovingPriceByIdAsync(id) ?? throw new TaktBusinessException("移动价格不存在");
    }

    /// <summary>
    /// 删除移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialMovingPriceByIdAsync(long id)
    {
        var (entity, yearTable) = await FindMovingPriceByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("移动价格不存在或已删除");
        }
        var deleted = await _materialMovingPriceRepository.DeleteAsync(id, yearTable);
        if (!deleted)
        {
            throw new TaktBusinessException("移动价格不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除移动价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialMovingPriceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialMovingPriceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialMovingPriceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialMovingPriceTemplateDto>(
            sheetName ?? "移动价格导入模板",
            fileName ?? "移动价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入移动价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialMovingPriceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialMovingPriceImportDto>(fileStream, sheetName ?? "移动价格导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktMaterialMovingPrice>();
                var importKey = $"{entity.PlantCode}|{entity.PeriodDate}|{entity.MaterialCode}|{entity.Valuation}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PeriodDate、MaterialCode、Valuation）");
                }
                var yearTable = await ResolveMovingPricePhysicalTableAsync(entity.PeriodDate.Year);
                await EnsureMovingPriceUniqueAsync(entity, yearTable);
                await _materialMovingPriceRepository.CreateAsync(entity, yearTable);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出移动价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceAsync(TaktMaterialMovingPriceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        query ??= new TaktMaterialMovingPriceQueryDto();
        var predicate = QueryExpression(query);
        var list = await GetMovingPriceListForRangeAsync(predicate, query.PeriodDateStart, query.PeriodDateEnd);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialMovingPriceExportDto>(),
                sheetName ?? "移动价格数据",
                fileName ?? "移动价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialMovingPriceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "移动价格数据",
            fileName ?? "移动价格导出.xlsx");
    }

    /// <inheritdoc />
    public async Task<TaktMaterialMovingPriceMonthlyTrendResultDto> GetMaterialMovingPriceMonthlyTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktMaterialMovingPriceMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktMaterialMovingPriceMonthlyTrendDto>.Create(
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
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceMonthlyTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "materialCode", "materialName", "valuation", "currency",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "物料编码", "物料名称", "评估类别", "币种",
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
                ["materialName"] = row.MaterialName,
                ["valuation"] = row.Valuation,
                ["currency"] = row.Currency,
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
            sheetName ?? "物料移动价格推移清单",
            fileName ?? $"物料移动价格推移清单_{query.PlantCode}.xlsx");
    }

    /// <inheritdoc />
    public async Task<TaktMaterialMovingPriceModelTrendResultDto> GetMaterialMovingPriceModelTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var monthly = await BuildMonthlyTrendAnalysisAsync(queryDto);
        if (monthly.OrderedRows.Count == 0)
        {
            return new TaktMaterialMovingPriceModelTrendResultDto
            {
                Paged = TaktPagedResult<TaktMaterialMovingPriceModelTrendDto>.Create(
                    new List<TaktMaterialMovingPriceModelTrendDto>(), 0, pageIndex, pageSize),
                PeriodOrder = monthly.PeriodOrder,
                MaterialCount = 0,
                BasePeriod = monthly.BasePeriod,
                ComparePeriod = monthly.ComparePeriod,
            };
        }
        // 仅对当前页物料做 BOM 关联（全量 2 万+ 物料关联明细会超时）
        var pageMonthly = monthly.OrderedRows.Skip(skip).Take(pageSize).ToList();
        var usage = await LoadBomMaterialUsageLookupAsync(
            queryDto.PlantCode.Trim(),
            pageMonthly.Select(r => r.MaterialCode).ToList());
        var pageRows = EnrichModelTrendRows(pageMonthly, usage);
        return new TaktMaterialMovingPriceModelTrendResultDto
        {
            Paged = TaktPagedResult<TaktMaterialMovingPriceModelTrendDto>.Create(
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
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceModelTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildModelTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "materialCode", "modelGroup", "productGroup", "materialText",
        };
        var columnLabels = new List<string>
        {
            "物料编码", "机种组", "产品组", "物料描述",
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
                ["trend"] = row.Trend,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent,
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
            sheetName ?? "物料机种价格推移清单",
            fileName ?? $"物料机种价格推移清单_{query.PlantCode}.xlsx");
    }

    /// <summary>
    /// 构建物料-机种-价格推移全量结果（导出用；BOM 用 DISTINCT 轻量查询）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>构建结果</returns>
    private async Task<ModelTrendAnalysisBuilt> BuildModelTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto)
    {
        var monthly = await BuildMonthlyTrendAnalysisAsync(queryDto);
        if (monthly.OrderedRows.Count == 0)
        {
            return ModelTrendAnalysisBuilt.Empty();
        }
        var plantCode = queryDto.PlantCode.Trim();
        var usage = await LoadBomMaterialUsageLookupAsync(
            plantCode,
            monthly.OrderedRows.Select(r => r.MaterialCode).ToList());
        var enriched = EnrichModelTrendRows(monthly.OrderedRows, usage);
        return new ModelTrendAnalysisBuilt
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
    private static List<TaktMaterialMovingPriceModelTrendDto> EnrichModelTrendRows(
        IReadOnlyList<TaktMaterialMovingPriceMonthlyTrendDto> monthlyRows,
        IReadOnlyDictionary<string, BomMaterialUsageInfo> usage)
    {
        return monthlyRows.Select(row =>
        {
            usage.TryGetValue(row.MaterialCode, out var info);
            var productCodes = info?.ProductCodes ?? new List<string>();
            var modelCodes = info?.ModelCodes ?? new List<string>();
            var bomText = info?.ComponentDescription ?? string.Empty;
            var materialText = !string.IsNullOrWhiteSpace(row.MaterialName)
                ? row.MaterialName
                : bomText;
            return new TaktMaterialMovingPriceModelTrendDto
            {
                PlantCode = row.PlantCode,
                MaterialCode = row.MaterialCode,
                MaterialName = row.MaterialName,
                Valuation = row.Valuation,
                Currency = row.Currency,
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

        // 小批量：按组件 IN 查 DISTINCT；大批量导出：工厂级 DISTINCT 再内存过滤（避免拉全量 CostingDate 明细实体）
        if (codes.Count <= MaterialNameLookupBatchSize)
        {
            await FillComponentProductPairsByCodesAsync(plantCode, codes, materialToProducts, allProducts);
        }
        else
        {
            await FillComponentProductPairsForPlantAsync(plantCode, new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase), materialToProducts, allProducts);
        }

        var productToModels = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var productList = allProducts.ToList();
        // 与 BOM 汇总同步同口径：产品→机种优先型号目的地，再回退成本汇总表
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

    /// <summary>
    /// 构建物料月移动价格推移全量结果（不分页；列表/导出共用）。
    /// ① 按 Plant + MaterialCode 去重得到物料清单（不区分 PeriodDate / Valuation）；
    /// ② 再按查询期间转置各月单价（缺月回填用历史价）。
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>排序后的全量行与汇总</returns>
    private async Task<MonthlyTrendAnalysisBuilt> BuildMonthlyTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var valuationFilter = string.IsNullOrWhiteSpace(queryDto.Valuation) ? null : queryDto.Valuation.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var (periodStart, periodEnd) = NormalizePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);

        // 清单源：工厂下近年分表内移动价格行（不含 PeriodDate 条件），再 Distinct MaterialCode
        var rosterExp = BuildTrendSourceExpression(plantCode, valuationFilter, materialFilter, periodStart: null, periodEnd: null);
        var rosterStart = periodStart?.AddYears(-5) ?? new DateTime(DateTime.Now.Year - YearShardProbeYears, 1, 1);
        var rosterEnd = periodEnd ?? DateTime.Now;
        var rosterSourceRows = await GetMovingPriceListForRangeAsync(rosterExp, rosterStart, rosterEnd);
        if (rosterSourceRows.Count == 0)
        {
            return MonthlyTrendAnalysisBuilt.Empty();
        }
        var materialCodes = rosterSourceRows
            .Where(r => !string.IsNullOrWhiteSpace(r.MaterialCode))
            .Select(r => r.MaterialCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (materialCodes.Count == 0)
        {
            return MonthlyTrendAnalysisBuilt.Empty();
        }

        // 转置/缺月回填：保留展示期止以前的全部历史价（不再截断为仅前 36 个月，
        // 否则如 2023-04-30 在查 2026 年时会被裁掉，既无回填也无 *）
        IEnumerable<TaktMaterialMovingPrice> priceQuery = rosterSourceRows;
        if (periodEnd.HasValue)
        {
            var periodEndExclusive = periodEnd.Value.AddMonths(1);
            priceQuery = priceQuery.Where(r => r.PeriodDate < periodEndExclusive);
        }
        var priceRows = priceQuery.ToList();
        var priceByMaterial = priceRows
            .Where(r => !string.IsNullOrWhiteSpace(r.MaterialCode))
            .GroupBy(r => r.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<TaktMaterialMovingPrice>)g.ToList(), StringComparer.OrdinalIgnoreCase);

        var periodOrder = BuildPeriodOrder(priceRows, periodStart, periodEnd);
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var nameLookup = await LoadMaterialNameLookupAsync(plantCode, materialCodes);
        var emptyRows = (IReadOnlyList<TaktMaterialMovingPrice>)Array.Empty<TaktMaterialMovingPrice>();
        var allRows = materialCodes
            .Select(code =>
            {
                if (!priceByMaterial.TryGetValue(code, out var materialRows))
                {
                    materialRows = emptyRows;
                }
                nameLookup.TryGetValue(code, out var materialName);
                return BuildMonthlyTrendRow(
                    plantCode,
                    code,
                    materialName ?? string.Empty,
                    ResolveDisplayValuation(materialRows, focusPeriod),
                    materialRows,
                    periodOrder,
                    focusPeriod);
            })
            .ToList();

        var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderTrendRows(filtered);
        return new MonthlyTrendAnalysisBuilt
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
    /// 构建推移分析源数据条件（租户/公司/工厂；可选评估、物料关键字、期间）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="valuationFilter">评估类别；空则不过滤</param>
    /// <param name="materialFilter">物料编码包含；空则不过滤</param>
    /// <param name="periodStart">期间起（含）；空则不按期间过滤</param>
    /// <param name="periodEnd">期间止（月初，含当月）；空则不按期间过滤</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktMaterialMovingPrice, bool>> BuildTrendSourceExpression(
        string plantCode,
        string? valuationFilter,
        string? materialFilter,
        DateTime? periodStart,
        DateTime? periodEnd)
    {
        var exp = Expressionable.Create<TaktMaterialMovingPrice>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        if (periodStart.HasValue)
        {
            var start = periodStart.Value;
            exp = exp.And(x => x.PeriodDate >= start);
        }
        if (periodEnd.HasValue)
        {
            var periodEndExclusive = periodEnd.Value.AddMonths(1);
            exp = exp.And(x => x.PeriodDate < periodEndExclusive);
        }
        if (!string.IsNullOrWhiteSpace(valuationFilter))
        {
            exp = exp.And(x => x.Valuation == valuationFilter);
        }
        if (!string.IsNullOrWhiteSpace(materialFilter))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialFilter));
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 加载工厂物料名称字典
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
                map[group.Key] = group.Select(x => x.MaterialName)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 构建单行物料月推移
    /// </summary>
    private static TaktMaterialMovingPriceMonthlyTrendDto BuildMonthlyTrendRow(
        string plantCode,
        string materialCode,
        string materialName,
        string valuation,
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        // 使用无自定义比较器的字典，避免 JSON 序列化后前端读不到来源月（* 标记）
        var periodUnitPrices = new Dictionary<string, decimal>();
        var periodPriceSourcePeriods = new Dictionary<string, string>();
        string currency = string.Empty;
        decimal? lastUnitPrice = null;
        string? lastSourcePeriod = null;
        string lastCurrency = string.Empty;

        if (periodOrder.Count > 0)
        {
            // 展示期首月之前的最近有价月（含 2023-04-30 这类月末日期）作为回填种子
            var seed = PickMostRecentPositiveOnOrBefore(materialRows, periodOrder[0], exclusive: true);
            if (seed != null && TryResolveTrendUnitPrice(seed, out var seedPrice))
            {
                lastUnitPrice = seedPrice;
                lastSourcePeriod = ToPeriodKey(seed.PeriodDate);
                lastCurrency = seed.Currency?.Trim() ?? string.Empty;
            }
        }
        foreach (var period in periodOrder)
        {
            var picked = PickForPeriod(materialRows, period);
            if (picked != null && TryResolveTrendUnitPrice(picked, out var periodUnit))
            {
                // 当月有正移动价：采用当月，来源=当月（前端不标 *）
                lastUnitPrice = periodUnit;
                lastSourcePeriod = period;
                if (!string.IsNullOrWhiteSpace(picked.Currency))
                {
                    lastCurrency = picked.Currency.Trim();
                }
            }
            // 当月无行或无正价：沿用最近有价月；来源月≠展示月 → 前端标 *
            if (!lastUnitPrice.HasValue || string.IsNullOrWhiteSpace(lastSourcePeriod))
            {
                continue;
            }
            periodUnitPrices[period] = lastUnitPrice.Value;
            periodPriceSourcePeriods[period] = lastSourcePeriod;
            if (string.IsNullOrWhiteSpace(currency) && !string.IsNullOrWhiteSpace(lastCurrency))
            {
                currency = lastCurrency;
            }
        }
        var row = new TaktMaterialMovingPriceMonthlyTrendDto
        {
            PlantCode = plantCode,
            MaterialCode = materialCode,
            MaterialName = materialName,
            Valuation = valuation,
            Currency = currency,
            PeriodUnitPrices = periodUnitPrices,
            PeriodPriceSourcePeriods = periodPriceSourcePeriods,
        };
        ApplyFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// PeriodDate → yyyy-MM（按年月，兼容月末 2023-04-30）
    /// </summary>
    /// <param name="periodDate">期间日期</param>
    /// <returns>yyyy-MM</returns>
    private static string ToPeriodKey(DateTime periodDate) =>
        new DateTime(periodDate.Year, periodDate.Month, 1).ToString("yyyy-MM");

    /// <summary>
    /// 展示用评估类别：优先关注月选中行，否则取最近一期有评估类别的行
    /// </summary>
    /// <param name="materialRows">同一物料全部期间行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <returns>评估类别</returns>
    private static string ResolveDisplayValuation(
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        string? focusPeriod)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            var focused = PickForPeriod(materialRows, focusPeriod.Trim());
            if (!string.IsNullOrWhiteSpace(focused?.Valuation))
            {
                return focused.Valuation.Trim();
            }
        }
        return materialRows
            .OrderByDescending(r => r.PeriodDate)
            .Select(r => r.Valuation?.Trim())
            .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))
            ?? string.Empty;
    }

    /// <summary>
    /// 选取某期间价格行（同月多行：V 优先、库存数量较大优先；可跨评估类别）
    /// </summary>
    private static TaktMaterialMovingPrice? PickForPeriod(
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        string periodKey)
    {
        return materialRows
            .Where(r => ToPeriodKey(r.PeriodDate) == periodKey)
            .OrderByDescending(r => string.Equals(r.PriceControl, "V", StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(r => r.StockQuantity)
            .ThenBy(r => r.Valuation, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();
    }

    /// <summary>
    /// 选取严格早于/不晚于目标月、且有正移动价的最近有价行（用于区间前种子与缺月回填）
    /// </summary>
    private static TaktMaterialMovingPrice? PickMostRecentPositiveOnOrBefore(
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        string periodKey,
        bool exclusive)
    {
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var targetMonth))
        {
            return null;
        }
        var candidates = materialRows
            .Select(r => (Row: r, Month: new DateTime(r.PeriodDate.Year, r.PeriodDate.Month, 1)))
            .Where(x => exclusive ? x.Month < targetMonth : x.Month <= targetMonth)
            .Where(x => TryResolveTrendUnitPrice(x.Row, out _))
            .ToList();
        if (candidates.Count == 0)
        {
            return null;
        }
        var latestMonth = candidates.Max(x => x.Month);
        return candidates
            .Where(x => x.Month == latestMonth)
            .Select(x => x.Row)
            .OrderByDescending(r => string.Equals(r.PriceControl, "V", StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(r => r.StockQuantity)
            .First();
    }

    /// <summary>
    /// 解析推移展示单价：MovingPrice&gt;0 视为有价；单价=MovingPrice/PriceUnit（过大单位导致四舍五入为 0 时回退 MovingPrice）
    /// </summary>
    /// <param name="row">价格行</param>
    /// <param name="unitPrice">展示单价</param>
    /// <returns>是否有价</returns>
    private static bool TryResolveTrendUnitPrice(TaktMaterialMovingPrice row, out decimal unitPrice)
    {
        ArgumentNullException.ThrowIfNull(row);
        unitPrice = 0m;
        if (row.MovingPrice <= 0m)
        {
            return false;
        }
        var unit = row.PriceUnit <= 0 ? 1 : row.PriceUnit;
        unitPrice = RoundUnitPrice(row.MovingPrice / unit);
        if (unitPrice <= 0m)
        {
            unitPrice = RoundUnitPrice(row.MovingPrice);
        }
        return unitPrice > 0m;
    }

    /// <summary>
    /// 按关注期间应用环比
    /// </summary>
    private static void ApplyFocusTrend(TaktMaterialMovingPriceMonthlyTrendDto row, string? focusPeriod)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                null,
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
        row.VarianceAmount = RoundUnitPrice(comparePrice - basePrice);
        if (basePrice != 0m)
        {
            // 小数比率（非百分数）：0.2978 → Excel 百分比列显示 29.78%
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
    private static List<TaktMaterialMovingPriceMonthlyTrendDto> FilterTrendRows(
        IReadOnlyList<TaktMaterialMovingPriceMonthlyTrendDto> rows,
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
    private static List<TaktMaterialMovingPriceMonthlyTrendDto> OrderTrendRows(
        IReadOnlyList<TaktMaterialMovingPriceMonthlyTrendDto> rows)
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
            .ToList();
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    private static (DateTime? Start, DateTime? End) NormalizePeriodBounds(DateTime? periodDateStart, DateTime? periodDateEnd)
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
    /// 构建期间列顺序
    /// </summary>
    private static List<string> BuildPeriodOrder(
        IReadOnlyList<TaktMaterialMovingPrice> priceRows,
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
        return priceRows
            .Select(r => new DateTime(r.PeriodDate.Year, r.PeriodDate.Month, 1).ToString("yyyy-MM"))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 解析关注期间
    /// </summary>
    private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 单价四舍五入至 5 位
    /// </summary>
    private static decimal RoundUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 物料月推移内存构建结果
    /// </summary>
    private sealed class MonthlyTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktMaterialMovingPriceMonthlyTrendDto> OrderedRows { get; init; } = new();

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
        public static MonthlyTrendAnalysisBuilt Empty() => new();
    }

    /// <summary>
    /// 物料-机种推移内存构建结果
    /// </summary>
    private sealed class ModelTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktMaterialMovingPriceModelTrendDto> OrderedRows { get; init; } = new();

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
        public static ModelTrendAnalysisBuilt Empty() => new();
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

        /// <summary>组件描述（物料描述回退）</summary>
        public string ComponentDescription { get; init; } = string.Empty;
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建移动价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialMovingPrice, bool>> QueryExpression(TaktMaterialMovingPriceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialMovingPrice>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.Valuation != null && x.Valuation.Contains(keywords))
                || SqlFunc.ToString(x.StockQuantity).Contains(keywords)
                || SqlFunc.ToString(x.StockAmount).Contains(keywords)
                || (x.PriceControl != null && x.PriceControl.Contains(keywords))
                || SqlFunc.ToString(x.MovingPrice).Contains(keywords)
                || SqlFunc.ToString(x.PriceUnit).Contains(keywords)
                || (x.Currency != null && x.Currency.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PeriodDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Valuation))
        {
            exp = exp.And(x => x.Valuation != null && x.Valuation.Contains(queryDto.Valuation));
        }

        if (queryDto?.StockQuantity.HasValue == true)
        {
            exp = exp.And(x => x.StockQuantity == queryDto.StockQuantity);
        }

        if (queryDto?.StockAmount.HasValue == true)
        {
            exp = exp.And(x => x.StockAmount == queryDto.StockAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.PriceControl))
        {
            exp = exp.And(x => x.PriceControl != null && x.PriceControl.Contains(queryDto.PriceControl));
        }

        if (queryDto?.MovingPrice.HasValue == true)
        {
            exp = exp.And(x => x.MovingPrice == queryDto.MovingPrice);
        }

        if (queryDto?.PriceUnit.HasValue == true)
        {
            exp = exp.And(x => x.PriceUnit == queryDto.PriceUnit);
        }

        if (!string.IsNullOrEmpty(queryDto?.Currency))
        {
            exp = exp.And(x => x.Currency != null && x.Currency.Contains(queryDto.Currency));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PeriodDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PeriodDate >= queryDto.PeriodDateStart);
        }

        if (queryDto?.PeriodDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PeriodDate <= queryDto.PeriodDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }

    // ========================================
    // 按年分表路由（{base}_{yyyy}）
    // ========================================

    /// <summary>
    /// 生成移动价格年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildMovingPriceYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(MovingPriceYearShardBaseTable, year);

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
    /// 按 Id 在近年分表中定位移动价格（跳过未建年分表，最后查基表）
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>实体与物理表名（基表时 Table 为 null）</returns>
    private async Task<(TaktMaterialMovingPrice? Entity, string? Table)> FindMovingPriceByIdAsync(long id)
    {
        var now = DateTime.Now.Year;
        for (var y = now + 1; y >= now - YearShardProbeYears + 1; y--)
        {
            var table = await ResolveMovingPricePhysicalTableAsync(y);
            if (table == null)
            {
                continue;
            }
            var entity = await _materialMovingPriceRepository.GetByIdAsync(id, table);
            if (entity != null)
            {
                return (entity, table);
            }
        }
        var baseEntity = await _materialMovingPriceRepository.GetByIdAsync(id);
        return (baseEntity, null);
    }

    /// <summary>
    /// 年分表（或基表）内唯一性校验
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="yearTable">物理表；null 表示基表</param>
    /// <param name="excludeId">排除 Id</param>
    private async Task EnsureMovingPriceUniqueAsync(
        TaktMaterialMovingPrice entity,
        string? yearTable,
        long? excludeId = null)
    {
        var existing = await _materialMovingPriceRepository.FirstAsync(
            x => x.PlantCode == entity.PlantCode
                && x.PeriodDate == entity.PeriodDate
                && x.MaterialCode == entity.MaterialCode
                && x.Valuation == entity.Valuation,
            yearTable);
        if (existing != null && (!excludeId.HasValue || existing.Id != excludeId.Value))
        {
            throw new TaktBusinessException("移动价格的PlantCode、PeriodDate、MaterialCode、Valuation已存在");
        }
    }

    /// <summary>
    /// 按年分表查询移动价格（可跨年合并；年分表未建时回退基表）
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="start">起</param>
    /// <param name="end">止</param>
    /// <param name="maxRows">总行上限</param>
    /// <returns>列表</returns>
    private async Task<List<TaktMaterialMovingPrice>> GetMovingPriceListForRangeAsync(
        Expression<Func<TaktMaterialMovingPrice, bool>> predicate,
        DateTime? start,
        DateTime? end,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYears(start, end);
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
            result.AddRange(basePart.Where(r => yearSet.Contains(r.PeriodDate.Year)));
        }
        return result;
    }
}
