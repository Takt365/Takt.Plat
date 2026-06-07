// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceItemService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格明细应用服务实现
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
/// 销售价格明细应用服务
/// </summary>
public class TaktSalesPriceItemService : TaktServiceBase, ITaktSalesPriceItemService
{
    private readonly ITaktCompanyRepository<TaktSalesPriceItem> _salesPriceItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceScale> _salesPriceScaleRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceItemRepository">销售价格明细仓储</param>
    /// <param name="salesPriceScaleRepository">SalesPriceScale仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceItemService(
        ITaktCompanyRepository<TaktSalesPriceItem> salesPriceItemRepository,
        ITaktCompanyRepository<TaktSalesPriceScale> salesPriceScaleRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceItemRepository = salesPriceItemRepository;
        _salesPriceScaleRepository = salesPriceScaleRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceItemDto>> GetSalesPriceItemListAsync(TaktSalesPriceItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceItemDto>.Create(
            data.Adapt<List<TaktSalesPriceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto?> GetSalesPriceItemByIdAsync(long id)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesPriceItemDto>();
        await FillSalesPriceItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售价格明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceItemRepository.GetListAsync(
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
    /// 创建销售价格明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto> CreateSalesPriceItemAsync(TaktSalesPriceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPriceItem>();
        var isUnique_ix_takt_logistics_sales_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceItemRepository,
            x => x.SalesPriceId == entity.SalesPriceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_price_item_price_line_unique)
        {
            throw new TaktBusinessException("销售价格明细的SalesPriceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesPriceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPriceId == entity.SalesPriceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPriceCode) ? entity.SalesPriceCode : entity.SalesPriceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesPriceItemRepository.CreateAsync(entity);
                await SaveSalesPriceItemChildrenAsync(entity, dto);
        return await GetSalesPriceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceItemDto>();
    }

    /// <summary>
    /// 更新销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto> UpdateSalesPriceItemAsync(long id, TaktSalesPriceItemUpdateDto dto)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceItemRepository,
            x => x.SalesPriceId == entity.SalesPriceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_price_item_price_line_unique)
        {
            throw new TaktBusinessException("销售价格明细的SalesPriceId、LineNumber已存在");
        }
        await _salesPriceItemRepository.UpdateAsync(entity);
                await SaveSalesPriceItemChildrenAsync(entity, dto);
        return await GetSalesPriceItemByIdAsync(id) ?? throw new TaktBusinessException("销售价格明细不存在");
    }

    /// <summary>
    /// 删除销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceItemByIdAsync(long id)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格明细不存在或已删除");
        }
        await _salesPriceScaleRepository.DeleteAsync(x => x.ItemId == entity.Id);
        var deleted = await _salesPriceItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售价格明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售价格明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPriceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPriceItemTemplateDto>(
            sheetName ?? "销售价格明细导入模板",
            fileName ?? "销售价格明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售价格明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPriceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPriceItemImportDto>(fileStream, sheetName ?? "销售价格明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPriceItem>();
                var importKey = $"{entity.SalesPriceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesPriceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPriceItemRepository,
                    x => x.SalesPriceId == entity.SalesPriceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_price_item_price_line_unique)
                {
                    throw new TaktBusinessException("销售价格明细的SalesPriceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesPriceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPriceId == entity.SalesPriceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPriceCode) ? entity.SalesPriceCode : entity.SalesPriceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesPriceItemRepository.CreateAsync(entity);
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
    /// 导出销售价格明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceItemAsync(TaktSalesPriceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPriceItemQueryDto());
        var list = await _salesPriceItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceItemExportDto>(),
                sheetName ?? "销售价格明细数据",
                fileName ?? "销售价格明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格明细数据",
            fileName ?? "销售价格明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充销售价格明细详情（加载 OneToMany 子表：销售价格阶梯）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesPriceItemDetailsAsync(TaktSalesPriceItemDto dto, TaktSalesPriceItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售价格阶梯 → dto.Scales
        var scales = await _salesPriceScaleRepository.GetListAsync(x => x.ItemId == entity.Id);
        dto.Scales = scales.Adapt<List<TaktSalesPriceScaleDto>>();
    }

    /// <summary>
    /// 保存销售价格明细子表级联（销售价格阶梯；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesPriceItemChildrenAsync(TaktSalesPriceItem entity, TaktSalesPriceItemCreateDto dto)
    {
        // 销售价格阶梯（Scales）
        if (dto.Scales is not { Count: > 0 })
        {
            await _salesPriceScaleRepository.DeleteAsync(x => x.ItemId == entity.Id);
        }
        else
        {
            var scales = dto.Scales.Adapt<List<TaktSalesPriceScale>>();
            foreach (var child in scales)
            {
                child.ItemId = entity.Id;
            }
            var scalesNeedLine = scales.Where(c => c.LineNumber <= 0).ToList();
            if (scalesNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPriceCode) ? entity.SalesPriceCode : entity.Id.ToString();
                var maxLine = await _salesPriceScaleRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ItemId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, scalesNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in scales)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < scales.Count; i++)
                        {
                            var key = $"{scales[i].CompanyCode}|{scales[i].ItemId}|{scales[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"销售价格阶梯第{i + 1}项与本次提交的其他项重复（CompanyCode、ItemId、LineNumber）");
                            }
                        }
            await _salesPriceScaleRepository.DeleteAsync(x => x.ItemId == entity.Id);
            foreach (var child in scales)
            {
            var isUnique_ix_takt_logistics_sales_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _salesPriceScaleRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ItemId == child.ItemId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_sales_price_scale_item_line_unique)
            {
                throw new TaktBusinessException("销售价格阶梯的CompanyCode、ItemId、LineNumber已存在");
            }
            }
            await _salesPriceScaleRepository.CreateRangeAsync(scales);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPriceItem, bool>> QueryExpression(TaktSalesPriceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPriceItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SalesPriceId).Contains(keywords)
                || (x.SalesPriceCode != null && x.SalesPriceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.SalesUnit != null && x.SalesUnit.Contains(keywords))
                || SqlFunc.ToString(x.SalesPrice).Contains(keywords)
                || SqlFunc.ToString(x.MinOrderQuantity).Contains(keywords)
                || SqlFunc.ToString(x.MaxOrderQuantity).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SalesPriceId.HasValue == true)
        {
            exp = exp.And(x => x.SalesPriceId == queryDto.SalesPriceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPriceCode))
        {
            exp = exp.And(x => x.SalesPriceCode != null && x.SalesPriceCode.Contains(queryDto.SalesPriceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesUnit))
        {
            exp = exp.And(x => x.SalesUnit != null && x.SalesUnit.Contains(queryDto.SalesUnit));
        }

        if (queryDto?.SalesPrice.HasValue == true)
        {
            exp = exp.And(x => x.SalesPrice == queryDto.SalesPrice);
        }

        if (queryDto?.MinOrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MinOrderQuantity == queryDto.MinOrderQuantity);
        }

        if (queryDto?.MaxOrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MaxOrderQuantity == queryDto.MaxOrderQuantity);
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
