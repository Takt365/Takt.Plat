// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPurchasePriceScaleService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格阶梯应用服务实现
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

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 采购价格阶梯应用服务
/// </summary>
public class TaktPurchasePriceScaleService : TaktServiceBase, ITaktPurchasePriceScaleService
{
    private readonly ITaktCompanyRepository<TaktPurchasePriceScale> _purchasePriceScaleRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceScaleRepository">采购价格阶梯仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceScaleService(
        ITaktCompanyRepository<TaktPurchasePriceScale> purchasePriceScaleRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceScaleRepository = purchasePriceScaleRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购价格阶梯列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePriceScaleDto>> GetPurchasePriceScaleListAsync(TaktPurchasePriceScaleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePriceScaleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePriceScaleDto>.Create(
            data.Adapt<List<TaktPurchasePriceScaleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleDto?> GetPurchasePriceScaleByIdAsync(long id)
    {
        var entity = await _purchasePriceScaleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchasePriceScaleDto>();
    }

    /// <summary>
    /// 获取采购价格阶梯选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceScaleOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePriceScaleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PurchasePriceCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PurchasePriceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购价格阶梯
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleDto> CreatePurchasePriceScaleAsync(TaktPurchasePriceScaleCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePriceScale>();
        var isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceScaleRepository,
            x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                && x.LineNumber == entity.LineNumber
                && x.StartQuantity == entity.StartQuantity);
        if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique)
        {
            throw new TaktBusinessException("采购价格阶梯的PurchasePriceItemId、LineNumber、StartQuantity已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _purchasePriceScaleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceItemId == entity.PurchasePriceItemId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PurchasePriceItemId, maxSort);
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchasePriceScaleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceItemId == entity.PurchasePriceItemId,
                x => x.LineNumber);
            var businessCode = entity.PurchasePriceItemId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchasePriceScaleRepository.CreateAsync(entity);
        return await GetPurchasePriceScaleByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePriceScaleDto>();
    }

    /// <summary>
    /// 更新采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleDto> UpdatePurchasePriceScaleAsync(long id, TaktPurchasePriceScaleUpdateDto dto)
    {
        var entity = await _purchasePriceScaleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格阶梯不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceScaleRepository,
            x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                && x.LineNumber == entity.LineNumber
                && x.StartQuantity == entity.StartQuantity,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique)
        {
            throw new TaktBusinessException("采购价格阶梯的PurchasePriceItemId、LineNumber、StartQuantity已存在");
        }
        await _purchasePriceScaleRepository.UpdateAsync(entity);
        return await GetPurchasePriceScaleByIdAsync(id) ?? throw new TaktBusinessException("采购价格阶梯不存在");
    }

    /// <summary>
    /// 删除采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceScaleByIdAsync(long id)
    {
        var deleted = await _purchasePriceScaleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购价格阶梯不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购价格阶梯
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceScaleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePriceScaleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购价格阶梯排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleDto> UpdatePurchasePriceScaleSortAsync(TaktPurchasePriceScaleSortDto dto)
    {
        var entity = await _purchasePriceScaleRepository.GetByIdAsync(dto.PurchasePriceScaleId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格阶梯不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _purchasePriceScaleRepository.UpdateAsync(entity);
        return await GetPurchasePriceScaleByIdAsync(dto.PurchasePriceScaleId) ?? throw new TaktBusinessException("采购价格阶梯不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePriceScaleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePriceScaleTemplateDto>(
            sheetName ?? "采购价格阶梯导入模板",
            fileName ?? "采购价格阶梯导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购价格阶梯
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePriceScaleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePriceScaleImportDto>(fileStream, sheetName ?? "采购价格阶梯导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePriceScale>();
                var importKey = $"{entity.PurchasePriceItemId}|{entity.LineNumber}|{entity.StartQuantity}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasePriceItemId、LineNumber、StartQuantity）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePriceScaleRepository,
                    x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                        && x.LineNumber == entity.LineNumber
                        && x.StartQuantity == entity.StartQuantity);
                if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique)
                {
                    throw new TaktBusinessException("采购价格阶梯的PurchasePriceItemId、LineNumber、StartQuantity已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _purchasePriceScaleRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceItemId == entity.PurchasePriceItemId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PurchasePriceItemId, maxSort);
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchasePriceScaleRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceItemId == entity.PurchasePriceItemId,
                        x => x.LineNumber);
                    var businessCode = entity.PurchasePriceItemId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchasePriceScaleRepository.CreateAsync(entity);
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
    /// 导出采购价格阶梯
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceScaleAsync(TaktPurchasePriceScaleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchasePriceScaleQueryDto());
        var list = await _purchasePriceScaleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceScaleExportDto>(),
                sheetName ?? "采购价格阶梯数据",
                fileName ?? "采购价格阶梯导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePriceScaleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购价格阶梯数据",
            fileName ?? "采购价格阶梯导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购价格阶梯查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePriceScale, bool>> QueryExpression(TaktPurchasePriceScaleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePriceScale>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PurchasePriceItemId).Contains(keywords)
                || (x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.StartQuantity).Contains(keywords)
                || SqlFunc.ToString(x.EndQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ScalePrice).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchasePriceItemId.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePriceItemId == queryDto.PurchasePriceItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasePriceCode))
        {
            exp = exp.And(x => x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(queryDto.PurchasePriceCode));
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

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
