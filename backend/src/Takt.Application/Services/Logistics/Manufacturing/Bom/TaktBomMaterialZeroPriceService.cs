// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialZeroPriceService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单应用服务（独立；FERT+建议代替末字母逆推）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
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
/// BOM 组件零价格清单服务（与成本分析服务分离）
/// </summary>
public class TaktBomMaterialZeroPriceService : TaktServiceBase, ITaktBomMaterialZeroPriceService
{
    /// <summary>
    /// 零价合并/建议价加载行上限
    /// </summary>
    private const int MaxRowLoad = 20000;

    /// <summary>BOM 成本明细按年分表基表名</summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    /// <summary>移动价格按年分表基表名</summary>
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
    /// <param name="materialMovingPriceRepository">移动价格仓储</param>
    /// <param name="companyRepository">公司仓储（RelatedPlant）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialZeroPriceService(
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
    public async Task<List<TaktSelectOption>> GetBomMaterialZeroPricePlantOptionsAsync()
    {
        EnsureThreeLayerContext();
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
    /// 查询栏机种选项：工厂 + 核算月 + MaterialType=FERT 下去重 ModelCode
    /// </summary>
    /// <param name="queryDto">工厂 + FocusPeriod</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialZeroPriceModelOptionsAsync(
        TaktBomMaterialZeroPriceModelOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var periodKey = queryDto.FocusPeriod?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(periodKey)
            || !DateTime.TryParseExact(
                periodKey,
                "yyyy-MM",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
        {
            return new List<TaktSelectOption>();
        }
        var plant = queryDto.PlantCode.Trim();
        var fertType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var headers = await _bomMaterialCostRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.CostingPeriod == periodKey
            && x.MaterialType == fertType
            && x.ModelCode != null
            && x.ModelCode != "");
        return headers
            .Select(h => h.ModelCode!.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .Select(code => new TaktSelectOption { DictValue = code, DictLabel = code })
            .ToList();
    }

    /// <summary>
    /// 组件零价格合并清单
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页合并结果</returns>
    public async Task<TaktBomMaterialZeroPriceResultDto> GetBomMaterialZeroPriceListAsync(
        TaktBomMaterialZeroPriceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);

        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(queryDto);
        var plantCode = queryDto.PlantCode.Trim();
        var modelFilters = ParseModelCodes(queryDto.ModelCodes, queryDto.ModelCode);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);

        var (productCodes, productToModels, allRows) = await BuildMergedRowsAsync(
            plantCode,
            modelFilters,
            costingStart,
            costingEnd,
            costingPeriod);

        if (allRows.Count == 0)
        {
            return new TaktBomMaterialZeroPriceResultDto
            {
                Paged = TaktPagedResult<TaktBomMaterialZeroPriceDto>.Create(
                    new List<TaktBomMaterialZeroPriceDto>(), 0, pageIndex, pageSize),
                ProductCodes = productCodes,
                ComponentCount = 0,
                CostingPeriod = costingPeriod,
            };
        }

        await FillSuggestedRevisionsAsync(plantCode, costingPeriod, allRows);

        var pageRows = allRows.Skip(skip).Take(pageSize).ToList();
        return new TaktBomMaterialZeroPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialZeroPriceDto>.Create(
                pageRows, allRows.Count, pageIndex, pageSize),
            ProductCodes = productCodes,
            ComponentCount = allRows.Count,
            CostingPeriod = costingPeriod,
        };
    }

    /// <summary>
    /// 导出组件零价格合并清单
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialZeroPriceAsync(
        TaktBomMaterialZeroPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentException.ThrowIfNullOrWhiteSpace(query.PlantCode);

        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = query.PlantCode.Trim();
        var modelFilters = ParseModelCodes(query.ModelCodes, query.ModelCode);
        var (_, _, allRows) = await BuildMergedRowsAsync(
            plantCode,
            modelFilters,
            costingStart,
            costingEnd,
            costingPeriod);
        await FillSuggestedRevisionsAsync(plantCode, costingPeriod, allRows);

        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "componentCode", "componentDescription",
            "productCodes", "productCount", "movingAveragePrice",
            "suggestedComponentCode", "suggestedMovingPrice", "costingPeriod",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "组件编码", "组件描述",
            "共用产品", "产品数", "移动平均价",
            "建议代替组件", "建议移动价格", "核算月",
        };
        var exportRows = allRows
            .Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["modelCode"] = row.ModelCode,
                ["componentCode"] = row.ComponentCode,
                ["componentDescription"] = row.ComponentDescription,
                ["productCodes"] = row.ProductCodes,
                ["productCount"] = row.ProductCount,
                ["movingAveragePrice"] = row.MovingAveragePrice,
                ["suggestedComponentCode"] = row.SuggestedComponentCode,
                ["suggestedMovingPrice"] = row.SuggestedMovingPrice,
                ["costingPeriod"] = row.CostingPeriod,
            })
            .ToList();

        var plant = plantCode;
        var modelPart = modelFilters.Count == 0
            ? "ALL"
            : string.Join("-", modelFilters);
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "BOM零价格",
            fileName ?? $"BOM零价格_{plant}_{modelPart}_{costingPeriod}.xlsx");
    }

    /// <summary>
    /// 批量/直接回填移动平均价（ComponentCode 空=批量发现组件；有值=单组件；与手工更新同一核心逻辑）
    /// </summary>
    /// <param name="dto">工厂+核算月；组件可选；机种可选（仅批量发现组件时用）</param>
    /// <returns>回填统计</returns>
    public async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> BackfillBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceMovingBackfillDto dto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);

        var query = new TaktBomMaterialZeroPriceQueryDto
        {
            PlantCode = dto.PlantCode,
            ModelCode = dto.ModelCode,
            ModelCodes = dto.ModelCodes,
            CostingDateStart = dto.CostingDateStart,
            CostingDateEnd = dto.CostingDateEnd,
        };
        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = dto.PlantCode.Trim();
        var singleComponent = string.IsNullOrWhiteSpace(dto.ComponentCode) ? null : dto.ComponentCode.Trim();
        var modelFilters = ParseModelCodes(dto.ModelCodes, dto.ModelCode);

        List<string> componentTargets;
        if (singleComponent != null)
        {
            componentTargets = new List<string> { singleComponent };
        }
        else
        {
            var (_, _, mergedRows) = await BuildMergedRowsAsync(
                plantCode,
                modelFilters,
                costingStart,
                costingEnd,
                costingPeriod,
                includeDeleted: false);
            componentTargets = mergedRows
                .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
                .Select(r => r.ComponentCode.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        if (componentTargets.Count == 0)
        {
            return new TaktBomMaterialZeroPriceMovingBackfillResultDto
            {
                ProcessedMonth = costingPeriod,
            };
        }

        var componentSources = new List<(string ComponentCode, SuggestedMovingSource Source)>();
        var skippedNoPrice = 0;
        foreach (var componentCode in componentTargets)
        {
            var source = await ResolveSuggestedMovingSourceAsync(plantCode, costingPeriod, componentCode);
            if (source == null)
            {
                skippedNoPrice = checked(skippedNoPrice + 1);
                continue;
            }
            componentSources.Add((componentCode, source));
        }
        var result = await ApplyMovingPriceBackfillCoreAsync(
            plantCode,
            costingStart,
            costingPeriod,
            componentSources);
        result.SkippedNoPriceCount = skippedNoPrice;
        return result;
    }

    /// <summary>
    /// 手工更新移动平均价（与批量/直接回填同一核心逻辑）
    /// </summary>
    /// <param name="dto">工厂+核算月+原组件+新组件+价/单位/币种</param>
    /// <returns>更新统计</returns>
    public async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> ManualUpdateBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceManualMovingDto dto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.ComponentCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.SourceComponentCode);
        if (dto.MovingAveragePrice <= 0m)
        {
            throw new TaktBusinessException("替换移动平均价必须大于 0");
        }

        var query = new TaktBomMaterialZeroPriceQueryDto
        {
            PlantCode = dto.PlantCode,
            CostingDateStart = dto.CostingDateStart,
            CostingDateEnd = dto.CostingDateEnd,
        };
        var (costingStart, _, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = dto.PlantCode.Trim();
        var componentCode = dto.ComponentCode.Trim();
        var priceUnit = dto.MovingPriceUnit <= 0 ? 1000 : dto.MovingPriceUnit;
        var currency = string.IsNullOrWhiteSpace(dto.MovingPriceCurrencyCode)
            ? "CNY"
            : dto.MovingPriceCurrencyCode.Trim().ToUpperInvariant();
        var source = new SuggestedMovingSource
        {
            SourceComponentCode = dto.SourceComponentCode.Trim(),
            ValuationPeriod = costingPeriod,
            MovingPrice = dto.MovingAveragePrice,
            PriceUnit = priceUnit,
            CurrencyCode = currency,
        };
        return await ApplyMovingPriceBackfillCoreAsync(
            plantCode,
            costingStart,
            costingPeriod,
            new List<(string ComponentCode, SuggestedMovingSource Source)> { (componentCode, source) });
    }

    /// <summary>
    /// 将 PCB SECT 整树明细写入 PcbSectIndicator=X（描述含 PCB SECT 的节点及其子孙；已标则跳过）
    /// </summary>
    /// <param name="dto">工厂+核算月；机种可选</param>
    /// <returns>打标统计</returns>
    public async Task<TaktBomMaterialZeroPricePcbSectMarkResultDto> MarkBomMaterialZeroPricePcbSectAsync(
        TaktBomMaterialZeroPricePcbSectMarkDto dto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);

        var query = new TaktBomMaterialZeroPriceQueryDto
        {
            PlantCode = dto.PlantCode,
            ModelCode = dto.ModelCode,
            ModelCodes = dto.ModelCodes,
            CostingDateStart = dto.CostingDateStart,
            CostingDateEnd = dto.CostingDateEnd,
        };
        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = dto.PlantCode.Trim();
        var modelFilters = ParseModelCodes(dto.ModelCodes, dto.ModelCode);
        var productCodes = await ResolveFertProductCodesAsync(
            plantCode,
            modelFilters,
            costingStart,
            costingEnd,
            includeDeleted: false);
        if (productCodes.Count == 0)
        {
            return new TaktBomMaterialZeroPricePcbSectMarkResultDto
            {
                ProcessedMonth = costingPeriod,
            };
        }

        var allItems = await LoadBomCostItemsRawForProductsAsync(
            plantCode,
            productCodes,
            costingStart,
            costingEnd,
            includeDeleted: false);
        var pcbRows = TaktBomMaterialCostItemLineCostHelper
            .CollectPcbSectHierarchyRows(allItems)
            .ToList();

        var updated = 0;
        var unchanged = 0;
        var rowsToUpdate = new List<TaktBomMaterialCostItem>();
        foreach (var row in pcbRows)
        {
            if (TaktBomMaterialCostItemLineCostHelper.HasPcbSectIndicatorMark(row.PcbSectIndicator))
            {
                unchanged = checked(unchanged + 1);
                continue;
            }
            if (!TaktBomMaterialCostItemLineCostHelper.TryApplyPcbSectIndicatorMark(row))
            {
                continue;
            }
            rowsToUpdate.Add(row);
            updated = checked(updated + 1);
        }

        if (rowsToUpdate.Count > 0)
        {
            var yearTable = await ResolveBomItemPhysicalTableAsync(costingStart.Year);
            const int updateChunkSize = 500;
            for (var offset = 0; offset < rowsToUpdate.Count; offset = checked(offset + updateChunkSize))
            {
                var chunk = rowsToUpdate.Skip(offset).Take(updateChunkSize).ToList();
                await _bomMaterialCostItemRepository.UpdateRangeAsync(chunk, yearTable);
            }
        }

        return new TaktBomMaterialZeroPricePcbSectMarkResultDto
        {
            ScannedRowCount = allItems.Count,
            PcbSectRowCount = pcbRows.Count,
            UpdatedRowCount = updated,
            UnchangedRowCount = unchanged,
            SkippedOverflowCount = 0,
            ProcessedMonth = costingPeriod,
        };
    }

    /// <summary>
    /// 统一核心（三步，顺序固定）：
    /// 1）明细移动价：工厂 + 核算月(日期) + 组件 → 范围内全部明细写价；
    /// 2）主表产品月计算：工厂 + 核算月 + 组件所涉产品 → ProductMonthlyCalculation = 旧值 + 本次回填行成本差额（非全量重算；不改 SAP 产品月成本）；
    /// 3）机种月平均成本：工厂 + 核算月(日期) + 物料类型 + 机种 → 按组内产品月成本算术平均。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingStart">核算月初</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="componentSources">组件与移动价源</param>
    /// <returns>回填统计</returns>
    private async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> ApplyMovingPriceBackfillCoreAsync(
        string plantCode,
        DateTime costingStart,
        string costingPeriod,
        IReadOnlyList<(string ComponentCode, SuggestedMovingSource Source)> componentSources)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentNullException.ThrowIfNull(componentSources);
        if (componentSources.Count == 0)
        {
            return new TaktBomMaterialZeroPriceMovingBackfillResultDto
            {
                ProcessedMonth = costingPeriod,
            };
        }

        var yearTable = await ResolveBomItemPhysicalTableAsync(costingStart.Year);
        var scanned = 0;
        var updated = 0;
        var unchanged = 0;
        var componentProcessed = 0;
        var affectedProductSources = new Dictionary<string, List<(string Component, SuggestedMovingSource Source)>>(
            StringComparer.OrdinalIgnoreCase);
        var lineDeltas = new List<MovingPriceLineDelta>();
        SuggestedMovingSource? firstSource = null;

        // ----------------------------------------
        // 步骤1：工厂 + 日期 + 组件 → 更新明细移动价格
        // ----------------------------------------
        foreach (var (componentCodeRaw, source) in componentSources)
        {
            ArgumentNullException.ThrowIfNull(source);
            var componentCode = componentCodeRaw.Trim();
            if (string.IsNullOrWhiteSpace(componentCode))
            {
                continue;
            }
            var items = await LoadBomCostItemsByComponentAsync(
                plantCode,
                componentCode,
                costingStart,
                costingStart,
                includeDeleted: true);
            if (items.Count == 0)
            {
                continue;
            }
            scanned = checked(scanned + items.Count);
            firstSource ??= source;
            var anyPriceUpdated = false;
            var rowsToUpdate = new List<TaktBomMaterialCostItem>();
            foreach (var row in items)
            {
                var oldPrice = row.MovingAveragePrice;
                var oldUnit = row.MovingPriceUnit <= 0 ? 1 : row.MovingPriceUnit;
                if (!TaktBomMaterialZeroPriceMovingBackfillHelper.ApplyMovingAveragePriceFields(
                        row,
                        source.SourceComponentCode,
                        source.ValuationPeriod,
                        source.MovingPrice,
                        source.PriceUnit,
                        source.CurrencyCode,
                        forceOverwrite: true))
                {
                    unchanged = checked(unchanged + 1);
                    continue;
                }
                var delta = ComputeMovingPriceLineCostDelta(row, oldPrice, oldUnit);
                if (delta != 0m && !string.IsNullOrWhiteSpace(row.ProductCode))
                {
                    lineDeltas.Add(new MovingPriceLineDelta
                    {
                        ProductCode = row.ProductCode.Trim(),
                        ComponentKey = TaktBomMaterialCostItemLineCostHelper.BuildComponentKey(row),
                        CostingDate = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(row.CostingDate),
                        IsDeleted = row.IsDeleted,
                        Id = row.Id,
                        Delta = delta,
                        CurrencyCode = row.MovingPriceCurrencyCode?.Trim() ?? string.Empty,
                    });
                }
                rowsToUpdate.Add(row);
                updated = checked(updated + 1);
                anyPriceUpdated = true;
            }
            if (rowsToUpdate.Count > 0)
            {
                const int updateChunkSize = 500;
                for (var offset = 0; offset < rowsToUpdate.Count; offset = checked(offset + updateChunkSize))
                {
                    var chunk = rowsToUpdate.Skip(offset).Take(updateChunkSize).ToList();
                    await _bomMaterialCostItemRepository.UpdateRangeAsync(chunk, yearTable);
                }
            }
            if (!anyPriceUpdated)
            {
                continue;
            }
            componentProcessed = checked(componentProcessed + 1);
            RegisterAffectedProductsFromComponentItems(
                items,
                componentCode,
                source,
                affectedProductSources);
        }

        var productDeltas = AggregateProductMonthlyCostDeltas(lineDeltas);

        // ----------------------------------------
        // 步骤2+3：旧产品月成本 + 差额 → 机种月平均
        // ----------------------------------------
        var productCostUpdated = 0;
        var modelAverageUpdated = 0;
        if (affectedProductSources.Count > 0)
        {
            (productCostUpdated, modelAverageUpdated) = await RefreshHeadersAfterMovingBackfillAsync(
                plantCode,
                costingPeriod,
                affectedProductSources,
                productDeltas);
        }

        return new TaktBomMaterialZeroPriceMovingBackfillResultDto
        {
            ScannedRowCount = scanned,
            UpdatedRowCount = updated,
            UnchangedRowCount = unchanged,
            SkippedNoPriceCount = 0,
            ComponentProcessedCount = componentProcessed,
            SourceComponentCode = firstSource?.SourceComponentCode,
            ValuationPeriod = firstSource?.ValuationPeriod,
            PriceInfo = firstSource == null
                ? null
                : TaktBomMaterialZeroPriceMovingBackfillHelper.FormatMovingPriceInfo(
                    firstSource.ValuationPeriod,
                    firstSource.MovingPrice,
                    firstSource.PriceUnit,
                    firstSource.CurrencyCode),
            ProductMonthlyCostUpdatedCount = productCostUpdated,
            ModelMonthlyAverageUpdatedCount = modelAverageUpdated,
            ProcessedMonth = costingPeriod,
        };
    }

    /// <summary>
    /// 步骤2：ProductMonthlyCalculation = 旧值 + 本次回填行成本差额；步骤3：刷机种月均（工厂+日期+物料类型+机种）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="affectedProductSources">产品 → 回填来源组件与移动价源</param>
    /// <param name="productDeltas">产品 → 本次回填成本差额合计</param>
    /// <returns>产品月成本更新行数、机种月成本更新行数</returns>
    private async Task<(int ProductCostUpdated, int ModelAverageUpdated)> RefreshHeadersAfterMovingBackfillAsync(
        string plantCode,
        string costingPeriod,
        IReadOnlyDictionary<string, List<(string Component, SuggestedMovingSource Source)>> affectedProductSources,
        IReadOnlyDictionary<string, (decimal Delta, string? CurrencyCode, DateTime? LatestCostingDate)> productDeltas)
    {
        if (affectedProductSources.Count == 0)
        {
            return (0, 0);
        }
        var affectedProductCodes = affectedProductSources.Keys.ToList();
        var periodHeaders = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.CostingPeriod == costingPeriod,
            asTableName: null,
            includeSoftDeleted: true);
        var productCostUpdated = 0;
        var touchedModelGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var modelSourceByGroup = new Dictionary<string, (string Component, SuggestedMovingSource Source)>(
            StringComparer.OrdinalIgnoreCase);
        var headersToUpdate = new List<TaktBomMaterialCost>();
        foreach (var productCode in affectedProductCodes)
        {
            var product = productCode.Trim();
            if (string.IsNullOrWhiteSpace(product)
                || !affectedProductSources.TryGetValue(product, out var sources)
                || sources.Count == 0)
            {
                continue;
            }
            productDeltas.TryGetValue(product, out var deltaInfo);
            var matchedHeaders = periodHeaders
                .Where(h => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(h.ProductCode, product))
                .ToList();
            foreach (var header in matchedHeaders)
            {
                var currentCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(header.ProductMonthlyCalculation);
                var nextCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(currentCost + deltaInfo.Delta);
                var writeCurrency = string.IsNullOrWhiteSpace(deltaInfo.CurrencyCode)
                    ? header.CurrencyCode
                    : deltaInfo.CurrencyCode.Trim();
                var writeDate = deltaInfo.LatestCostingDate ?? header.CostingDate;
                var materialType = string.IsNullOrWhiteSpace(header.MaterialType)
                    ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
                    : header.MaterialType.Trim();
                var modelCode = header.ModelCode?.Trim() ?? string.Empty;
                if (currentCost != nextCost
                    || !string.Equals(
                        header.CurrencyCode?.Trim() ?? string.Empty,
                        writeCurrency?.Trim() ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase)
                    || header.CostingDate != writeDate)
                {
                    foreach (var (componentCode, source) in sources)
                    {
                        TaktBomMaterialZeroPriceMovingBackfillHelper.ApplyHeaderProductCostMovingBackfillHistory(
                            header,
                            componentCode,
                            source.SourceComponentCode,
                            source.ValuationPeriod,
                            source.MovingPrice,
                            source.PriceUnit,
                            source.CurrencyCode,
                            currentCost,
                            nextCost);
                    }
                }
                header.ProductMonthlyCalculation = nextCost;
                header.CurrencyCode = writeCurrency ?? string.Empty;
                header.CostingDate = writeDate;
                header.CostingPeriod = costingPeriod;
                headersToUpdate.Add(header);
                productCostUpdated = checked(productCostUpdated + 1);
                if (!string.IsNullOrWhiteSpace(modelCode))
                {
                    var groupKey = BuildModelAverageGroupKey(materialType, modelCode);
                    if (!string.IsNullOrWhiteSpace(groupKey))
                    {
                        touchedModelGroups.Add(groupKey);
                        modelSourceByGroup[groupKey] = sources[0];
                    }
                }
            }
        }
        if (headersToUpdate.Count > 0)
        {
            await _bomMaterialCostRepository.UpdateRangeAsync(headersToUpdate);
        }
        var modelAverageUpdated = 0;
        foreach (var groupKey in touchedModelGroups.OrderBy(k => k, StringComparer.OrdinalIgnoreCase))
        {
            if (!modelSourceByGroup.TryGetValue(groupKey, out var sourcePair))
            {
                continue;
            }
            var parts = groupKey.Split('|', 2);
            if (parts.Length != 2)
            {
                continue;
            }
            modelAverageUpdated = checked(
                modelAverageUpdated
                + await RefreshModelMonthlyAverageForPeriodAsync(
                    plantCode,
                    parts[0],
                    parts[1],
                    costingPeriod,
                    sourcePair.Component,
                    sourcePair.Source));
        }
        return (productCostUpdated, modelAverageUpdated);
    }

    /// <summary>
    /// 单行回填前后成本差额（仅 X+F 且用量&gt;0.001；行金额=数量×(价÷单位)）
    /// </summary>
    /// <param name="row">已写新价的明细行</param>
    /// <param name="oldPrice">回填前移动价</param>
    /// <param name="oldUnit">回填前价格单位</param>
    /// <returns>新行成本 − 旧行成本</returns>
    private static decimal ComputeMovingPriceLineCostDelta(
        TaktBomMaterialCostItem row,
        decimal oldPrice,
        int oldUnit)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!TaktBomMaterialCostItemLineCostHelper.CountsTowardBomMaterialCostItem(row)
            || row.ComponentQuantity <= TaktBomMaterialCostItemLineCostHelper.MinParticipatingComponentQuantity)
        {
            return 0m;
        }
        var unitOld = oldUnit <= 0 ? 1 : oldUnit;
        var unitNew = TaktBomMaterialCostItemLineCostHelper.ResolveMovingPriceUnit(row);
        var oldLine = oldPrice == 0m
            ? 0m
            : TaktBomMaterialCostItemLineCostHelper.RoundCost(row.ComponentQuantity * (oldPrice / unitOld));
        var newLine = row.MovingAveragePrice == 0m
            ? 0m
            : TaktBomMaterialCostItemLineCostHelper.RoundCost(
                row.ComponentQuantity * (row.MovingAveragePrice / unitNew));
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(newLine - oldLine);
    }

    /// <summary>
    /// 按产品汇总差额：同产品取最后核算日，同组件键优先未软删再取最大 Id，避免重复加计
    /// </summary>
    /// <param name="lineDeltas">行差额</param>
    /// <returns>产品 → (差额合计, 币种, 最后核算日)</returns>
    private static Dictionary<string, (decimal Delta, string? CurrencyCode, DateTime? LatestCostingDate)> AggregateProductMonthlyCostDeltas(
        IReadOnlyList<MovingPriceLineDelta> lineDeltas)
    {
        ArgumentNullException.ThrowIfNull(lineDeltas);
        var result = new Dictionary<string, (decimal Delta, string? CurrencyCode, DateTime? LatestCostingDate)>(
            StringComparer.OrdinalIgnoreCase);
        foreach (var productGroup in lineDeltas.GroupBy(d => d.ProductCode, StringComparer.OrdinalIgnoreCase))
        {
            var product = productGroup.Key;
            if (string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            var latestDay = productGroup.Max(d => d.CostingDate);
            var dayRows = productGroup.Where(d => d.CostingDate == latestDay).ToList();
            var delta = dayRows
                .GroupBy(d => d.ComponentKey, StringComparer.Ordinal)
                .Select(g => g.OrderBy(x => x.IsDeleted).ThenByDescending(x => x.Id).First())
                .Sum(x => x.Delta);
            var currency = dayRows
                .Select(x => x.CurrencyCode)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
            result[product] = (
                TaktBomMaterialCostItemLineCostHelper.RoundCost(delta),
                currency,
                latestDay);
        }
        return result;
    }

    /// <summary>
    /// 步骤3：刷新同「工厂 + 核算月(日期) + 物料类型 + 机种」的机种月平均成本（主表 ExtField 记履历）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <param name="periodKey">核算月</param>
    /// <param name="componentCode">回填组件</param>
    /// <param name="source">移动价源</param>
    /// <returns>更新行数</returns>
    private async Task<int> RefreshModelMonthlyAverageForPeriodAsync(
        string plantCode,
        string materialType,
        string modelCode,
        string periodKey,
        string componentCode,
        SuggestedMovingSource source)
    {
        if (string.IsNullOrWhiteSpace(modelCode) || string.IsNullOrWhiteSpace(materialType))
        {
            return 0;
        }
        ArgumentNullException.ThrowIfNull(source);
        var mt = materialType.Trim();
        var model = modelCode.Trim();
        var plant = plantCode.Trim();
        // 工厂 + 日期 + 物料类型（机种在内存 Trim 比对，兼容空类型对齐 FERT）
        var headers = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.CostingPeriod == periodKey,
            asTableName: null,
            includeSoftDeleted: true);
        var matched = headers
            .Where(h =>
                string.Equals((h.ModelCode ?? string.Empty).Trim(), model, StringComparison.OrdinalIgnoreCase)
                && MaterialTypeMatchesModelAverageGroup(h.MaterialType, mt))
            .ToList();
        return await ApplyModelMonthlyAverageAndSaveAsync(matched, componentCode, source);
    }

    /// <summary>
    /// 按「工厂+日期+物料类型+机种」下各产品月成本算术平均写入机种月成本并保存；变更时追加 ExtField._bk.mp 履历
    /// </summary>
    /// <param name="headers">同工厂+日期+物料类型+机种主表行</param>
    /// <param name="componentCode">回填组件</param>
    /// <param name="source">移动价源</param>
    /// <returns>更新行数</returns>
    private async Task<int> ApplyModelMonthlyAverageAndSaveAsync(
        List<TaktBomMaterialCost> headers,
        string componentCode,
        SuggestedMovingSource source)
    {
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(source);
        if (headers.Count == 0)
        {
            return 0;
        }
        // 按产品去重取产品月计算（>0）再算术平均；写回该组全部主表行
        var costs = headers
            .Where(h => h.ProductMonthlyCalculation > 0m)
            .GroupBy(
                h => (h.ProductCode ?? string.Empty).Trim(),
                StringComparer.OrdinalIgnoreCase)
            .Where(g => !string.IsNullOrWhiteSpace(g.Key))
            .Select(g => g
                .OrderBy(h => h.IsDeleted)
                .ThenByDescending(h => h.Id)
                .First()
                .ProductMonthlyCalculation)
            .ToList();
        var average = TaktBomMaterialCostItemModelEnrichmentHelper.ComputeModelMonthlyAverageFromProductCosts(costs);
        var toSave = new List<TaktBomMaterialCost>();
        foreach (var header in headers)
        {
            var current = TaktBomMaterialCostItemLineCostHelper.RoundCost(header.ModelMonthlyAverageCost);
            if (current == average)
            {
                continue;
            }
            TaktBomMaterialZeroPriceMovingBackfillHelper.ApplyHeaderModelAverageMovingBackfillHistory(
                header,
                componentCode,
                source.SourceComponentCode,
                source.ValuationPeriod,
                source.MovingPrice,
                source.PriceUnit,
                source.CurrencyCode,
                current,
                average);
            header.ModelMonthlyAverageCost = average;
            toSave.Add(header);
        }
        if (toSave.Count == 0)
        {
            return 0;
        }
        await _bomMaterialCostRepository.UpdateRangeAsync(toSave);
        return toSave.Count;
    }

    /// <summary>
    /// 机种月成本分组键（物料类型|机种；类型空时用 FERT）
    /// </summary>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <returns>分组键；机种空则空串</returns>
    private static string BuildModelAverageGroupKey(string? materialType, string? modelCode)
    {
        var model = modelCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(model))
        {
            return string.Empty;
        }
        var mt = string.IsNullOrWhiteSpace(materialType)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : materialType.Trim();
        return $"{mt}|{model}";
    }

    /// <summary>
    /// 空物料类型是否与分组期望类型视为同组（空对齐 FERT）
    /// </summary>
    /// <param name="stored">主表已存类型</param>
    /// <param name="expected">分组期望类型</param>
    /// <returns>是否同组</returns>
    private static bool MaterialTypeMatchesModelAverageGroup(string? stored, string expected)
    {
        var actual = stored?.Trim() ?? string.Empty;
        if (string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return string.IsNullOrEmpty(actual)
            && string.Equals(
                expected,
                TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode,
                StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 步骤2 登记：将步骤1「工厂+日期+组件」明细中出现的全部产品记入待刷新产品月成本
    /// </summary>
    /// <param name="componentItems">该组件在工厂+核算月下的明细</param>
    /// <param name="componentCode">组件编码</param>
    /// <param name="source">移动价源</param>
    /// <param name="affectedProductSources">产品 → 来源列表</param>
    private static void RegisterAffectedProductsFromComponentItems(
        IEnumerable<TaktBomMaterialCostItem> componentItems,
        string componentCode,
        SuggestedMovingSource source,
        IDictionary<string, List<(string Component, SuggestedMovingSource Source)>> affectedProductSources)
    {
        ArgumentNullException.ThrowIfNull(componentItems);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(affectedProductSources);
        var component = (componentCode ?? string.Empty).Trim();
        foreach (var row in componentItems)
        {
            if (string.IsNullOrWhiteSpace(row.ProductCode))
            {
                continue;
            }
            var product = row.ProductCode.Trim();
            if (!affectedProductSources.TryGetValue(product, out var list))
            {
                list = new List<(string Component, SuggestedMovingSource Source)>();
                affectedProductSources[product] = list;
            }
            if (!list.Exists(x =>
                    string.Equals(x.Component, component, StringComparison.OrdinalIgnoreCase)))
            {
                list.Add((component, source));
            }
        }
    }

    /// <summary>
    /// 加载产品 BOM 明细全量（含非 X+F，供产品月成本快照/PCB SECT 排除）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（同月初则整月）</param>
    /// <param name="includeDeleted">true=全量不区分 IsDeleted（回填后刷新主表）</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsRawForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd,
        bool includeDeleted = false)
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
            if (!includeDeleted)
            {
                exp = exp.And(x => x.IsDeleted == 0);
            }
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
            // includeDeleted 须传仓储 includeSoftDeleted：ApplyReadScope 默认过滤 IsDeleted=0，仅靠表达式无法含软删
            var part = await _bomMaterialCostItemRepository.GetListAsync(
                exp.ToExpression(),
                yearTable,
                includeSoftDeleted: includeDeleted);
            allItems.AddRange(part);
        }
        return allItems;
    }

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
    /// 取工厂+核算月下 FERT 主表产品编码（机种可选；不跑零价合并）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelFilters">机种过滤（空=全部）</param>
    /// <param name="costingStart">核算日起</param>
    /// <param name="costingEnd">核算日止</param>
    /// <param name="includeDeleted">是否含软删主表</param>
    /// <returns>去重产品编码</returns>
    private async Task<List<string>> ResolveFertProductCodesAsync(
        string plantCode,
        IReadOnlyList<string> modelFilters,
        DateTime costingStart,
        DateTime costingEnd,
        bool includeDeleted = false)
    {
        var fertType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.MaterialType == fertType
            && x.CostingDate >= costingStart
            && x.CostingDate <= costingEnd);
        if (!includeDeleted)
        {
            exp = exp.And(x => x.IsDeleted == 0);
        }
        List<TaktBomMaterialCost> headers;
        if (modelFilters.Count > 0)
        {
            var headersAll = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
            var modelSet = new HashSet<string>(modelFilters, StringComparer.OrdinalIgnoreCase);
            headers = headersAll
                .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                    && modelSet.Contains(h.ModelCode.Trim()))
                .ToList();
        }
        else
        {
            headers = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
        }
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .Select(h => h.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 规范化核算月（须单月）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>月初、月末、yyyy-MM</returns>
    private static (DateTime Start, DateTime End, string Period) PrepareCostingMonth(
        TaktBomMaterialZeroPriceQueryDto queryDto)
    {
        if (!queryDto.CostingDateStart.HasValue || !queryDto.CostingDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择核算月份");
        }
        var startMonth = new DateTime(
            queryDto.CostingDateStart.Value.Year,
            queryDto.CostingDateStart.Value.Month,
            1);
        var endMonth = new DateTime(
            queryDto.CostingDateEnd.Value.Year,
            queryDto.CostingDateEnd.Value.Month,
            1);
        if (startMonth != endMonth)
        {
            throw new TaktBusinessException("零价格清单仅支持单个核算月份");
        }
        var lastDay = DateTime.DaysInMonth(startMonth.Year, startMonth.Month);
        var end = new DateTime(startMonth.Year, startMonth.Month, lastDay, 23, 59, 59, 999);
        return (startMonth, end, $"{startMonth.Year:D4}-{startMonth.Month:D2}");
    }

    /// <summary>
    /// 构建零价合并全量行（未分页；主表 MaterialType=FERT；列表默认排除已删，回填发现组件时可含已删）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelFilters">机种过滤（空=全部）</param>
    /// <param name="costingStart">核算日起</param>
    /// <param name="costingEnd">核算日止</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="includeDeleted">true 时主表/明细含已软删（批量回填发现组件）</param>
    /// <returns>产品码、产品→机种、合并行</returns>
    private async Task<(
        List<string> ProductCodes,
        Dictionary<string, HashSet<string>> ProductToModels,
        List<TaktBomMaterialZeroPriceDto> Rows)> BuildMergedRowsAsync(
        string plantCode,
        IReadOnlyList<string> modelFilters,
        DateTime costingStart,
        DateTime costingEnd,
        string costingPeriod,
        bool includeDeleted = false)
    {
        var fertType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.MaterialType == fertType
            && x.CostingDate >= costingStart
            && x.CostingDate <= costingEnd);
        if (!includeDeleted)
        {
            exp = exp.And(x => x.IsDeleted == 0);
        }
        List<TaktBomMaterialCost> headers;
        if (modelFilters.Count > 0)
        {
            // 机种在内存 Trim 比对（单选/多选一致），避免库内尾随空格导致漏产品
            // includeDeleted 须传仓储 includeSoftDeleted（ApplyReadScope 默认过滤软删）
            var headersAll = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
            var modelSet = new HashSet<string>(modelFilters, StringComparer.OrdinalIgnoreCase);
            headers = headersAll
                .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                    && modelSet.Contains(h.ModelCode.Trim()))
                .ToList();
        }
        else
        {
            headers = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
        }
        var productToModels = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers)
        {
            if (string.IsNullOrWhiteSpace(header.ProductCode))
            {
                continue;
            }
            var product = header.ProductCode.Trim();
            if (!productToModels.TryGetValue(product, out var models))
            {
                models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                productToModels[product] = models;
            }
            if (!string.IsNullOrWhiteSpace(header.ModelCode))
            {
                models.Add(header.ModelCode.Trim());
            }
        }
        var productCodes = productToModels.Keys
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        if (productCodes.Count == 0)
        {
            return (productCodes, productToModels, new List<TaktBomMaterialZeroPriceDto>());
        }

        var items = await LoadBomCostItemsForProductsAsync(
            plantCode,
            productCodes,
            costingStart,
            costingStart,
            includeDeleted);
        // 同一 ComponentCode 多位置须按行判定：任一笔生产相关=X、PCB SECT 标识为空、采购类型=F、用量>0.001、价0 即入清单
        var zeroItems = items
            .Where(TaktBomMaterialCostItemLineCostHelper.QualifiesAsZeroPriceListLine)
            .ToList();
        if (zeroItems.Count >= MaxRowLoad)
        {
            ThrowBusinessException($"零价 BOM 明细行为 {zeroItems.Count}，达到上限 {MaxRowLoad}，请缩小机种范围");
        }

        var map = new Dictionary<string, MergedBomComponent>(StringComparer.OrdinalIgnoreCase);
        var modelsByComponent = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in zeroItems)
        {
            if (string.IsNullOrWhiteSpace(item.ComponentCode))
            {
                continue;
            }
            var code = item.ComponentCode.Trim();
            if (!map.TryGetValue(code, out var merged))
            {
                merged = new MergedBomComponent
                {
                    ComponentCode = code,
                    ComponentDescription = item.ComponentDescription?.Trim() ?? string.Empty,
                };
                map[code] = merged;
                modelsByComponent[code] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            else if (string.IsNullOrWhiteSpace(merged.ComponentDescription)
                && !string.IsNullOrWhiteSpace(item.ComponentDescription))
            {
                merged.ComponentDescription = item.ComponentDescription.Trim();
            }
            if (!string.IsNullOrWhiteSpace(item.ProductCode))
            {
                var product = item.ProductCode.Trim();
                merged.ProductCodes.Add(product);
                if (productToModels.TryGetValue(product, out var models))
                {
                    foreach (var m in models)
                    {
                        modelsByComponent[code].Add(m);
                    }
                }
            }
        }

        var allRows = map.Values
            .Select(c => new TaktBomMaterialZeroPriceDto
            {
                PlantCode = plantCode,
                ModelCode = modelsByComponent.TryGetValue(c.ComponentCode, out var ms)
                    ? string.Join(",", ms.OrderBy(m => m, StringComparer.OrdinalIgnoreCase))
                    : string.Empty,
                ComponentCode = c.ComponentCode,
                ComponentDescription = c.ComponentDescription,
                ProductCodes = string.Join(",", c.ProductCodes.OrderBy(p => p, StringComparer.Ordinal)),
                ProductCount = c.ProductCodes.Count,
                MovingAveragePrice = 0m,
                CostingPeriod = costingPeriod,
            })
            .OrderBy(r => r.ComponentCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.ModelCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return (productCodes, productToModels, allRows);
    }

    /// <summary>
    /// 为零价组件填充建议代替：末字母依次前推；优先取核算月（期间）相同 ValuationPeriod 的移动价，否则取核算月及以前最近有价月
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="rows">零价合并行</param>
    private async Task FillSuggestedRevisionsAsync(
        string plantCode,
        string costingPeriod,
        List<TaktBomMaterialZeroPriceDto> rows)
    {
        if (rows.Count == 0 || string.IsNullOrWhiteSpace(costingPeriod))
        {
            return;
        }

        var periodKey = costingPeriod.Trim();
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var costingMonth))
        {
            return;
        }

        var lookbackStart = costingMonth.AddMonths(-24).ToString("yyyy-MM");

        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var candidatesByComponent = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var candidates = EnumeratePreviousLetterRevisions(row.ComponentCode).ToList();
            candidatesByComponent[row.ComponentCode] = candidates;
            foreach (var code in candidates)
            {
                foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(code))
                {
                    candidateSet.Add(variant);
                }
            }
        }
        if (candidateSet.Count == 0)
        {
            return;
        }

        var samePeriodByMaterial = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var priceByMaterial = new Dictionary<string, (string Period, decimal Price)>(StringComparer.OrdinalIgnoreCase);
        const int chunkSize = 200;
        var lookupCodes = candidateSet.ToList();
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktMaterialMovingPrice, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode)
                && x.MovingPrice > 0;
            var remaining = MaxRowLoad - Math.Max(samePeriodByMaterial.Count, priceByMaterial.Count);
            if (remaining <= 0)
            {
                break;
            }
            var part = await GetMovingPriceListForRangeAsync(
                predicate,
                lookbackStart,
                periodKey,
                remaining);
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
                var ym = (price.ValuationPeriod ?? string.Empty).Trim().Replace('/', '-');
                if (ym.Length >= 7 && ym[4] == '-')
                {
                    ym = ym[..7];
                }
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
                        if (!samePeriodByMaterial.TryGetValue(key, out var existingSame)
                            || unitPrice > existingSame)
                        {
                            samePeriodByMaterial[key] = unitPrice;
                        }
                    }
                    if (!priceByMaterial.TryGetValue(key, out var existing)
                        || string.CompareOrdinal(ym, existing.Period) > 0
                        || (string.Equals(ym, existing.Period, StringComparison.Ordinal)
                            && unitPrice > existing.Price))
                    {
                        priceByMaterial[key] = (ym, unitPrice);
                    }
                }
            }
        }

        foreach (var row in rows)
        {
            if (!candidatesByComponent.TryGetValue(row.ComponentCode, out var list))
            {
                continue;
            }
            foreach (var candidate in list)
            {
                decimal? suggestedPrice = null;
                foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
                {
                    if (samePeriodByMaterial.TryGetValue(variant, out var exact) && exact > 0m)
                    {
                        suggestedPrice = exact;
                        break;
                    }
                }
                if (!suggestedPrice.HasValue)
                {
                    foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
                    {
                        if (!priceByMaterial.TryGetValue(variant, out var hit))
                        {
                            continue;
                        }
                        suggestedPrice = hit.Price;
                        break;
                    }
                }
                if (!suggestedPrice.HasValue)
                {
                    continue;
                }
                row.SuggestedComponentCode = candidate;
                row.SuggestedMovingPrice = suggestedPrice.Value;
                break;
            }
        }
    }

    /// <summary>
    /// 按工厂+组件+核算月加载 BOM 明细（批量/手工回填：includeDeleted=true 时全量，不区分 IsDeleted）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="componentCode">组件编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（同月初则整月）</param>
    /// <param name="includeDeleted">true=全量（须仓储 includeSoftDeleted，否则读隔离仍滤软删）</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsByComponentAsync(
        string plantCode,
        string componentCode,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd,
        bool includeDeleted = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(componentCode);
        var component = componentCode.Trim();
        var componentLookup = TaktBomMaterialCostItemLineCostHelper
            .ExpandProductCodeLookupVariants(component)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && componentLookup.Contains(x.ComponentCode));
        if (!includeDeleted)
        {
            exp = exp.And(x => x.IsDeleted == 0);
        }
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
        // includeDeleted 须传仓储 includeSoftDeleted：ApplyReadScope 默认过滤 IsDeleted=0，仅靠表达式无法含软删
        var part = await _bomMaterialCostItemRepository.GetListAsync(
            exp.ToExpression(),
            yearTable,
            includeSoftDeleted: includeDeleted);
        // 手工更新：同组件全量写入，不再按 X+F / 零价清单收窄
        return part.ToList();
    }

    /// <summary>
    /// 加载产品 BOM 明细：先拉全量展开，再 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F）。
    /// 分块仅用于查询，Filter 在合并后整表执行一次，避免同产品多码写法拆树导致误排除。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（同月初则整月）</param>
    /// <param name="includeDeleted">true 时含已软删明细（回填移动价）</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd,
        bool includeDeleted = false)
    {
        var raw = await LoadBomCostItemsRawForProductsAsync(
            plantCode,
            productCodes,
            costingMonthStart,
            costingMonthEnd,
            includeDeleted);
        return TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(raw).ToList();
    }

    /// <summary>
    /// 按年分表查询移动价格
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
    /// 解析 BOM 成本明细物理表
    /// </summary>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
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
    /// 逆推建议代替源：末字母前推；优先核算月同 ValuationPeriod，否则核算月及以前最近有价月；返回移动价原值/单位/币种
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="componentCode">零价组件</param>
    /// <returns>源价；无则 null</returns>
    private async Task<SuggestedMovingSource?> ResolveSuggestedMovingSourceAsync(
        string plantCode,
        string costingPeriod,
        string componentCode)
    {
        var periodKey = costingPeriod.Trim();
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var costingMonth))
        {
            return null;
        }
        var candidates = EnumeratePreviousLetterRevisions(componentCode).ToList();
        if (candidates.Count == 0)
        {
            return null;
        }
        var lookbackStart = costingMonth.AddMonths(-24).ToString("yyyy-MM");
        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in candidates)
        {
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(code))
            {
                candidateSet.Add(variant);
            }
        }
        // key → (源码、期间、原价、单位、币种)；同期间优先，否则期间最新
        var samePeriod = new Dictionary<string, SuggestedMovingSource>(StringComparer.OrdinalIgnoreCase);
        var anyPeriod = new Dictionary<string, SuggestedMovingSource>(StringComparer.OrdinalIgnoreCase);
        const int chunkSize = 200;
        var lookupCodes = candidateSet.ToList();
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktMaterialMovingPrice, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode)
                && x.MovingPrice > 0;
            var remaining = MaxRowLoad - Math.Max(samePeriod.Count, anyPeriod.Count);
            if (remaining <= 0)
            {
                break;
            }
            var part = await GetMovingPriceListForRangeAsync(
                predicate,
                lookbackStart,
                periodKey,
                remaining);
            foreach (var price in part)
            {
                if (string.IsNullOrWhiteSpace(price.MaterialCode) || price.MovingPrice <= 0m)
                {
                    continue;
                }
                var ym = TaktBomMaterialZeroPriceMovingBackfillHelper.NormalizeValuationPeriod(price.ValuationPeriod);
                if (ym.Length == 0
                    || string.CompareOrdinal(ym, lookbackStart) < 0
                    || string.CompareOrdinal(ym, periodKey) > 0)
                {
                    continue;
                }
                var unit = price.PriceUnit <= 0 ? 1 : price.PriceUnit;
                var currency = price.CurrencyCode?.Trim() ?? string.Empty;
                var material = price.MaterialCode.Trim();
                var hit = new SuggestedMovingSource
                {
                    SourceComponentCode = material,
                    ValuationPeriod = ym,
                    MovingPrice = price.MovingPrice,
                    PriceUnit = unit,
                    CurrencyCode = currency,
                };
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(material))
                {
                    if (string.Equals(ym, periodKey, StringComparison.Ordinal))
                    {
                        if (!samePeriod.TryGetValue(key, out var existingSame)
                            || price.MovingPrice > existingSame.MovingPrice)
                        {
                            samePeriod[key] = hit;
                        }
                    }
                    if (!anyPeriod.TryGetValue(key, out var existing)
                        || string.CompareOrdinal(ym, existing.ValuationPeriod) > 0
                        || (string.Equals(ym, existing.ValuationPeriod, StringComparison.Ordinal)
                            && price.MovingPrice > existing.MovingPrice))
                    {
                        anyPeriod[key] = hit;
                    }
                }
            }
        }
        foreach (var candidate in candidates)
        {
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
            {
                if (samePeriod.TryGetValue(variant, out var exact))
                {
                    exact.SourceComponentCode = candidate;
                    return exact;
                }
            }
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
            {
                if (!anyPeriod.TryGetValue(variant, out var hit))
                {
                    continue;
                }
                hit.SourceComponentCode = candidate;
                return hit;
            }
        }
        return null;
    }

    /// <summary>
    /// 枚举组件编码末字母前推版本
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

    /// <summary>
    /// 合并组件中间结果
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
    /// 建议代替移动价源（原值，非÷单位）
    /// </summary>
    private sealed class SuggestedMovingSource
    {
        /// <summary>
        /// 建议代替组件编码
        /// </summary>
        public string SourceComponentCode { get; set; } = string.Empty;

        /// <summary>
        /// 评估期间 yyyy-MM
        /// </summary>
        public string ValuationPeriod { get; set; } = string.Empty;

        /// <summary>
        /// 移动价格原值
        /// </summary>
        public decimal MovingPrice { get; set; }

        /// <summary>
        /// 价格单位
        /// </summary>
        public int PriceUnit { get; set; } = 1;

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 步骤1 写价后的单行成本差额（供步骤2 增量加到产品月成本）
    /// </summary>
    private sealed class MovingPriceLineDelta
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 组件业务键
        /// </summary>
        public string ComponentKey { get; set; } = string.Empty;

        /// <summary>
        /// 核算日
        /// </summary>
        public DateTime CostingDate { get; set; }

        /// <summary>
        /// 是否软删
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 明细 Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 行成本差额（新−旧）
        /// </summary>
        public decimal Delta { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
