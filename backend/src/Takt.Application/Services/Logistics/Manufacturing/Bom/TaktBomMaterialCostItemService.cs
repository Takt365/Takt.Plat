// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemService.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using OfficeOpenXml;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM物料成本明细应用服务
/// </summary>
public class TaktBomMaterialCostItemService : TaktServiceBase, ITaktBomMaterialCostItemService
{
    private const int MaxCascadeSelectOptions = 500;

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
    private readonly ITaktBomMaterialCostService _bomMaterialCostService;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM物料成本汇总仓储（转置/涨跌分析数据源）</param>
    /// <param name="materialPlantRepository">工厂物料仓储（成本合计仅 FERT）</param>
    /// <param name="materialMovingPriceRepository">移动价格仓储（组件单价期间转置）</param>
    /// <param name="calendarRepository">工厂日历仓储（第 N 工作日判定）</param>
    /// <param name="bomMaterialCostService">BOM物料成本主表服务（明细落库后回算）</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostItemService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktCompanyRepository<TaktCalendar> calendarRepository,
        ITaktBomMaterialCostService bomMaterialCostService,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
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
        _bomMaterialCostService = bomMaterialCostService;
        _modelDestinationRepository = modelDestinationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取BOM物料成本明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBomMaterialCostItemDto>> GetBomMaterialCostItemListAsync(TaktBomMaterialCostItemQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        await ApplyModelCodeProductScopeAsync(queryDto);
        var predicate = QueryExpression(queryDto);
        string? yearTable;
        try
        {
            yearTable = await ResolveBomItemPhysicalTableAsync(
                TaktYearShardTableHelper.RequireSingleYear(queryDto.CostingDateStart, queryDto.CostingDateEnd));
        }
        catch (ArgumentException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
        var (data, total) = await _bomMaterialCostItemRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.CreatedAt,
            true,
            yearTable);
        return TaktPagedResult<TaktBomMaterialCostItemDto>.Create(
            data.Adapt<List<TaktBomMaterialCostItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostItemDto?> GetBomMaterialCostItemByIdAsync(long id)
    {
        var (entity, _) = await FindBomItemByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBomMaterialCostItemDto>();
    }

    /// <summary>
    /// 获取BOM物料成本选项列表（产品编码去重，DictValue=ProductCode）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于缩小候选产品）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostItemOptionsAsync(string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var yearTable = await ResolveBomItemPhysicalTableAsync(DateTime.Now.Year);
        var list = await _bomMaterialCostItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (string.IsNullOrWhiteSpace(plantCode) || (x.PlantCode != null && x.PlantCode == plantCode)),
            x => x.ProductCode ?? string.Empty,
            false,
            yearTable);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ProductCode))
            .GroupBy(e => e.ProductCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.First();
                var description = first.ProductDescription?.Trim();
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
    /// 创建BOM物料成本明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostItemDto> CreateBomMaterialCostItemAsync(TaktBomMaterialCostItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktBomMaterialCostItem>();
        NormalizeBomMaterialCostItemCodes(entity);
        var yearTable = await ResolveBomItemPhysicalTableAsync(entity.CostingDate.Year);
        await EnsureBomItemUniqueAsync(entity, yearTable);
        entity = await _bomMaterialCostItemRepository.CreateAsync(entity, yearTable);
        await _bomMaterialCostService.SyncBomMaterialCostFromItemsAsync(
            entity.PlantCode, entity.ProductCode, entity.CostingDate);
        return await GetBomMaterialCostItemByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostItemDto>();
    }

    /// <summary>
    /// 更新BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostItemDto> UpdateBomMaterialCostItemAsync(long id, TaktBomMaterialCostItemUpdateDto dto)
    {
        var (entity, yearTable) = await FindBomItemByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM物料成本明细不存在");
        }
        var originalYear = entity.CostingDate.Year;
        dto.Adapt(entity);
        NormalizeBomMaterialCostItemCodes(entity);
        if (entity.CostingDate.Year != originalYear)
        {
            throw new TaktBusinessException("按年分表后不可跨年修改核算日期，请删除后重建");
        }
        await EnsureBomItemUniqueAsync(entity, yearTable, id);
        await _bomMaterialCostItemRepository.UpdateAsync(entity, yearTable);
        await _bomMaterialCostService.SyncBomMaterialCostFromItemsAsync(
            entity.PlantCode, entity.ProductCode, entity.CostingDate);
        return await GetBomMaterialCostItemByIdAsync(id) ?? throw new TaktBusinessException("BOM物料成本明细不存在");
    }

    /// <summary>
    /// 删除BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostItemByIdAsync(long id)
    {
        var (entity, yearTable) = await FindBomItemByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM物料成本明细不存在或已删除");
        }
        var plantCode = entity.PlantCode;
        var productCode = entity.ProductCode;
        var costingDate = entity.CostingDate;
        var deleted = await _bomMaterialCostItemRepository.DeleteAsync(id, yearTable);
        if (!deleted)
        {
            throw new TaktBusinessException("BOM物料成本明细不存在或已删除");
        }
        await _bomMaterialCostService.SyncBomMaterialCostFromItemsAsync(plantCode, productCode, costingDate);
    }

    /// <summary>
    /// 批量删除BOM物料成本明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBomMaterialCostItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBomMaterialCostItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBomMaterialCostItemTemplateDto>(
            sheetName ?? "BOM物料成本明细导入模板",
            fileName ?? "BOM物料成本明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入BOM物料成本明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBomMaterialCostItemImportDto>(fileStream, sheetName ?? "BOM物料成本明细导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var syncKeys = new List<(string PlantCode, string ProductCode, DateTime CostingDate)>();
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBomMaterialCostItem>();
                NormalizeBomMaterialCostItemCodes(entity);
                var importKey = string.Join(
                    "|",
                    entity.PlantCode,
                    entity.ProductCode,
                    entity.SequenceNo,
                    entity.BomLevel,
                    entity.BomItemNo,
                    entity.ComponentCode,
                    entity.ComponentQuantity.ToString("0.##########", System.Globalization.CultureInfo.InvariantCulture),
                    entity.BatchIndicator ?? string.Empty,
                    entity.ProductionRelated ?? string.Empty,
                    entity.PurchaseType,
                    entity.SpecialProcurementType ?? string.Empty,
                    entity.CostingDate.ToString("O", System.Globalization.CultureInfo.InvariantCulture));
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProductCode、SequenceNo、BomLevel、BomItemNo、ComponentCode、ComponentQuantity、BatchIndicator、ProductionRelated、PurchaseType、SpecialProcurementType、CostingDate）");
                }
                var yearTable = await ResolveBomItemPhysicalTableAsync(entity.CostingDate.Year);
                await EnsureBomItemUniqueAsync(entity, yearTable);
                await _bomMaterialCostItemRepository.CreateAsync(entity, yearTable);
                syncKeys.Add((entity.PlantCode, entity.ProductCode, entity.CostingDate));
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        if (syncKeys.Count > 0)
        {
            await _bomMaterialCostService.SyncBomMaterialCostFromItemsBatchAsync(syncKeys);
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出BOM物料成本明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemAsync(TaktBomMaterialCostItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        query ??= new TaktBomMaterialCostItemQueryDto();
        await ApplyModelCodeProductScopeAsync(query);
        var predicate = QueryExpression(query);
        var list = await GetBomItemListForRangeAsync(
            predicate,
            query.CostingDateStart,
            query.CostingDateEnd);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomMaterialCostItemExportDto>(),
                sheetName ?? "BOM物料成本明细数据",
                fileName ?? "BOM物料成本明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBomMaterialCostItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM物料成本明细数据",
            fileName ?? "BOM物料成本明细导出.xlsx");
    }

    /// <summary>
    /// 归一化产品/组件编码（18 位 SAP 数字物料码截断为后 10 位）
    /// </summary>
    /// <param name="entity">明细实体</param>
    private static void NormalizeBomMaterialCostItemCodes(TaktBomMaterialCostItem entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!string.IsNullOrWhiteSpace(entity.ProductCode))
        {
            entity.ProductCode = TaktStringHelper.NormalizeSapNumericMaterialCode(entity.ProductCode.Trim());
        }
        if (!string.IsNullOrWhiteSpace(entity.ComponentCode))
        {
            entity.ComponentCode = TaktStringHelper.NormalizeSapNumericMaterialCode(entity.ComponentCode.Trim());
        }
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建BOM物料成本明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBomMaterialCostItem, bool>> QueryExpression(TaktBomMaterialCostItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.SequenceNo != null && x.SequenceNo.Contains(keywords))
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords))
                || (x.BomLevel != null && x.BomLevel.Contains(keywords))
                || (x.BomItemNo != null && x.BomItemNo.Contains(keywords))
                || (x.ComponentCode != null && x.ComponentCode.Contains(keywords))
                || (x.ComponentDescription != null && x.ComponentDescription.Contains(keywords))
                || SqlFunc.ToString(x.ComponentQuantity).Contains(keywords)
                || (x.BatchIndicator != null && x.BatchIndicator.Contains(keywords))
                || (x.ProductionRelated != null && x.ProductionRelated.Contains(keywords))
                || (x.PurchaseType != null && x.PurchaseType.Contains(keywords))
                || (x.SpecialProcurementType != null && x.SpecialProcurementType.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || SqlFunc.ToString(x.MovingAveragePrice).Contains(keywords)
                || SqlFunc.ToString(x.MovingPriceUnit).Contains(keywords)
                || (x.MovingPriceCurrency != null && x.MovingPriceCurrency.Contains(keywords))
                || (x.PurchaseOrganization != null && x.PurchaseOrganization.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || SqlFunc.ToString(x.NetPurchasePrice).Contains(keywords)
                || SqlFunc.ToString(x.PurchasePriceUnit).Contains(keywords)
                || (x.PurchaseCurrency != null && x.PurchaseCurrency.Contains(keywords))
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

        if (queryDto?.ProductCodes != null && queryDto.ProductCodes.Count > 0)
        {
            var productCodes = queryDto.ProductCodes;
            exp = exp.And(x => x.ProductCode != null && productCodes.Contains(x.ProductCode));
        }
        else if (!string.IsNullOrEmpty(queryDto?.ProductCode))
        {
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(queryDto.ProductCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SequenceNo))
        {
            exp = exp.And(x => x.SequenceNo != null && x.SequenceNo.Contains(queryDto.SequenceNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductDescription))
        {
            exp = exp.And(x => x.ProductDescription != null && x.ProductDescription.Contains(queryDto.ProductDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.BomLevel))
        {
            exp = exp.And(x => x.BomLevel != null && x.BomLevel.Contains(queryDto.BomLevel));
        }

        if (!string.IsNullOrEmpty(queryDto?.BomItemNo))
        {
            exp = exp.And(x => x.BomItemNo != null && x.BomItemNo.Contains(queryDto.BomItemNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ComponentCode))
        {
            exp = exp.And(x => x.ComponentCode != null && x.ComponentCode.Contains(queryDto.ComponentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ComponentDescription))
        {
            exp = exp.And(x => x.ComponentDescription != null && x.ComponentDescription.Contains(queryDto.ComponentDescription));
        }

        if (queryDto?.ComponentQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ComponentQuantity == queryDto.ComponentQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchIndicator))
        {
            exp = exp.And(x => x.BatchIndicator != null && x.BatchIndicator.Contains(queryDto.BatchIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionRelated))
        {
            var productionRelated = queryDto.ProductionRelated.Trim();
            exp = exp.And(x => x.ProductionRelated == productionRelated);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseType))
        {
            var purchaseType = queryDto.PurchaseType.Trim();
            exp = exp.And(x => x.PurchaseType == purchaseType);
        }

        if (!string.IsNullOrEmpty(queryDto?.SpecialProcurementType))
        {
            exp = exp.And(x => x.SpecialProcurementType != null && x.SpecialProcurementType.Contains(queryDto.SpecialProcurementType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProfitCenterCode))
        {
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(queryDto.ProfitCenterCode));
        }

        if (queryDto?.MovingAveragePrice.HasValue == true)
        {
            exp = exp.And(x => x.MovingAveragePrice == queryDto.MovingAveragePrice);
        }

        if (queryDto?.MovingPriceUnit.HasValue == true)
        {
            exp = exp.And(x => x.MovingPriceUnit == queryDto.MovingPriceUnit);
        }

        if (!string.IsNullOrEmpty(queryDto?.MovingPriceCurrency))
        {
            exp = exp.And(x => x.MovingPriceCurrency != null && x.MovingPriceCurrency.Contains(queryDto.MovingPriceCurrency));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseOrganization))
        {
            exp = exp.And(x => x.PurchaseOrganization != null && x.PurchaseOrganization.Contains(queryDto.PurchaseOrganization));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseGroup))
        {
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(queryDto.PurchaseGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (queryDto?.NetPurchasePrice.HasValue == true)
        {
            exp = exp.And(x => x.NetPurchasePrice == queryDto.NetPurchasePrice);
        }

        if (queryDto?.PurchasePriceUnit.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePriceUnit == queryDto.PurchasePriceUnit);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseCurrency))
        {
            exp = exp.And(x => x.PurchaseCurrency != null && x.PurchaseCurrency.Contains(queryDto.PurchaseCurrency));
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

    /// <summary>
    /// 校验并规范化机种月平均重算查询（须单个核算月份）
    /// </summary>
    /// <param name="queryDto">原始查询</param>
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
        // 起止日先规范化（UTC→本地日），避免 7 月请求被解析成 6 月
        var startDay = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(normalized.CostingDateStart.Value);
        var endDay = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(normalized.CostingDateEnd.Value);
        var startMonth = new DateTime(startDay.Year, startDay.Month, 1);
        var endMonth = new DateTime(endDay.Year, endDay.Month, 1);
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
        var periodKey = prepared.ProcessedMonth;
        var filterModel = prepared.Query.ModelCode?.Trim();
        // 成本合计：明细按 工厂+核算期间+产品 分组合计 → Sync Upsert 主表；仅 MaterialType=FERT
        var itemRows = await GetBomItemListForRangeAsync(
            QueryExpression(prepared.Query),
            prepared.Query.CostingDateStart,
            prepared.Query.CostingDateEnd);
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
                var latest = g
                    .OrderByDescending(x => TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(x.CostingDate))
                    .First();
                var costingDate = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(latest.CostingDate);
                return (
                    PlantCode: latest.PlantCode!.Trim(),
                    ProductCode: latest.ProductCode!.Trim(),
                    CostingDate: costingDate,
                    Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(costingDate));
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
        var syncKeys = new List<(string PlantCode, string ProductCode, DateTime CostingDate)>();
        var skippedCount = 0;
        foreach (var key in groupedKeys)
        {
            if (!TaktBomMaterialCostItemLineCostHelper.IsFertPlantProduct(
                    materialPlants, key.PlantCode, key.ProductCode))
            {
                skippedCount++;
                continue;
            }
            if (!string.IsNullOrWhiteSpace(filterModel))
            {
                var resolvedModel = await GetBomMaterialCostItemModelCodeByProductAsync(
                    key.ProductCode,
                    key.PlantCode);
                if (!string.Equals(resolvedModel?.Trim(), filterModel, StringComparison.OrdinalIgnoreCase))
                {
                    skippedCount++;
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
            // Sync：按组从明细合计产品月成本并新增/更新主表，再刷新机种月均
            await _bomMaterialCostService.SyncBomMaterialCostFromItemsBatchAsync(syncKeys);
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

    /// <summary>
    /// 构建定时任务目标月查询：仅判定日所在自然月（CostingDate 当月，避免跨月重复合计）
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

    // ========================================
    // 转置 / 差异 / 月度涨跌分析
    // ========================================

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemTransposedResultDto> GetBomMaterialCostItemTransposedListAsync(
        TaktBomMaterialCostItemTransposedQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
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
        var productGroups = rows
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => r.ProductCode!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        var productCatalog = productGroups
            .Where(kv => kv.Value.Any(r =>
                !string.IsNullOrWhiteSpace(r.CostingPeriod)
                && displayPeriodSet.Contains(r.CostingPeriod!)))
            .Select(kv => (
                ProductCode: kv.Key,
                ProductDescription: kv.Value.OrderByDescending(r => r.CostingDate).FirstOrDefault()?.ProductDescription?.Trim() ?? string.Empty))
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
        TaktBomMaterialCostItemModelSummaryDto? modelSummary = null;
        if (!string.IsNullOrWhiteSpace(modelCode) && string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var modelNameLookup = await BuildModelNameLookupAsync();
            modelNameLookup.TryGetValue(modelCode, out var modelName);
            modelSummary = BuildModelSummary(modelCode, modelName ?? modelCode, allTransposedRows, periodOrder);
        }
        var filteredRows = FilterTransposedRowsByTrend(allTransposedRows, queryDto.TrendFilter);
        var total = filteredRows.Count;
        var transposedRows = filteredRows.Skip(skip).Take(pageSize).ToList();
        var (periodCostTotals, varianceAmountTotal) = SumTransposedRowGrandTotals(filteredRows, periodOrder);
        return new TaktBomMaterialCostItemTransposedResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostItemTransposedDto>.Create(transposedRows, total, pageIndex, pageSize),
            PeriodOrder = periodOrder,
            ModelSummary = modelSummary,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemTransposedAsync(
        TaktBomMaterialCostItemTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        query ??= new TaktBomMaterialCostItemTransposedQueryDto();
        query.PageIndex = 1;
        query.PageSize = TaktPagedClamp.DefaultMaxPageSize;
        var result = await GetBomMaterialCostItemTransposedListAsync(query);
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

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemVarianceResultDto> GetBomMaterialCostItemVarianceAnalysisAsync(
        TaktBomMaterialCostItemVarianceQueryDto queryDto)
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
        return new TaktBomMaterialCostItemVarianceResultDto
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

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemVarianceAnalysisAsync(
        TaktBomMaterialCostItemVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostItemVarianceAnalysisAsync(query);
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
            "bomItemNo", "componentCode", "componentDescription", "purchaseType", "currency",
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
            ["bomItemNo"] = line.BomItemNo,
            ["componentCode"] = line.ComponentCode,
            ["componentDescription"] = line.ComponentDescription,
            ["purchaseType"] = line.PurchaseType,
            ["currency"] = line.Currency,
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
        if (detailSheet.Dimension != null)
        {
            detailSheet.Cells[detailSheet.Dimension.Address].AutoFitColumns();
        }
        var actualFileName = fileName ?? "BOM材料成本差异分析.xlsx";
        var content = await package.GetAsByteArrayAsync();
        return (actualFileName, content);
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemMonthlyTrendResultDto> GetBomMaterialCostItemMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostItemMonthlyTrendQueryDto queryDto)
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
        var loadQuery = new TaktBomMaterialCostItemTransposedQueryDto
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
        var trendLines = new List<TaktBomMaterialCostItemMonthlyTrendLineDto>();
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
            trendLines.Add(new TaktBomMaterialCostItemMonthlyTrendLineDto
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
        return new TaktBomMaterialCostItemMonthlyTrendResultDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode ?? string.Empty,
            ProductDescription = productDescription,
            AllMaterialsUnderModel = allMaterialsUnderModel,
            Lines = trendLines,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostItemMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostItemMonthlyTrendAnalysisAsync(query);
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

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemComponentMovingPriceResultDto> GetBomMaterialCostItemComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemComponentMovingPriceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildComponentMovingPriceAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        var (periodCostTotals, varianceAmountTotal) = SumComponentMovingPriceRowGrandTotals(
            built.OrderedRows, built.PeriodOrder);
        return new TaktBomMaterialCostItemComponentMovingPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostItemComponentMovingPriceDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            ProductCodes = built.ProductCodes,
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

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemComponentMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        // 导出取全量分析行，勿套用列表 MaxPageSize(100)；按条件全量不截断
        var built = await BuildComponentMovingPriceAnalysisAsync(query);
        var (periodCostTotals, varianceAmountTotal) = SumComponentMovingPriceRowGrandTotals(
            built.OrderedRows, built.PeriodOrder);
        var result = new TaktBomMaterialCostItemComponentMovingPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostItemComponentMovingPriceDto>.Create(
                built.OrderedRows, built.OrderedRows.Count, 1, Math.Max(built.OrderedRows.Count, 1)),
            PeriodOrder = built.PeriodOrder,
            ProductCodes = built.ProductCodes,
            ComponentCount = built.OrderedRows.Count,
            BasePeriod = built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "productCode", "productDescription",
            "sequenceNo", "bomLevel", "bomItemNo",
            "componentCode", "componentDescription", "componentQuantity",
            "productionRelated", "purchaseType", "currency",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "产品编码", "产品描述",
            "序号", "层级", "BOM项目号",
            "组件编码", "组件描述", "组件数量",
            "生产相关", "采购类型", "币种",
        };
        foreach (var period in result.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });

        var exportRows = (result.Paged.Data ?? new List<TaktBomMaterialCostItemComponentMovingPriceDto>())
            .Select(row =>
            {
                var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["plantCode"] = row.PlantCode,
                    ["modelCode"] = row.ModelCode,
                    ["productCode"] = row.ProductCode,
                    ["productDescription"] = row.ProductDescription,
                    ["sequenceNo"] = row.SequenceNo,
                    ["bomLevel"] = row.BomLevel,
                    ["bomItemNo"] = row.BomItemNo,
                    ["componentCode"] = row.ComponentCode,
                    ["componentDescription"] = row.ComponentDescription,
                    ["componentQuantity"] = row.ComponentQuantity,
                    ["productionRelated"] = row.ProductionRelated,
                    ["purchaseType"] = row.PurchaseType,
                    ["currency"] = row.Currency,
                    ["basePeriod"] = row.BasePeriod,
                    ["comparePeriod"] = row.ComparePeriod,
                    ["varianceAmount"] = row.VarianceAmount,
                    ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(row.VariancePercent),
                    ["trend"] = row.Trend,
                };
                foreach (var period in result.PeriodOrder)
                {
                    dict[$"period_{period}"] = row.PeriodMaterialCosts.TryGetValue(period, out var cost)
                        ? cost
                        : null;
                }
                return (IReadOnlyDictionary<string, object?>)dict;
            })
            .ToList();

        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "DTA 产品成本分析表",
            fileName ?? "DTA 产品成本分析表.xlsx");
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemModelMovingPriceResultDto> GetBomMaterialCostItemModelMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemModelMovingPriceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildModelMovingPriceAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        var (periodCostTotals, varianceAmountTotal) = SumModelMovingPriceRowGrandTotals(
            built.OrderedRows, built.PeriodOrder);
        return new TaktBomMaterialCostItemModelMovingPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostItemModelMovingPriceDto>.Create(
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

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemModelMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemModelMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildModelMovingPriceAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "componentCode", "modelName", "productCodes",
            "componentDescription", "productionRelated", "purchaseType", "productCount", "currency",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "组件编码", "机种名称", "产品组",
            "组件描述", "生产相关", "采购类型", "产品数", "币种",
        };
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
                    ["modelName"] = row.ModelName,
                    ["componentCode"] = row.ComponentCode,
                    ["componentDescription"] = row.ComponentDescription,
                    ["productionRelated"] = row.ProductionRelated,
                    ["purchaseType"] = row.PurchaseType,
                    ["productCodes"] = row.ProductCodes,
                    ["productCount"] = row.ProductCount,
                    ["currency"] = row.Currency,
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

        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "DTA 机种成本推移表",
            fileName ?? "DTA 机种成本推移表.xlsx");
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostItemZeroMovingPriceResultDto> GetBomMaterialCostItemZeroMovingPriceMergedAsync(
        TaktBomMaterialCostItemZeroMovingPriceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ModelCode);
        EnsureThreeLayerContext();

        var (costingStart, costingEnd, costingPeriod) = PrepareZeroMovingPriceCostingMonth(queryDto);
        var plantCode = queryDto.PlantCode.Trim();
        var modelCode = queryDto.ModelCode.Trim();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);

        var productCodes = await LoadModelProductCodesAsync(plantCode, modelCode, costingStart, costingStart);
        if (productCodes.Count == 0)
        {
            return new TaktBomMaterialCostItemZeroMovingPriceResultDto
            {
                Paged = TaktPagedResult<TaktBomMaterialCostItemZeroMovingPriceDto>.Create(
                    new List<TaktBomMaterialCostItemZeroMovingPriceDto>(), 0, pageIndex, pageSize),
                ProductCodes = productCodes,
                ComponentCount = 0,
                CostingPeriod = costingPeriod,
            };
        }

        var merged = await LoadZeroMovingPriceMergedComponentsAsync(
            plantCode, productCodes, costingStart, costingEnd);
        var allRows = merged
            .Select(c => new TaktBomMaterialCostItemZeroMovingPriceDto
            {
                PlantCode = plantCode,
                ModelCode = modelCode,
                ComponentCode = c.ComponentCode,
                ComponentDescription = c.ComponentDescription,
                ProductCodes = string.Join(",", c.ProductCodes.OrderBy(p => p, StringComparer.Ordinal)),
                ProductCount = c.ProductCodes.Count,
                MovingAveragePrice = 0m,
                CostingPeriod = costingPeriod,
            })
            .OrderBy(r => r.ComponentCode, StringComparer.Ordinal)
            .ToList();

        await FillZeroMovingPriceSuggestedRevisionsAsync(plantCode, costingStart, allRows);

        var pageRows = allRows.Skip(skip).Take(pageSize).ToList();
        return new TaktBomMaterialCostItemZeroMovingPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostItemZeroMovingPriceDto>.Create(
                pageRows, allRows.Count, pageIndex, pageSize),
            ProductCodes = productCodes,
            ComponentCount = allRows.Count,
            CostingPeriod = costingPeriod,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemZeroMovingPriceMergedAsync(
        TaktBomMaterialCostItemZeroMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        query.PageIndex = 1;
        query.PageSize = TaktPagedClamp.DefaultMaxPageSize;
        var result = await GetBomMaterialCostItemZeroMovingPriceMergedAsync(query);
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
        var exportRows = (result.Paged.Data ?? new List<TaktBomMaterialCostItemZeroMovingPriceDto>())
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

        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "零价格合并",
            fileName ?? $"零价格合并_{query.ModelCode}_{result.CostingPeriod}.xlsx");
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetBomMaterialCostItemModelOptionsAsync(string? plantCode = null)
    {
        // 机种来自汇总表 takt_bom_material_cost（与 TaktBomMaterialCosts/model-options 同口径）
        EnsureThreeLayerContext();
        var trimmedPlant = plantCode?.Trim();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.ModelCode ?? string.Empty,
            false);
        var modelNameLookup = await BuildModelNameLookupAsync();
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

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetBomMaterialCostItemProductOptionsByModelAsync(string? modelCode, string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var trimmedModelCode = modelCode?.Trim();
        var trimmedPlant = plantCode?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedModelCode) && string.IsNullOrWhiteSpace(trimmedPlant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (string.IsNullOrWhiteSpace(trimmedModelCode) || x.ModelCode == trimmedModelCode)
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.ProductCode ?? string.Empty,
            false);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ProductCode))
            .GroupBy(e => e.ProductCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Take(MaxCascadeSelectOptions)
            .Select(g =>
            {
                var first = g.OrderByDescending(x => x.CostingDate).First();
                var description = first.ProductDescription?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                    ExtValue = first.ModelCode,
                };
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<string?> GetBomMaterialCostItemModelCodeByProductAsync(string productCode, string? plantCode = null)
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
    /// 加载转置/月度涨跌用成本汇总行
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>成本汇总行</returns>
    private async Task<List<TaktBomMaterialCost>> LoadTransposedCostHeadersAsync(TaktBomMaterialCostItemTransposedQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
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
    /// 克隆转置查询并将起始日提前一月，便于环比取上月成本（不改变展示用 CostingDateStart）
    /// </summary>
    /// <param name="queryDto">原查询</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <returns>加载用查询</returns>
    private static TaktBomMaterialCostItemTransposedQueryDto CloneTransposedQueryForMomLoad(
        TaktBomMaterialCostItemTransposedQueryDto queryDto,
        string? focusPeriod)
    {
        var loadQuery = new TaktBomMaterialCostItemTransposedQueryDto
        {
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
            KeyWords = queryDto.KeyWords,
            PlantCode = queryDto.PlantCode,
            ModelCode = queryDto.ModelCode,
            ProductCode = queryDto.ProductCode,
            CostingDateStart = queryDto.CostingDateStart,
            CostingDateEnd = queryDto.CostingDateEnd,
            FocusPeriod = queryDto.FocusPeriod,
            TrendFilter = queryDto.TrendFilter,
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
    private static TaktBomMaterialCostItemTransposedDto BuildTransposedRowForCatalogItem(
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
            return new TaktBomMaterialCostItemTransposedDto
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
    private static TaktBomMaterialCostItemTransposedDto BuildTransposedRowFromCostHeaders(
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
        return new TaktBomMaterialCostItemTransposedDto
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
        TaktBomMaterialCostItemTransposedDto row,
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
    private static List<TaktBomMaterialCostItemTransposedDto> FilterTransposedRowsByTrend(
        IReadOnlyList<TaktBomMaterialCostItemTransposedDto> rows,
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
    private static TaktBomMaterialCostItemModelSummaryDto BuildModelSummary(
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostItemTransposedDto> rows,
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
        return new TaktBomMaterialCostItemModelSummaryDto
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
    private static TaktBomMaterialCostItemVarianceLineDto BuildVarianceLine(
        TaktBomMaterialCostItem? baseRow,
        TaktBomMaterialCostItem? compareRow)
    {
        var baseCost = baseRow != null ? TaktBomMaterialCostItemLineCostHelper.CalculateLineCost(baseRow) : 0m;
        var compareCost = compareRow != null ? TaktBomMaterialCostItemLineCostHelper.CalculateLineCost(compareRow) : 0m;
        var baseUnitPrice = baseRow != null ? TaktBomMaterialCostItemLineCostHelper.ResolveEffectiveUnitPrice(baseRow) : 0m;
        var compareUnitPrice = compareRow != null ? TaktBomMaterialCostItemLineCostHelper.ResolveEffectiveUnitPrice(compareRow) : 0m;
        var baseQty = baseRow?.ComponentQuantity ?? 0m;
        var compareQty = compareRow?.ComponentQuantity ?? 0m;
        var priceUnit = baseRow != null
            ? TaktBomMaterialCostItemLineCostHelper.ResolvePriceUnit(baseRow)
            : (compareRow != null ? TaktBomMaterialCostItemLineCostHelper.ResolvePriceUnit(compareRow) : 1);
        if (priceUnit <= 0)
        {
            priceUnit = 1;
        }
        var unitPriceVariance = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareUnitPrice - baseUnitPrice);
        var quantityVariance = compareQty - baseQty;
        var priceEffect = TaktBomMaterialCostItemLineCostHelper.RoundCost(unitPriceVariance * baseQty / priceUnit);
        var quantityEffect = TaktBomMaterialCostItemLineCostHelper.RoundCost(quantityVariance * baseUnitPrice / priceUnit);
        var varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareCost - baseCost);
        decimal? variancePercent = null;
        if (baseCost != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(varianceAmount / baseCost);
        }
        var changeType = ResolveVarianceChangeType(baseRow, compareRow, unitPriceVariance, quantityVariance);
        return new TaktBomMaterialCostItemVarianceLineDto
        {
            BomItemNo = compareRow?.BomItemNo ?? baseRow?.BomItemNo ?? string.Empty,
            ComponentCode = compareRow?.ComponentCode ?? baseRow?.ComponentCode ?? string.Empty,
            ComponentDescription = compareRow?.ComponentDescription ?? baseRow?.ComponentDescription ?? string.Empty,
            PurchaseType = compareRow?.PurchaseType ?? baseRow?.PurchaseType ?? string.Empty,
            Currency = compareRow != null
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
    /// 机种编码 → 产品编码集合（明细行无 ModelCode；未显式指定产品时用于列表/导出缩小范围）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
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
    /// 规范化零价合并查询的核算月（须单月）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>月初、月末、yyyy-MM</returns>
    private static (DateTime Start, DateTime End, string Period) PrepareZeroMovingPriceCostingMonth(
        TaktBomMaterialCostItemZeroMovingPriceQueryDto queryDto)
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
    /// 为零价组件填充建议代替：末字母依次前推，取同月移动价格&gt;0 的首个版本
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="periodMonthStart">核算月首日</param>
    /// <param name="rows">零价合并行</param>
    private async Task FillZeroMovingPriceSuggestedRevisionsAsync(
        string plantCode,
        DateTime periodMonthStart,
        List<TaktBomMaterialCostItemZeroMovingPriceDto> rows)
    {
        if (rows.Count == 0)
        {
            return;
        }

        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var candidatesByComponent = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var candidates = EnumeratePreviousLetterRevisions(row.ComponentCode).ToList();
            candidatesByComponent[row.ComponentCode] = candidates;
            foreach (var code in candidates)
            {
                candidateSet.Add(code);
            }
        }

        if (candidateSet.Count == 0)
        {
            return;
        }

        var priceRows = await LoadMovingPricesForComponentsAsync(
            plantCode,
            candidateSet.ToList(),
            periodMonthStart,
            periodMonthStart,
            valuationFilter: null);

        // 同物料同月多评估类别时取 MovingPrice>0 的最大价
        var priceByMaterial = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        foreach (var price in priceRows)
        {
            if (string.IsNullOrWhiteSpace(price.MaterialCode) || price.MovingPrice <= 0m)
            {
                continue;
            }
            var key = price.MaterialCode.Trim();
            if (!priceByMaterial.TryGetValue(key, out var existing) || price.MovingPrice > existing)
            {
                priceByMaterial[key] = price.MovingPrice;
            }
        }

        foreach (var row in rows)
        {
            if (!candidatesByComponent.TryGetValue(row.ComponentCode, out var candidates))
            {
                continue;
            }
            foreach (var candidate in candidates)
            {
                if (!priceByMaterial.TryGetValue(candidate, out var suggestedPrice))
                {
                    continue;
                }
                row.SuggestedComponentCode = candidate;
                row.SuggestedMovingPrice = suggestedPrice;
                break;
            }
        }
    }

    /// <summary>
    /// 枚举组件编码末字母前推版本（如 A00001D → A00001C、A00001B、A00001A）
    /// </summary>
    /// <param name="componentCode">当前组件编码</param>
    /// <returns>前推候选（近→远）</returns>
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
    /// 加载核算月内移动平均价=0 的 X+F 明细并按组件合并产品
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingDateStart">核算日起</param>
    /// <param name="costingDateEnd">核算日止</param>
    /// <returns>合并组件</returns>
    private async Task<List<MergedBomComponent>> LoadZeroMovingPriceMergedComponentsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime costingDateStart,
        DateTime costingDateEnd)
    {
        var allItems = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        for (var i = 0; i < productCodes.Count; i += chunkSize)
        {
            var chunk = productCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktBomMaterialCostItem, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.ProductCode)
                && x.CostingDate >= costingDateStart
                && x.CostingDate <= costingDateEnd
                && x.MovingAveragePrice == 0;
            var part = await GetBomItemListForRangeAsync(
                predicate,
                costingDateStart,
                costingDateEnd,
                MaxAnalysisRowLoad - allItems.Count);
            allItems.AddRange(TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(part));
            if (allItems.Count >= MaxAnalysisRowLoad)
            {
                ThrowBusinessException($"零价 BOM 明细行为 {allItems.Count}，达到上限 {MaxAnalysisRowLoad}，请缩小机种范围");
            }
        }

        var map = new Dictionary<string, MergedBomComponent>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in allItems)
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
            }
            else if (string.IsNullOrWhiteSpace(merged.ComponentDescription)
                && !string.IsNullOrWhiteSpace(item.ComponentDescription))
            {
                merged.ComponentDescription = item.ComponentDescription.Trim();
            }
            if (!string.IsNullOrWhiteSpace(item.ProductCode))
            {
                merged.ProductCodes.Add(item.ProductCode.Trim());
            }
        }
        return map.Values
            .OrderBy(c => c.ComponentCode, StringComparer.Ordinal)
            .ToList();
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
    /// 批量加载组件对应移动价格行
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="componentCodes">组件编码</param>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <param name="valuationFilter">评估类别可选</param>
    /// <returns>移动价格行</returns>
    private async Task<List<TaktMaterialMovingPrice>> LoadMovingPricesForComponentsAsync(
        string plantCode,
        IReadOnlyList<string> componentCodes,
        DateTime? periodStart,
        DateTime? periodEnd,
        string? valuationFilter)
    {
        var result = new List<TaktMaterialMovingPrice>();
        const int chunkSize = 200;
        for (var i = 0; i < componentCodes.Count; i += chunkSize)
        {
            var chunk = componentCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktMaterialMovingPrice>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode));
            if (periodStart.HasValue)
            {
                var start = periodStart.Value;
                exp = exp.And(x => x.PeriodDate >= start);
            }
            if (periodEnd.HasValue)
            {
                var end = periodEnd.Value;
                exp = exp.And(x => x.PeriodDate <= end);
            }
            if (!string.IsNullOrWhiteSpace(valuationFilter))
            {
                var valuation = valuationFilter;
                exp = exp.And(x => x.Valuation == valuation);
            }
            var part = await GetMovingPriceListForRangeAsync(
                exp.ToExpression(),
                periodStart,
                periodEnd,
                MaxAnalysisRowLoad - result.Count);
            result.AddRange(part);
            if (result.Count >= MaxAnalysisRowLoad)
            {
                ThrowBusinessException($"移动价格行为 {result.Count}，达到上限 {MaxAnalysisRowLoad}，请缩小期间范围");
            }
        }
        return result;
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
    /// 产品成本分析：单个产品下 BOM 明细行（TaktBomMaterialCostItem）× 月材料成本转置涨跌
    /// </summary>
    /// <param name="queryDto">查询条件（工厂+产品必填）</param>
    /// <returns>排序后的全量明细行与汇总</returns>
    private async Task<ComponentMovingPriceAnalysisBuilt> BuildComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemComponentMovingPriceQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ProductCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var modelCode = string.IsNullOrWhiteSpace(queryDto.ModelCode) ? null : queryDto.ModelCode.Trim();
        var (periodStart, periodEnd) = NormalizeMovingPricePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);
        var filterProduct = queryDto.ProductCode.Trim();

        var productMeta = await LoadProductMetaAsync(plantCode, modelCode, periodStart, periodEnd);
        var productCodesFromMeta = productMeta.Keys
            .Where(c => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(c, filterProduct))
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        var lookupCodes = TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(filterProduct)
            .Concat(productCodesFromMeta.SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            lookupCodes = new List<string> { filterProduct };
        }

        // 环比需关注月上月成本：加载区间向前扩 1 个月；展示列仍用原核算期间
        var loadStart = periodStart;
        var focusHint = !string.IsNullOrWhiteSpace(queryDto.FocusPeriod)
            ? queryDto.FocusPeriod.Trim()
            : (periodEnd.HasValue ? periodEnd.Value.ToString("yyyy-MM") : null);
        if (!string.IsNullOrWhiteSpace(focusHint)
            && DateTime.TryParseExact(
                focusHint + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var focusMonth))
        {
            var momStart = new DateTime(focusMonth.Year, focusMonth.Month, 1).AddMonths(-1);
            if (!loadStart.HasValue || loadStart.Value > momStart)
            {
                loadStart = momStart;
            }
        }

        var costItemsRaw = await LoadBomCostItemsForProductsAsync(plantCode, lookupCodes, loadStart, periodEnd);
        var costItems = costItemsRaw
            .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, filterProduct))
            .ToList();
        var productCodes = costItems
            .Select(r => r.ProductCode?.Trim() ?? string.Empty)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        if (productCodes.Count == 0)
        {
            productCodes = productCodesFromMeta.Count > 0
                ? productCodesFromMeta
                : new List<string> { filterProduct };
        }
        if (costItems.Count == 0)
        {
            return ComponentMovingPriceAnalysisBuilt.Empty(productCodes);
        }

        var periodOrder = BuildCostingPeriodOrder(costItems, periodStart, periodEnd);
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var productCode = productCodes[0];
        var meta = productMeta.TryGetValue(productCode, out var m)
            ? m
            : productMeta.FirstOrDefault(kv =>
                TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(kv.Key, filterProduct)).Value
                ?? new ModelProductMeta();
        var rowModelCode = !string.IsNullOrWhiteSpace(meta.ModelCode)
            ? meta.ModelCode
            : (modelCode ?? string.Empty);
        var productDescription = meta.Description?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(productDescription))
        {
            productDescription = costItems
                .OrderByDescending(r => r.CostingDate)
                .Select(r => r.ProductDescription?.Trim())
                .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))
                ?? string.Empty;
        }

        // 按 BOM 明细行业务键展开（Sequence+Level+Item+Component+Qty…），不做机种级合并
        var lineGroups = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(BuildBomLineTrendKey, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .ToList();
        var allRows = lineGroups
            .Select(group => BuildProductComponentMaterialCostTrendRow(
                plantCode,
                rowModelCode,
                productCode,
                productDescription,
                group.ToList(),
                periodOrder,
                focusPeriod))
            .Where(r => r.PeriodMaterialCosts.Count > 0)
            .ToList();

        var filtered = FilterComponentMovingPriceRows(allRows, queryDto.TrendFilter);
        var ordered = OrderComponentMovingPriceRowsByBomStructure(filtered);
        return new ComponentMovingPriceAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            ProductCodes = productCodes,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// BOM 成本推移（产品×月材料成本）内存构建结果
    /// </summary>
    private sealed class ComponentMovingPriceAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktBomMaterialCostItemComponentMovingPriceDto> OrderedRows { get; init; } = new();

        /// <summary>期间列顺序</summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>机种下产品编码</summary>
        public List<string> ProductCodes { get; init; } = new();

        /// <summary>基准期间</summary>
        public string? BasePeriod { get; init; }

        /// <summary>对比期间</summary>
        public string? ComparePeriod { get; init; }

        /// <summary>上涨行数（过滤前全量趋势统计）</summary>
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
        public static ComponentMovingPriceAnalysisBuilt Empty(List<string> productCodes) => new()
        {
            ProductCodes = productCodes,
        };
    }

    /// <summary>
    /// 机种产品元数据（机种/描述/币种）
    /// </summary>
    private sealed class ModelProductMeta
    {
        /// <summary>机种编码</summary>
        public string ModelCode { get; set; } = string.Empty;

        /// <summary>产品描述</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>币种</summary>
        public string Currency { get; set; } = string.Empty;
    }

    /// <summary>
    /// 从主表加载产品编码及描述/币种/机种（可按机种、核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种（空=工厂下全部机种产品）</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <returns>产品编码 → 元数据</returns>
    private async Task<Dictionary<string, ModelProductMeta>> LoadProductMetaAsync(
        string plantCode,
        string? modelCode = null,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null)
    {
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        if (!string.IsNullOrWhiteSpace(modelCode))
        {
            var model = modelCode.Trim();
            exp = exp.And(x => x.ModelCode == model);
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
                Currency = header.CurrencyCode?.Trim() ?? string.Empty,
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
        return LoadProductMetaAsync(plantCode, modelCode, costingMonthStart, costingMonthEnd);
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

    /// <summary>
    /// 机种分析合并键：Plant+ComponentCode+ProductionRelated+PurchaseType
    /// </summary>
    /// <param name="item">明细行</param>
    /// <returns>稳定键</returns>
    private static string BuildModelCostMergeKey(TaktBomMaterialCostItem item)
    {
        return string.Join(
            "|",
            item.PlantCode?.Trim() ?? string.Empty,
            item.ComponentCode?.Trim() ?? string.Empty,
            item.ProductionRelated?.Trim() ?? string.Empty,
            item.PurchaseType?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// 构建单个产品下 BOM 明细行 × 月材料成本（按 BuildComponentKey 跨月对齐）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="productCode">产品</param>
    /// <param name="productDescription">产品描述</param>
    /// <param name="keyItems">同 BOM 行键明细（可含环比扩窗上月）</param>
    /// <param name="periodOrder">展示期间列</param>
    /// <param name="focusPeriod">关注月</param>
    /// <returns>明细推移行</returns>
    private static TaktBomMaterialCostItemComponentMovingPriceDto BuildProductComponentMaterialCostTrendRow(
        string plantCode,
        string modelCode,
        string productCode,
        string productDescription,
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        var identity = keyItems
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .First();
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var currency = string.Empty;
        foreach (var period in periodOrder)
        {
            var monthCost = ResolveBomLineMaterialCostForPeriod(keyItems, period);
            if (monthCost == null)
            {
                continue;
            }
            periodCosts[period] = monthCost.Value;
            if (string.IsNullOrWhiteSpace(currency))
            {
                var picked = keyItems
                    .Where(r => ToPeriodKey(r.CostingDate) == period)
                    .OrderByDescending(r => r.CostingDate)
                    .ThenByDescending(r => r.Id)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(picked?.MovingPriceCurrency))
                {
                    currency = picked.MovingPriceCurrency.Trim();
                }
            }
        }

        // 环比基准月可在展示列之外（扩窗加载），写入字典供涨跌计算后再裁剪
        if (!string.IsNullOrWhiteSpace(focusPeriod)
            && DateTime.TryParseExact(
                focusPeriod.Trim() + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
            if (!periodCosts.ContainsKey(basePeriod))
            {
                var baseCost = ResolveBomLineMaterialCostForPeriod(keyItems, basePeriod);
                if (baseCost != null)
                {
                    periodCosts[basePeriod] = baseCost.Value;
                }
            }
        }

        var description = productDescription;
        if (string.IsNullOrWhiteSpace(description))
        {
            description = keyItems
                .OrderByDescending(r => r.CostingDate)
                .Select(r => r.ProductDescription?.Trim())
                .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))
                ?? string.Empty;
        }

        var row = new TaktBomMaterialCostItemComponentMovingPriceDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode,
            ProductDescription = description,
            SequenceNo = identity.SequenceNo?.Trim() ?? string.Empty,
            BomLevel = identity.BomLevel?.Trim() ?? string.Empty,
            BomItemNo = identity.BomItemNo?.Trim() ?? string.Empty,
            ComponentCode = identity.ComponentCode?.Trim() ?? string.Empty,
            ComponentDescription = identity.ComponentDescription?.Trim() ?? string.Empty,
            ComponentQuantity = identity.ComponentQuantity,
            ProductionRelated = identity.ProductionRelated?.Trim(),
            PurchaseType = identity.PurchaseType?.Trim() ?? string.Empty,
            Currency = currency,
            PeriodMaterialCosts = periodCosts,
        };
        ApplyMaterialCostFocusTrend(row.PeriodMaterialCosts, focusPeriod, row);
        var displaySet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        row.PeriodMaterialCosts = row.PeriodMaterialCosts
            .Where(kv => displaySet.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);
        return row;
    }

    /// <summary>
    /// 同 BOM 行键在某月取最新核算日行成本（单行键；无数据返回 null）
    /// </summary>
    /// <param name="items">同键明细</param>
    /// <param name="periodKey">yyyy-MM</param>
    /// <returns>行材料成本</returns>
    private static decimal? ResolveBomLineMaterialCostForPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> items,
        string periodKey)
    {
        var picked = items
            .Where(r => ToPeriodKey(r.CostingDate) == periodKey)
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault();
        if (picked == null)
        {
            return null;
        }
        return TaktBomMaterialCostItemLineCostHelper.CalculateLineCost(picked);
    }

    /// <summary>
    /// 将环比结果写入产品/组件推移行
    /// </summary>
    /// <param name="periodCosts">各月材料成本</param>
    /// <param name="focusPeriod">关注月</param>
    /// <param name="row">目标行</param>
    private static void ApplyMaterialCostFocusTrend(
        IReadOnlyDictionary<string, decimal> periodCosts,
        string? focusPeriod,
        TaktBomMaterialCostItemComponentMovingPriceDto row)
    {
        ApplyUnitPriceFocusTrend(
            periodCosts,
            focusPeriod,
            out var trend,
            out var basePeriod,
            out var comparePeriod,
            out var varianceAmount,
            out var variancePercent);
        row.Trend = trend;
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.VarianceAmount = varianceAmount;
        row.VariancePercent = variancePercent;
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
    /// 按涨跌筛选过滤产品行
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>过滤后列表</returns>
    private static List<TaktBomMaterialCostItemComponentMovingPriceDto> FilterComponentMovingPriceRows(
        IReadOnlyList<TaktBomMaterialCostItemComponentMovingPriceDto> rows,
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
    /// 产品成本明细按 BOM 展开序：ProductCode → SequenceNo → BomLevel（1…n 后 .1、..2）
    /// </summary>
    /// <param name="rows">行</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomMaterialCostItemComponentMovingPriceDto> OrderComponentMovingPriceRowsByBomStructure(
        IReadOnlyList<TaktBomMaterialCostItemComponentMovingPriceDto> rows)
    {
        return rows
            .OrderBy(r => r, Comparer<TaktBomMaterialCostItemComponentMovingPriceDto>.Create(static (a, b) =>
                TaktBomMaterialCostItemLineCostHelper.CompareBomExplosionOrder(
                    a.ProductCode,
                    a.SequenceNo,
                    a.BomLevel,
                    b.ProductCode,
                    b.SequenceNo,
                    b.BomLevel)))
            .ThenBy(r => r.BomItemNo, StringComparer.Ordinal)
            .ThenBy(r => r.ComponentCode, StringComparer.Ordinal)
            .ThenBy(r => r.PurchaseType, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 构建机种成本推移：机种月材料成本 + 合并键月材料成本分析（缺月不回填；按条件全量不截断）
    /// 机种/产品均可空：皆空=工厂期间全量产品；仅产品=该产品；仅机种=机种下全部产品
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>排序后的全量行与汇总</returns>
    private async Task<ModelMovingPriceAnalysisBuilt> BuildModelMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemModelMovingPriceQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var productFilter = string.IsNullOrWhiteSpace(queryDto.ProductCode) ? null : queryDto.ProductCode.Trim();
        var modelCode = string.IsNullOrWhiteSpace(queryDto.ModelCode) ? string.Empty : queryDto.ModelCode.Trim();
        var (periodStart, periodEnd) = NormalizeMovingPricePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);

        if (string.IsNullOrWhiteSpace(modelCode) && !string.IsNullOrWhiteSpace(productFilter))
        {
            modelCode = await GetBomMaterialCostItemModelCodeByProductAsync(productFilter, plantCode) ?? string.Empty;
        }

        var modelNameLookup = await BuildModelNameLookupAsync();
        var modelName = !string.IsNullOrWhiteSpace(modelCode)
            && modelNameLookup.TryGetValue(modelCode, out var name)
            && !string.IsNullOrWhiteSpace(name)
            ? name.Trim()
            : modelCode;

        List<string> productCodes;
        if (!string.IsNullOrWhiteSpace(productFilter))
        {
            productCodes = new List<string> { productFilter };
        }
        else if (!string.IsNullOrWhiteSpace(modelCode))
        {
            productCodes = await LoadModelProductCodesAsync(plantCode, modelCode, periodStart, periodEnd);
        }
        else
        {
            var plantMeta = await LoadProductMetaAsync(plantCode, null, periodStart, periodEnd);
            productCodes = plantMeta.Keys.OrderBy(c => c, StringComparer.Ordinal).ToList();
            modelName = string.Empty;
        }
        if (productCodes.Count == 0)
        {
            return ModelMovingPriceAnalysisBuilt.Empty(productCodes);
        }

        var costItems = await LoadBomCostItemsForProductsAsync(plantCode, productCodes, periodStart, periodEnd);
        if (costItems.Count == 0)
        {
            return ModelMovingPriceAnalysisBuilt.Empty(productCodes);
        }

        var periodOrder = BuildCostingPeriodOrder(costItems, periodStart, periodEnd);
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);

        var modelPeriodCosts = BuildModelPeriodMaterialCosts(productCodes, costItems, periodOrder);
        ApplyUnitPriceFocusTrend(
            modelPeriodCosts,
            focusPeriod,
            out var modelTrend,
            out var modelBasePeriod,
            out var modelComparePeriod,
            out var modelVarianceAmount,
            out var modelVariancePercent);

        var mergeGroups = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(BuildModelCostMergeKey, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .ToList();

        var allRows = mergeGroups
            .Select(group => BuildModelMergeKeyMaterialCostRow(
                plantCode, modelCode, modelName, group.ToList(), periodOrder, focusPeriod))
            .Where(r => r.PeriodMaterialCosts.Count > 0)
            .ToList();

        var filtered = FilterModelMovingPriceRows(allRows, queryDto.TrendFilter);
        var ordered = OrderModelMovingPriceRowsByTrend(filtered);
        return new ModelMovingPriceAnalysisBuilt
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
    private sealed class ModelMovingPriceAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktBomMaterialCostItemModelMovingPriceDto> OrderedRows { get; init; } = new();

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
        public static ModelMovingPriceAnalysisBuilt Empty(List<string> productCodes) => new()
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
    /// 构建合并键 × 月材料成本分析行
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="modelName">机种名称</param>
    /// <param name="keyItems">同合并键明细</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注月</param>
    /// <returns>分析行</returns>
    private static TaktBomMaterialCostItemModelMovingPriceDto BuildModelMergeKeyMaterialCostRow(
        string plantCode,
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        var identity = keyItems
            .OrderBy(r => r.CostingDate)
            .ThenBy(r => r.Id)
            .First();
        var productSet = keyItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .Select(r => r.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var currency = string.Empty;
        foreach (var period in periodOrder)
        {
            var monthCost = SumMaterialCostByLineKeyForPeriod(keyItems, period);
            if (monthCost == null)
            {
                continue;
            }
            periodCosts[period] = monthCost.Value;
            if (string.IsNullOrWhiteSpace(currency))
            {
                var picked = keyItems
                    .Where(r => ToPeriodKey(r.CostingDate) == period)
                    .OrderByDescending(r => r.CostingDate)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(picked?.MovingPriceCurrency))
                {
                    currency = picked.MovingPriceCurrency.Trim();
                }
            }
        }

        var row = new TaktBomMaterialCostItemModelMovingPriceDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ModelName = modelName,
            ComponentCode = identity.ComponentCode?.Trim() ?? string.Empty,
            ComponentDescription = identity.ComponentDescription?.Trim() ?? string.Empty,
            ProductionRelated = identity.ProductionRelated?.Trim(),
            PurchaseType = identity.PurchaseType?.Trim() ?? string.Empty,
            ProductCodes = string.Join(",", productSet),
            ProductCount = productSet.Count,
            Currency = currency,
            PeriodMaterialCosts = periodCosts,
        };
        ApplyUnitPriceFocusTrend(
            row.PeriodMaterialCosts,
            focusPeriod,
            out var trend,
            out var basePeriod,
            out var comparePeriod,
            out var varianceAmount,
            out var variancePercent);
        row.Trend = trend;
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.VarianceAmount = varianceAmount;
        row.VariancePercent = variancePercent;
        return row;
    }

    /// <summary>
    /// 按涨跌筛选过滤机种合并分析行
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>过滤后列表</returns>
    private static List<TaktBomMaterialCostItemModelMovingPriceDto> FilterModelMovingPriceRows(
        IReadOnlyList<TaktBomMaterialCostItemModelMovingPriceDto> rows,
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
    /// 转置行全量合计：各期间成本 + 环比差额（分页前）
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumTransposedRowGrandTotals(
        IReadOnlyList<TaktBomMaterialCostItemTransposedDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        return SumPeriodAndVarianceGrandTotals(
            periodOrder,
            rows.Select(r => (PeriodMap: (IReadOnlyDictionary<string, decimal>)r.PeriodCosts, r.VarianceAmount)));
    }

    /// <summary>
    /// 产品分析行全量合计
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumComponentMovingPriceRowGrandTotals(
        IReadOnlyList<TaktBomMaterialCostItemComponentMovingPriceDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        return SumPeriodAndVarianceGrandTotals(
            periodOrder,
            rows.Select(r => (PeriodMap: (IReadOnlyDictionary<string, decimal>)r.PeriodMaterialCosts, r.VarianceAmount)));
    }

    /// <summary>
    /// 机种分析行全量合计
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumModelMovingPriceRowGrandTotals(
        IReadOnlyList<TaktBomMaterialCostItemModelMovingPriceDto> rows,
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
    /// 机种合并分析行涨跌优先排序
    /// </summary>
    /// <param name="rows">行</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomMaterialCostItemModelMovingPriceDto> OrderModelMovingPriceRowsByTrend(
        IReadOnlyList<TaktBomMaterialCostItemModelMovingPriceDto> rows)
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
                && x.SequenceNo == entity.SequenceNo
                && x.BomLevel == entity.BomLevel
                && x.BomItemNo == entity.BomItemNo
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
            throw new TaktBusinessException("BOM物料成本明细的PlantCode、ProductCode、SequenceNo、BomLevel、BomItemNo、ComponentCode、ComponentQuantity、BatchIndicator、ProductionRelated、PurchaseType、SpecialProcurementType、CostingDate已存在");
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
    /// <param name="start">起</param>
    /// <param name="end">止</param>
    /// <param name="maxRows">总行上限（可选）</param>
    /// <returns>移动价格列表</returns>
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
