// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格应用服务
/// </summary>
public class TaktSalesPriceService : TaktServiceBase, ITaktSalesPriceService
{
    private readonly ITaktCompanyRepository<TaktSalesPrice> _salesPriceRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceItem> _salesPriceItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceChangeLog> _salesPriceChangeLogRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceRepository">销售价格仓储</param>
    /// <param name="salesPriceItemRepository">SalesPriceItem仓储</param>
    /// <param name="salesPriceChangeLogRepository">SalesPriceChangeLog仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceService(
        ITaktCompanyRepository<TaktSalesPrice> salesPriceRepository,
        ITaktCompanyRepository<TaktSalesPriceItem> salesPriceItemRepository,
        ITaktCompanyRepository<TaktSalesPriceChangeLog> salesPriceChangeLogRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceRepository = salesPriceRepository;
        _salesPriceItemRepository = salesPriceItemRepository;
        _salesPriceChangeLogRepository = salesPriceChangeLogRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceDto>> GetSalesPriceListAsync(TaktSalesPriceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceDto>.Create(
            data.Adapt<List<TaktSalesPriceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto?> GetSalesPriceByIdAsync(long id)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesPriceDto>();
        await FillSalesPriceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建销售价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto> CreateSalesPriceAsync(TaktSalesPriceCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPrice>();
        var isUnique_ix_takt_logistics_sales_price_sp_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesPriceCode == entity.SalesPriceCode
                && x.CustomerCode == entity.CustomerCode
                && x.PriceType == entity.PriceType
                && x.EffectiveStartDate == entity.EffectiveStartDate);
        if (!isUnique_ix_takt_logistics_sales_price_sp_unique)
        {
            throw new TaktBusinessException("销售价格的PlantCode、SalesPriceCode、CustomerCode、PriceType、EffectiveStartDate已存在");
        }
        entity = await _salesPriceRepository.CreateAsync(entity);
                await SaveSalesPriceChildrenAsync(entity, dto);
        return await GetSalesPriceByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceDto>();
    }

    /// <summary>
    /// 更新销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto> UpdateSalesPriceAsync(long id, TaktSalesPriceUpdateDto dto)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_price_sp_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesPriceCode == entity.SalesPriceCode
                && x.CustomerCode == entity.CustomerCode
                && x.PriceType == entity.PriceType
                && x.EffectiveStartDate == entity.EffectiveStartDate,
            id);
        if (!isUnique_ix_takt_logistics_sales_price_sp_unique)
        {
            throw new TaktBusinessException("销售价格的PlantCode、SalesPriceCode、CustomerCode、PriceType、EffectiveStartDate已存在");
        }
        await _salesPriceRepository.UpdateAsync(entity);
                await SaveSalesPriceChildrenAsync(entity, dto);
        return await GetSalesPriceByIdAsync(id) ?? throw new TaktBusinessException("销售价格不存在");
    }

    /// <summary>
    /// 删除销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceByIdAsync(long id)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格不存在或已删除");
        }
        await _salesPriceItemRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
        await _salesPriceChangeLogRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
        var deleted = await _salesPriceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售价格不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售价格状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto> UpdateSalesPriceStatusAsync(TaktSalesPriceStatusDto dto)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(dto.SalesPriceId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格不存在");
        }
        entity.PriceStatus = dto.PriceStatus;
        await _salesPriceRepository.UpdateAsync(entity);
        return await GetSalesPriceByIdAsync(dto.SalesPriceId) ?? throw new TaktBusinessException("销售价格不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPriceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPriceTemplateDto>(
            sheetName ?? "销售价格导入模板",
            fileName ?? "销售价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPriceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPriceImportDto>(fileStream, sheetName ?? "销售价格导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPrice>();
                var importKey = $"{entity.PlantCode}|{entity.SalesPriceCode}|{entity.CustomerCode}|{entity.PriceType}|{entity.EffectiveStartDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesPriceCode、CustomerCode、PriceType、EffectiveStartDate）");
                }
                var isUnique_ix_takt_logistics_sales_price_sp_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPriceRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesPriceCode == entity.SalesPriceCode
                        && x.CustomerCode == entity.CustomerCode
                        && x.PriceType == entity.PriceType
                        && x.EffectiveStartDate == entity.EffectiveStartDate);
                if (!isUnique_ix_takt_logistics_sales_price_sp_unique)
                {
                    throw new TaktBusinessException("销售价格的PlantCode、SalesPriceCode、CustomerCode、PriceType、EffectiveStartDate已存在");
                }
                await _salesPriceRepository.CreateAsync(entity);
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
    /// 导出销售价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceAsync(TaktSalesPriceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPriceQueryDto());
        var list = await _salesPriceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceExportDto>(),
                sheetName ?? "销售价格数据",
                fileName ?? "销售价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格数据",
            fileName ?? "销售价格导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充销售价格详情（加载 OneToMany 子表：销售价格明细、销售价格变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesPriceDetailsAsync(TaktSalesPriceDto dto, TaktSalesPrice entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售价格明细 → dto.Items
        var items = await _salesPriceItemRepository.GetListAsync(x => x.SalesPriceId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesPriceItemDto>>();
        // 销售价格变更记录 → dto.ChangeLogs
        var changelogs = await _salesPriceChangeLogRepository.GetListAsync(x => x.SalesPriceId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktSalesPriceChangeLogDto>>();
    }

    /// <summary>
    /// 保存销售价格子表级联（销售价格明细、销售价格变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesPriceChildrenAsync(TaktSalesPrice entity, TaktSalesPriceCreateDto dto)
    {
        // 销售价格明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _salesPriceItemRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktSalesPriceItem>>();
            foreach (var child in items)
            {
                child.SalesPriceId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPriceCode) ? entity.SalesPriceCode : entity.Id.ToString();
                var maxLine = await _salesPriceItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPriceId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].SalesPriceId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"销售价格明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesPriceId、LineNumber）");
                            }
                        }
            await _salesPriceItemRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_sales_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
                _salesPriceItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.SalesPriceId == child.SalesPriceId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_sales_price_item_price_line_unique)
            {
                throw new TaktBusinessException("销售价格明细的CompanyCode、SalesPriceId、LineNumber已存在");
            }
            }
            await _salesPriceItemRepository.CreateRangeAsync(items);
        }
        // 销售价格变更记录（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _salesPriceChangeLogRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktSalesPriceChangeLog>>();
            foreach (var child in changelogs)
            {
                child.SalesPriceId = entity.Id;
            }
            await _salesPriceChangeLogRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _salesPriceChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPrice, bool>> QueryExpression(TaktSalesPriceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPrice>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesPriceCode != null && x.SalesPriceCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || SqlFunc.ToString(x.PriceType).Contains(keywords)
                || SqlFunc.ToString(x.PriceStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveStartDate).Contains(keywords)
                || SqlFunc.ToString(x.EffectiveEndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPriceCode))
        {
            exp = exp.And(x => x.SalesPriceCode != null && x.SalesPriceCode.Contains(queryDto.SalesPriceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (queryDto?.PriceType.HasValue == true)
        {
            exp = exp.And(x => x.PriceType == queryDto.PriceType);
        }

        if (queryDto?.PriceStatus.HasValue == true)
        {
            exp = exp.And(x => x.PriceStatus == queryDto.PriceStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveStartDate >= queryDto.EffectiveStartDateStart);
        }

        if (queryDto?.EffectiveStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveStartDate <= queryDto.EffectiveStartDateEnd);
        }

        if (queryDto?.EffectiveEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveEndDate >= queryDto.EffectiveEndDateStart);
        }

        if (queryDto?.EffectiveEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveEndDate <= queryDto.EffectiveEndDateEnd);
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
