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
    /// <summary>移动价格按年分表基表名</summary>
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    /// <summary>按 Id 探测年分表年数</summary>
    private const int YearShardProbeYears = 6;

    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingPriceRepository">移动价格仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialMovingPriceService(
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialMovingPriceRepository = materialMovingPriceRepository;
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
            yearTable = null;
            if (!string.IsNullOrWhiteSpace(queryDto.ValuationPeriod))
            {
                yearTable = await ResolveMovingPricePhysicalTableAsync(ParseValuationPeriodYear(queryDto.ValuationPeriod));
            }
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
        var yearTable = await ResolveMovingPricePhysicalTableAsync(ParseValuationPeriodYear(entity.ValuationPeriod));
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
        var originalYear = ParseValuationPeriodYear(entity.ValuationPeriod);
        dto.Adapt(entity);
        if (ParseValuationPeriodYear(entity.ValuationPeriod) != originalYear)
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
                var importKey = $"{entity.PlantCode}|{entity.ValuationPeriod}|{entity.MaterialCode}|{entity.Valuation}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ValuationPeriod、MaterialCode、Valuation）");
                }
                var yearTable = await ResolveMovingPricePhysicalTableAsync(ParseValuationPeriodYear(entity.ValuationPeriod));
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
        DateTime? exportStart = null;
        DateTime? exportEnd = null;
        if (!string.IsNullOrWhiteSpace(query.ValuationPeriod))
        {
            exportStart = ParseYmToDateRequired(query.ValuationPeriod);
            exportEnd = exportStart;
        }
        var list = await GetMovingPriceListForRangeAsync(predicate, exportStart, exportEnd);
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
    // 查询表达式
    // ========================================

    /// <summary>
    /// 默认评估期间（上月，yyyy-MM）
    /// </summary>
    /// <returns>期间字符串</returns>
    private static string GetDefaultValuationPeriod()
    {
        var d = DateTime.Today.AddMonths(-1);
        return d.ToString("yyyy-MM");
    }

    /// <summary>
    /// 是否存在除评估期间外的查询条件（有参则不强制上月期间）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有其它条件为 true</returns>
    private static bool HasFiltersBesidesValuationPeriod(TaktMaterialMovingPriceQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        return !string.IsNullOrWhiteSpace(queryDto.KeyWords)
            || !string.IsNullOrWhiteSpace(queryDto.PlantCode)
            || !string.IsNullOrWhiteSpace(queryDto.MaterialCode)
            || !string.IsNullOrWhiteSpace(queryDto.Valuation)
            || !string.IsNullOrWhiteSpace(queryDto.PriceControl)
            || !string.IsNullOrWhiteSpace(queryDto.CurrencyCode)
            || !string.IsNullOrWhiteSpace(queryDto.ExtField)
            || !string.IsNullOrWhiteSpace(queryDto.Remark)
            || queryDto.StockQuantity.HasValue
            || queryDto.StockAmount.HasValue
            || queryDto.MovingPrice.HasValue
            || queryDto.PriceUnit.HasValue
            || queryDto.CreatedAtStart.HasValue
            || queryDto.CreatedAtEnd.HasValue;
    }

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
                || (x.PriceControl != null && x.PriceControl.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        // 有评估期间 → 按参精确匹配；无期间且无其它条件 → 默认上月；有其它条件 → 不绑期间
        var valuationPeriod = queryDto?.ValuationPeriod?.Trim();
        if (!string.IsNullOrEmpty(valuationPeriod))
        {
            exp = exp.And(x => x.ValuationPeriod == valuationPeriod);
        }
        else if (!HasFiltersBesidesValuationPeriod(queryDto))
        {
            var defaultPeriod = GetDefaultValuationPeriod();
            exp = exp.And(x => x.ValuationPeriod == defaultPeriod);
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

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
    /// 解析评估期间年份（yyyy-MM）
    /// </summary>
    private static int ParseValuationPeriodYear(string? valuationPeriod)
    {
        var ym = NormalizeYm(valuationPeriod);
        if (ym.Length < 4 || !int.TryParse(ym.AsSpan(0, 4), out var year))
        {
            throw new TaktBusinessException("评估期间格式无效，须为 yyyy-MM");
        }
        return year;
    }

    /// <summary>
    /// 规范化 yyyy-MM
    /// </summary>
    private static string NormalizeYm(string? valuationPeriod)
    {
        var v = (valuationPeriod ?? string.Empty).Trim();
        return v.Length >= 7 ? v.Substring(0, 7) : v;
    }

    /// <summary>
    /// yyyy-MM → 当月首日
    /// </summary>
    private static DateTime ParseYmToDateRequired(string? valuationPeriod)
    {
        var ym = NormalizeYm(valuationPeriod);
        if (!DateTime.TryParseExact(ym + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var d))
        {
            throw new TaktBusinessException("评估期间格式无效，须为 yyyy-MM");
        }
        return d;
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
                && x.ValuationPeriod == entity.ValuationPeriod
                && x.MaterialCode == entity.MaterialCode
                && x.Valuation == entity.Valuation,
            yearTable);
        if (existing != null && (!excludeId.HasValue || existing.Id != excludeId.Value))
        {
            throw new TaktBusinessException("移动价格的PlantCode、ValuationPeriod、MaterialCode、Valuation已存在");
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
            result.AddRange(basePart.Where(r => yearSet.Contains(ParseValuationPeriodYear(r.ValuationPeriod))));
        }
        return result;
    }
}
