// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceScaleService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格阶梯应用服务实现
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

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格阶梯应用服务
/// </summary>
public class TaktSalesPriceScaleService : TaktServiceBase, ITaktSalesPriceScaleService
{
    private readonly ITaktCompanyRepository<TaktSalesPriceScale> _salesPriceScaleRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceItem> _salesPriceItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceScaleRepository">销售价格阶梯仓储</param>
    /// <param name="salesPriceItemRepository">销售价格明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceScaleService(
        ITaktCompanyRepository<TaktSalesPriceScale> salesPriceScaleRepository,
        ITaktCompanyRepository<TaktSalesPriceItem> salesPriceItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceScaleRepository = salesPriceScaleRepository;
        _salesPriceItemRepository = salesPriceItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格阶梯列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceScaleDto>> GetSalesPriceScaleListAsync(TaktSalesPriceScaleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceScaleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceScaleDto>.Create(
            data.Adapt<List<TaktSalesPriceScaleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleDto?> GetSalesPriceScaleByIdAsync(long id)
    {
        var entity = await _salesPriceScaleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesPriceScaleDto>();
    }

    /// <summary>
    /// 获取销售价格阶梯选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceScaleOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceScaleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SalesPriceCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SalesPriceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建销售价格阶梯
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleDto> CreateSalesPriceScaleAsync(TaktSalesPriceScaleCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPriceScale>();
        await StampSalesPriceScaleSalesPriceItemAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceScaleRepository,
            x => x.ItemId == entity.ItemId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_price_scale_item_line_unique)
        {
            throw new TaktBusinessException("销售价格阶梯的ItemId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesPriceScaleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ItemId == entity.ItemId,
                x => x.LineNumber);
            var businessCode = entity.ItemId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesPriceScaleRepository.CreateAsync(entity);
        return await GetSalesPriceScaleByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceScaleDto>();
    }

    /// <summary>
    /// 更新销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleDto> UpdateSalesPriceScaleAsync(long id, TaktSalesPriceScaleUpdateDto dto)
    {
        var entity = await _salesPriceScaleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格阶梯不存在");
        }
        dto.Adapt(entity);
        await StampSalesPriceScaleSalesPriceItemAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceScaleRepository,
            x => x.ItemId == entity.ItemId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_price_scale_item_line_unique)
        {
            throw new TaktBusinessException("销售价格阶梯的ItemId、LineNumber已存在");
        }
        await _salesPriceScaleRepository.UpdateAsync(entity);
        return await GetSalesPriceScaleByIdAsync(id) ?? throw new TaktBusinessException("销售价格阶梯不存在");
    }

    /// <summary>
    /// 删除销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceScaleByIdAsync(long id)
    {
        var deleted = await _salesPriceScaleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售价格阶梯不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售价格阶梯
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceScaleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceScaleByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPriceScaleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPriceScaleTemplateDto>(
            sheetName ?? "销售价格阶梯导入模板",
            fileName ?? "销售价格阶梯导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售价格阶梯
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPriceScaleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPriceScaleImportDto>(fileStream, sheetName ?? "销售价格阶梯导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPriceScale>();
                var importDto = rows[i].Adapt<TaktSalesPriceScaleCreateDto>();
                await StampSalesPriceScaleSalesPriceItemAsync(entity, importDto);
                var importKey = $"{entity.ItemId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ItemId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPriceScaleRepository,
                    x => x.ItemId == entity.ItemId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_price_scale_item_line_unique)
                {
                    throw new TaktBusinessException("销售价格阶梯的ItemId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesPriceScaleRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ItemId == entity.ItemId,
                        x => x.LineNumber);
                    var businessCode = entity.ItemId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesPriceScaleRepository.CreateAsync(entity);
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
    /// 导出销售价格阶梯
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceScaleAsync(TaktSalesPriceScaleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPriceScaleQueryDto());
        var list = await _salesPriceScaleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceScaleExportDto>(),
                sheetName ?? "销售价格阶梯数据",
                fileName ?? "销售价格阶梯导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceScaleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格阶梯数据",
            fileName ?? "销售价格阶梯导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步销售价格阶梯主表外键（ManyToOne → 销售价格明细）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSalesPriceScaleSalesPriceItemAsync(TaktSalesPriceScale entity, TaktSalesPriceScaleCreateDto dto)
    {
        if (dto.ItemId <= 0)
        {
            return;
        }
        var master = await _salesPriceItemRepository.GetByIdAsync(dto.ItemId);
        if (master == null)
        {
            throw new TaktBusinessException("销售价格明细不存在");
        }
        entity.ItemId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格阶梯查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPriceScale, bool>> QueryExpression(TaktSalesPriceScaleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPriceScale>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ItemId).Contains(keywords)
                || (x.SalesPriceCode != null && x.SalesPriceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.StartQuantity).Contains(keywords)
                || SqlFunc.ToString(x.EndQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ScalePrice).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ItemId.HasValue == true)
        {
            exp = exp.And(x => x.ItemId == queryDto.ItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPriceCode))
        {
            exp = exp.And(x => x.SalesPriceCode != null && x.SalesPriceCode.Contains(queryDto.SalesPriceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.StartQuantity.HasValue == true)
        {
            exp = exp.And(x => x.StartQuantity == queryDto.StartQuantity);
        }

        if (queryDto?.EndQuantity.HasValue == true)
        {
            exp = exp.And(x => x.EndQuantity == queryDto.EndQuantity);
        }

        if (queryDto?.ScalePrice.HasValue == true)
        {
            exp = exp.And(x => x.ScalePrice == queryDto.ScalePrice);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
}
