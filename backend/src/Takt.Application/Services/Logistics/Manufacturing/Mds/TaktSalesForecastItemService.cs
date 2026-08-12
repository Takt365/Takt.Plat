// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastItemService.cs
// 创建时间：2026-07-29
// 创建人：Takt365(Cursor AI)
// 功能描述：销售预测明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mds;

/// <summary>
/// 销售预测明细应用服务
/// </summary>
public class TaktSalesForecastItemService : TaktServiceBase, ITaktSalesForecastItemService
{
    private readonly ITaktCompanyRepository<TaktSalesForecastItem> _salesForecastItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesForecastItemRepository">销售预测明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesForecastItemService(
        ITaktCompanyRepository<TaktSalesForecastItem> salesForecastItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesForecastItemRepository = salesForecastItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售预测明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesForecastItemDto>> GetSalesForecastItemListAsync(TaktSalesForecastItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesForecastItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesForecastItemDto>.Create(
            data.Adapt<List<TaktSalesForecastItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售预测明细
    /// </summary>
    /// <param name="id">销售预测明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastItemDto?> GetSalesForecastItemByIdAsync(long id)
    {
        var entity = await _salesForecastItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesForecastItemDto>();
    }

    /// <summary>
    /// 获取销售预测明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesForecastItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesForecastItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.SalesForecastCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesForecastCode,
            DictLabel = e.SalesForecastCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售预测明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastItemDto> CreateSalesForecastItemAsync(TaktSalesForecastItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesForecastItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_item_month_unique = await _uniqueValidator.IsUniqueAsync(
            _salesForecastItemRepository,
            x => x.SalesForecastId == entity.SalesForecastId
                && x.FiscalYear == entity.FiscalYear
                && x.PlanMonth == entity.PlanMonth);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_item_month_unique)
        {
            throw new TaktBusinessException("销售预测明细的SalesForecastId、FiscalYear、PlanMonth已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesForecastItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesForecastId == entity.SalesForecastId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SalesForecastCode) ? entity.SalesForecastCode : entity.SalesForecastId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesForecastItemRepository.CreateAsync(entity);
        return await GetSalesForecastItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesForecastItemDto>();
    }

    /// <summary>
    /// 更新销售预测明细
    /// </summary>
    /// <param name="id">销售预测明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastItemDto> UpdateSalesForecastItemAsync(long id, TaktSalesForecastItemUpdateDto dto)
    {
        var entity = await _salesForecastItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售预测明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_item_month_unique = await _uniqueValidator.IsUniqueAsync(
            _salesForecastItemRepository,
            x => x.SalesForecastId == entity.SalesForecastId
                && x.FiscalYear == entity.FiscalYear
                && x.PlanMonth == entity.PlanMonth,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_item_month_unique)
        {
            throw new TaktBusinessException("销售预测明细的SalesForecastId、FiscalYear、PlanMonth已存在");
        }
        await _salesForecastItemRepository.UpdateAsync(entity);
        return await GetSalesForecastItemByIdAsync(id) ?? throw new TaktBusinessException("销售预测明细不存在");
    }

    /// <summary>
    /// 删除销售预测明细
    /// </summary>
    /// <param name="id">销售预测明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesForecastItemByIdAsync(long id)
    {
        var entity = await _salesForecastItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售预测明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售预测明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("销售预测明细已作废");
        }
        entity.IsObsolete = 1;
        await _salesForecastItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除销售预测明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesForecastItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesForecastItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售预测明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastItemDto> UpdateSalesForecastItemObsoleteAsync(TaktSalesForecastItemObsoleteDto dto)
    {
        var entity = await _salesForecastItemRepository.GetByIdAsync(dto.SalesForecastItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售预测明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售预测明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesForecastItemRepository.UpdateAsync(entity);
        return await GetSalesForecastItemByIdAsync(dto.SalesForecastItemId) ?? throw new TaktBusinessException("销售预测明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesForecastItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesForecastItemTemplateDto>(
            sheetName ?? "销售预测明细导入模板",
            fileName ?? "销售预测明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售预测明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesForecastItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesForecastItemImportDto>(fileStream, sheetName ?? "销售预测明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesForecastItem>();
                var importKey = $"{entity.SalesForecastId}|{entity.FiscalYear}|{entity.PlanMonth}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesForecastId、FiscalYear、PlanMonth）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_item_month_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesForecastItemRepository,
                    x => x.SalesForecastId == entity.SalesForecastId
                        && x.FiscalYear == entity.FiscalYear
                        && x.PlanMonth == entity.PlanMonth);
                if (!isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_item_month_unique)
                {
                    throw new TaktBusinessException("销售预测明细的SalesForecastId、FiscalYear、PlanMonth已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesForecastItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesForecastId == entity.SalesForecastId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesForecastCode) ? entity.SalesForecastCode : entity.SalesForecastId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesForecastItemRepository.CreateAsync(entity);
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
    /// 导出销售预测明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesForecastItemAsync(TaktSalesForecastItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesForecastItemQueryDto());
        var list = await _salesForecastItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesForecastItemExportDto>(),
                sheetName ?? "销售预测明细数据",
                fileName ?? "销售预测明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesForecastItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售预测明细数据",
            fileName ?? "销售预测明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售预测明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesForecastItem, bool>> QueryExpression(TaktSalesForecastItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesForecastItem>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SalesForecastId).Contains(keywords)
                || (x.SalesForecastCode != null && x.SalesForecastCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.FiscalYear != null && x.FiscalYear.Contains(keywords))
                || SqlFunc.ToString(x.PlanMonth).Contains(keywords)
                || SqlFunc.ToString(x.PlanQuantity001).Contains(keywords)
                || SqlFunc.ToString(x.PlanQuantity002).Contains(keywords)
                || SqlFunc.ToString(x.PlanQuantityDelta).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedUnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedAmount).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SalesForecastId.HasValue == true)
        {
            exp = exp.And(x => x.SalesForecastId == queryDto.SalesForecastId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesForecastCode))
        {
            exp = exp.And(x => x.SalesForecastCode != null && x.SalesForecastCode.Contains(queryDto.SalesForecastCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.FiscalYear))
        {
            exp = exp.And(x => x.FiscalYear != null && x.FiscalYear.Contains(queryDto.FiscalYear));
        }

        if (queryDto?.PlanMonth.HasValue == true)
        {
            exp = exp.And(x => x.PlanMonth == queryDto.PlanMonth);
        }

        if (queryDto?.PlanQuantity001.HasValue == true)
        {
            exp = exp.And(x => x.PlanQuantity001 == queryDto.PlanQuantity001);
        }

        if (queryDto?.PlanQuantity002.HasValue == true)
        {
            exp = exp.And(x => x.PlanQuantity002 == queryDto.PlanQuantity002);
        }

        if (queryDto?.PlanQuantityDelta.HasValue == true)
        {
            exp = exp.And(x => x.PlanQuantityDelta == queryDto.PlanQuantityDelta);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedQuantity == queryDto.ConvertedQuantity);
        }

        if (queryDto?.EstimatedUnitPrice.HasValue == true)
        {
            exp = exp.And(x => x.EstimatedUnitPrice == queryDto.EstimatedUnitPrice);
        }

        if (queryDto?.EstimatedAmount.HasValue == true)
        {
            exp = exp.And(x => x.EstimatedAmount == queryDto.EstimatedAmount);
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
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
