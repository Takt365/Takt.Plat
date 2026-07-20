// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostService.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
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
/// BOM物料成本应用服务
/// </summary>
public class TaktBomMaterialCostService : TaktServiceBase, ITaktBomMaterialCostService
{
    /// <summary>
    /// 机种分组扫描产品行上限（防止全表入内存）
    /// </summary>
    private const int MaxModelGroupScanRows = 20000;

    /// <summary>
    /// 级联下拉选项上限
    /// </summary>
    private const int MaxCascadeSelectOptions = 500;

    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostRepository">BOM物料成本仓储</param>
    /// <param name="bomMaterialCostItemRepository">BomMaterialCostItem仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储（成本合计仅 FERT）</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostService(
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取BOM物料成本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBomMaterialCostDto>> GetBomMaterialCostListAsync(TaktBomMaterialCostQueryDto queryDto)
    {
        var pageIndex = Math.Max(1, queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _bomMaterialCostRepository.GetPagedAsync(
            pageIndex,
            pageSize,
            predicate);
        return TaktPagedResult<TaktBomMaterialCostDto>.Create(
            data.Adapt<List<TaktBomMaterialCostDto>>(),
            total,
            pageIndex,
            pageSize);
    }

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktBomMaterialCostModelGroupDto>> GetBomMaterialCostModelGroupListAsync(
        TaktBomMaterialCostQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        queryDto ??= new TaktBomMaterialCostQueryDto();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var predicate = QueryExpression(queryDto);
        var rows = await _bomMaterialCostRepository.GetListForExportAsync(predicate, MaxModelGroupScanRows);
        if (rows.Count >= MaxModelGroupScanRows)
        {
            throw new TaktBusinessException(
                $"机种汇总扫描行数为 {rows.Count}，达到上限 {MaxModelGroupScanRows}，请缩小筛选（如工厂/机种/核算期间）");
        }
        var groups = rows
            .GroupBy(
                x => $"{x.PlantCode?.Trim() ?? string.Empty}|{x.ModelCode?.Trim() ?? string.Empty}|{x.CostingPeriod?.Trim() ?? string.Empty}",
                StringComparer.OrdinalIgnoreCase)
            .Select(g =>
            {
                var first = g.OrderByDescending(x => x.CostingDate).First();
                return new TaktBomMaterialCostModelGroupDto
                {
                    GroupKey = g.Key,
                    PlantCode = first.PlantCode ?? string.Empty,
                    ModelCode = first.ModelCode ?? string.Empty,
                    ModelMonthlyAverageCost = first.ModelMonthlyAverageCost,
                    CurrencyCode = first.CurrencyCode ?? string.Empty,
                    CostingPeriod = first.CostingPeriod ?? string.Empty,
                    CostingDate = first.CostingDate,
                    ProductRowCount = g.Count(),
                };
            })
            .OrderByDescending(x => x.CostingPeriod)
            .ThenBy(x => x.PlantCode)
            .ThenBy(x => x.ModelCode)
            .ToList();
        var total = groups.Count;
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var page = groups.Skip(skip).Take(pageSize).ToList();
        return TaktPagedResult<TaktBomMaterialCostModelGroupDto>.Create(page, total, pageIndex, pageSize);
    }

    /// <summary>
    /// 根据ID获取BOM物料成本
    /// </summary>
    /// <param name="id">BOM物料成本ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostDto?> GetBomMaterialCostByIdAsync(long id)
    {
        var entity = await _bomMaterialCostRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBomMaterialCostDto>();
    }

    /// <summary>
    /// 获取BOM物料成本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetBomMaterialCostModelOptionsAsync(string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var trimmedPlant = plantCode?.Trim();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.ModelCode ?? string.Empty,
            false);
        var modelNameLookup = await BuildBomMaterialCostModelNameLookupAsync();
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ModelCode))
            .GroupBy(e => e.ModelCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Take(MaxCascadeSelectOptions)
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
    /// 构建机种名称查找表（型号目的地 ModelCode → ModelName）
    /// </summary>
    /// <returns>机种编码→名称</returns>
    private async Task<Dictionary<string, string>> BuildBomMaterialCostModelNameLookupAsync()
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
    /// 创建BOM物料成本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostDto> CreateBomMaterialCostAsync(TaktBomMaterialCostCreateDto dto)
    {
        var entity = dto.Adapt<TaktBomMaterialCost>();
        ApplyCostingPeriod(entity);
        await EnsureBomMaterialCostMonthUniqueAsync(entity);
        entity = await _bomMaterialCostRepository.CreateAsync(entity);
        return await GetBomMaterialCostByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostDto>();
    }

    /// <summary>
    /// 更新BOM物料成本
    /// </summary>
    /// <param name="id">BOM物料成本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostDto> UpdateBomMaterialCostAsync(long id, TaktBomMaterialCostUpdateDto dto)
    {
        var entity = await _bomMaterialCostRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM物料成本不存在");
        }
        dto.Adapt(entity);
        ApplyCostingPeriod(entity);
        await EnsureBomMaterialCostMonthUniqueAsync(entity, id);
        await _bomMaterialCostRepository.UpdateAsync(entity);
        return await GetBomMaterialCostByIdAsync(id) ?? throw new TaktBusinessException("BOM物料成本不存在");
    }

    /// <summary>
    /// 删除BOM物料成本
    /// </summary>
    /// <param name="id">BOM物料成本ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostByIdAsync(long id)
    {
        var entity = await _bomMaterialCostRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM物料成本不存在或已删除");
        }
        // 仅删汇总行；明细为源数据，不级联删除
        var deleted = await _bomMaterialCostRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("BOM物料成本不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除BOM物料成本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBomMaterialCostByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBomMaterialCostTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBomMaterialCostTemplateDto>(
            sheetName ?? "BOM物料成本导入模板",
            fileName ?? "BOM物料成本导入模板.xlsx");
    }

    /// <summary>
    /// 导入BOM物料成本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBomMaterialCostImportDto>(fileStream, sheetName ?? "BOM物料成本导入模板");
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
                var entity = rows[i].Adapt<TaktBomMaterialCost>();
                ApplyCostingPeriod(entity);
                var importKey = $"{entity.PlantCode}|{entity.ModelCode}|{entity.ProductCode}|{entity.CostingPeriod}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ModelCode、ProductCode、CostingPeriod）");
                }
                await EnsureBomMaterialCostMonthUniqueAsync(entity);
                await _bomMaterialCostRepository.CreateAsync(entity);
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
    /// 导出BOM物料成本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAsync(TaktBomMaterialCostQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBomMaterialCostQueryDto());
        var list = await _bomMaterialCostRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomMaterialCostExportDto>(),
                sheetName ?? "BOM物料成本数据",
                fileName ?? "BOM物料成本导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBomMaterialCostExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM物料成本数据",
            fileName ?? "BOM物料成本导出.xlsx");
    }

    // ========================================
    // 明细回算汇总表（同月同机种同产品 Upsert；无线上外键挂接）
    // ========================================

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostDto?> SyncBomMaterialCostFromItemsAsync(
        string plantCode,
        string productCode,
        DateTime costingDate)
    {
        EnsureThreeLayerContext();
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        var plant = plantCode.Trim();
        var product = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode.Trim());
        if (string.IsNullOrWhiteSpace(product))
        {
            product = productCode.Trim();
        }
        // 成本汇总仅接受成品：工厂物料 MaterialType 必须为 FERT
        var plantMaterials = await _materialPlantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant);
        if (!TaktBomMaterialCostItemLineCostHelper.IsFertPlantProduct(plantMaterials, plant, product))
        {
            return null;
        }
        // 主表 CostingDate 必须与本次合计所用明细 CostingDate 严格一致（日历日）
        var syncCostingDate = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(costingDate);
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(syncCostingDate);
        var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodKey);
        var modelCode = await ResolveModelCodeByProductAsync(product);
        var monthItems = await _bomMaterialCostItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.CostingDate >= monthStart
                && x.CostingDate <= monthEnd
                && x.ProductCode != null);
        var productItems = monthItems
            .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, product))
            .ToList();
        // 唯一维度：工厂 + 机种 + 产品 + 核算月 → 不存在则创建，存在则更新
        var existing = await FindHeaderByMonthKeyAsync(plant, modelCode, product, periodKey);
        if (productItems.Count == 0)
        {
            if (existing == null)
            {
                return null;
            }
            existing.ModelCode = string.IsNullOrWhiteSpace(modelCode) ? existing.ModelCode : modelCode;
            existing.ProductMonthlyCost = 0;
            existing.CostingDate = syncCostingDate;
            existing.CostingPeriod = periodKey;
            await _bomMaterialCostRepository.UpdateAsync(existing);
            await RefreshModelMonthlyAverageForPeriodAsync(plant, existing.ModelCode, periodKey);
            return await GetBomMaterialCostByIdAsync(existing.Id);
        }
        // 优先按传入核算日取快照；该日无 X+F 行时回退到同月最后核算日（仍用明细真实日期）
        var headerCostingDate = syncCostingDate;
        var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolveDateSnapshot(
            productItems, plant, product, headerCostingDate);
        if (snapshot.Count == 0)
        {
            var latestInMonth = TaktBomMaterialCostItemLineCostHelper.ResolveLatestCostingDate(
                productItems, plant, product, periodKey);
            if (latestInMonth != null)
            {
                headerCostingDate = latestInMonth.Value;
                snapshot = TaktBomMaterialCostItemLineCostHelper.ResolveDateSnapshot(
                    productItems, plant, product, headerCostingDate);
            }
        }
        var productMonthlyCost = TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(snapshot);
        var currency = snapshot
            .Select(TaktBomMaterialCostItemLineCostHelper.ResolveCurrency)
            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? productItems
                .Select(x => x.MovingPriceCurrency)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? string.Empty;
        var productDescription = snapshot.FirstOrDefault()?.ProductDescription
            ?? productItems
                .OrderByDescending(x => TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(x.CostingDate))
                .First().ProductDescription
            ?? string.Empty;
        if (existing == null)
        {
            // 同月同机种同产品主表不存在 → 新增
            existing = new TaktBomMaterialCost
            {
                PlantCode = plant,
                ModelCode = modelCode,
                ProductCode = product,
                ProductDescription = productDescription,
                ProductMonthlyCost = productMonthlyCost,
                ModelMonthlyAverageCost = 0,
                CurrencyCode = currency,
                CostingPeriod = periodKey,
                CostingDate = headerCostingDate,
            };
            existing = await _bomMaterialCostRepository.CreateAsync(existing);
        }
        else
        {
            // 同月同机种同产品主表已存在 → 更新（含机种回填、币种、产品月成本、核算日与明细对齐）
            existing.ModelCode = string.IsNullOrWhiteSpace(modelCode) ? existing.ModelCode : modelCode;
            existing.ProductCode = product;
            existing.ProductDescription = productDescription;
            existing.ProductMonthlyCost = productMonthlyCost;
            existing.CurrencyCode = currency;
            existing.CostingPeriod = periodKey;
            existing.CostingDate = headerCostingDate;
            await _bomMaterialCostRepository.UpdateAsync(existing);
        }
        var effectiveModel = string.IsNullOrWhiteSpace(existing.ModelCode) ? modelCode : existing.ModelCode;
        await RefreshModelMonthlyAverageForPeriodAsync(plant, effectiveModel, periodKey);
        return await GetBomMaterialCostByIdAsync(existing.Id);
    }

    /// <inheritdoc />
    public async Task SyncBomMaterialCostFromItemsBatchAsync(
        IEnumerable<(string PlantCode, string ProductCode, DateTime CostingDate)> keys)
    {
        ArgumentNullException.ThrowIfNull(keys);
        var distinct = keys
            .Where(k => !string.IsNullOrWhiteSpace(k.PlantCode) && !string.IsNullOrWhiteSpace(k.ProductCode))
            .Select(k =>
            {
                var costingDate = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(k.CostingDate);
                return (
                    PlantCode: k.PlantCode.Trim(),
                    ProductCode: k.ProductCode.Trim(),
                    Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(costingDate),
                    CostingDate: costingDate);
            })
            .GroupBy(k => $"{k.PlantCode}|{k.ProductCode}|{k.Period}", StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(x => x.CostingDate).First())
            .ToList();
        foreach (var key in distinct)
        {
            await SyncBomMaterialCostFromItemsAsync(key.PlantCode, key.ProductCode, key.CostingDate);
        }
    }

    /// <summary>
    /// 规范化核算期间（空则由核算日期推导）
    /// </summary>
    /// <param name="entity">主表实体</param>
    private static void ApplyCostingPeriod(TaktBomMaterialCost entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (string.IsNullOrWhiteSpace(entity.CostingPeriod))
        {
            entity.CostingPeriod = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(entity.CostingDate);
        }
        else
        {
            entity.CostingPeriod = entity.CostingPeriod.Trim();
        }
    }

    /// <summary>
    /// 校验同工厂+机种+产品+核算期间唯一
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="excludeId">排除的主键（更新时）</param>
    /// <returns>任务</returns>
    private async Task EnsureBomMaterialCostMonthUniqueAsync(TaktBomMaterialCost entity, long? excludeId = null)
    {
        ApplyCostingPeriod(entity);
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _bomMaterialCostRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ModelCode == entity.ModelCode
                && x.ProductCode == entity.ProductCode
                && x.CostingPeriod == entity.CostingPeriod,
            excludeId);
        if (!isUnique)
        {
            throw new TaktBusinessException("BOM物料成本的PlantCode、ModelCode、ProductCode、CostingPeriod（同月）已存在，请更新原记录");
        }
    }

    /// <summary>
    /// 按产品编码从型号目的地解析机种（同步新增机种）
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <returns>机种编码；未匹配时为空</returns>
    private async Task<string> ResolveModelCodeByProductAsync(string productCode)
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialCode != null);
        var match = list.FirstOrDefault(x =>
            TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.MaterialCode, productCode));
        return match?.ModelCode?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 查找同月主表行（工厂 + 机种 + 产品 + 核算期间；机种空时按工厂+产品+期间匹配以便补机种）
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
        var productMatches = list
            .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, productCode))
            .ToList();
        if (productMatches.Count == 0)
        {
            return null;
        }
        if (!string.IsNullOrWhiteSpace(modelCode))
        {
            var byModel = productMatches.FirstOrDefault(x =>
                string.Equals(x.ModelCode, modelCode, StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrWhiteSpace(x.ModelCode));
            if (byModel != null)
            {
                return byModel;
            }
        }
        return productMatches[0];
    }

    /// <summary>
    /// 刷新同工厂+机种+核算期间下全部主表行的机种月平均成本（仅主表已有产品行，不用型号目的地扩编）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="periodKey">期间</param>
    /// <returns>任务</returns>
    private async Task RefreshModelMonthlyAverageForPeriodAsync(
        string plantCode,
        string modelCode,
        string periodKey)
    {
        if (string.IsNullOrWhiteSpace(modelCode))
        {
            return;
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ModelCode == modelCode
                && x.CostingPeriod == periodKey);
        if (headers.Count == 0)
        {
            return;
        }
        var materialPlants = await _materialPlantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode);
        var costs = headers
            .Where(h => h.ProductMonthlyCost > 0
                && TaktBomMaterialCostItemLineCostHelper.IsFertPlantProduct(
                    materialPlants, h.PlantCode, h.ProductCode))
            .Select(h => h.ProductMonthlyCost)
            .ToList();
        var average = TaktBomMaterialCostItemModelEnrichmentHelper.ComputeModelMonthlyAverageFromProductCosts(costs);
        foreach (var header in headers)
        {
            if (header.ModelMonthlyAverageCost == average)
            {
                continue;
            }
            header.ModelMonthlyAverageCost = average;
            await _bomMaterialCostRepository.UpdateAsync(header);
        }
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建BOM物料成本查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBomMaterialCost, bool>> QueryExpression(TaktBomMaterialCostQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBomMaterialCost>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || SqlFunc.ToString(x.ModelMonthlyAverageCost).Contains(keywords)
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords))
                || SqlFunc.ToString(x.ProductMonthlyCost).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.CostingPeriod != null && x.CostingPeriod.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CostingDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (queryDto?.ModelMonthlyAverageCost.HasValue == true)
        {
            exp = exp.And(x => x.ModelMonthlyAverageCost == queryDto.ModelMonthlyAverageCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductCode))
        {
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(queryDto.ProductCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductDescription))
        {
            exp = exp.And(x => x.ProductDescription != null && x.ProductDescription.Contains(queryDto.ProductDescription));
        }

        if (queryDto?.ProductMonthlyCost.HasValue == true)
        {
            exp = exp.And(x => x.ProductMonthlyCost == queryDto.ProductMonthlyCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CostingPeriod))
        {
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod == queryDto.CostingPeriod);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CostingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.CostingDate >= queryDto.CostingDateStart);
        }

        if (queryDto?.CostingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.CostingDate <= queryDto.CostingDateEnd);
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
}
