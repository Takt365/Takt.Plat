// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialTransactionItemService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料交易明细应用服务实现
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
/// 物料交易明细应用服务
/// </summary>
public class TaktMaterialTransactionItemService : TaktServiceBase, ITaktMaterialTransactionItemService
{
    private readonly ITaktCompanyRepository<TaktMaterialTransactionItem> _materialTransactionItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialTransaction> _materialTransactionRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialTransactionItemRepository">物料交易明细仓储</param>
    /// <param name="materialTransactionRepository">物料交易仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialTransactionItemService(
        ITaktCompanyRepository<TaktMaterialTransactionItem> materialTransactionItemRepository,
        ITaktCompanyRepository<TaktMaterialTransaction> materialTransactionRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialTransactionItemRepository = materialTransactionItemRepository;
        _materialTransactionRepository = materialTransactionRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料交易明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialTransactionItemDto>> GetMaterialTransactionItemListAsync(TaktMaterialTransactionItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialTransactionItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialTransactionItemDto>.Create(
            data.Adapt<List<TaktMaterialTransactionItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料交易明细
    /// </summary>
    /// <param name="id">物料交易明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionItemDto?> GetMaterialTransactionItemByIdAsync(long id)
    {
        var entity = await _materialTransactionItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialTransactionItemDto>();
    }

    /// <summary>
    /// 获取物料交易明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialTransactionItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _materialTransactionItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建物料交易明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionItemDto> CreateMaterialTransactionItemAsync(TaktMaterialTransactionItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialTransactionItem>();
        await StampMaterialTransactionItemMaterialTransactionAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialTransactionItemRepository,
            x => x.MaterialTransactionId == entity.MaterialTransactionId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique)
        {
            throw new TaktBusinessException("物料交易明细的MaterialTransactionId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _materialTransactionItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialTransactionId == entity.MaterialTransactionId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialTransactionCode) ? entity.MaterialTransactionCode : entity.MaterialTransactionId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _materialTransactionItemRepository.CreateAsync(entity);
        return await GetMaterialTransactionItemByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialTransactionItemDto>();
    }

    /// <summary>
    /// 更新物料交易明细
    /// </summary>
    /// <param name="id">物料交易明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionItemDto> UpdateMaterialTransactionItemAsync(long id, TaktMaterialTransactionItemUpdateDto dto)
    {
        var entity = await _materialTransactionItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料交易明细不存在");
        }
        dto.Adapt(entity);
        await StampMaterialTransactionItemMaterialTransactionAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialTransactionItemRepository,
            x => x.MaterialTransactionId == entity.MaterialTransactionId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique)
        {
            throw new TaktBusinessException("物料交易明细的MaterialTransactionId、LineNumber已存在");
        }
        await _materialTransactionItemRepository.UpdateAsync(entity);
        return await GetMaterialTransactionItemByIdAsync(id) ?? throw new TaktBusinessException("物料交易明细不存在");
    }

    /// <summary>
    /// 删除物料交易明细
    /// </summary>
    /// <param name="id">物料交易明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialTransactionItemByIdAsync(long id)
    {
        var deleted = await _materialTransactionItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料交易明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料交易明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialTransactionItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialTransactionItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialTransactionItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialTransactionItemTemplateDto>(
            sheetName ?? "物料交易明细导入模板",
            fileName ?? "物料交易明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料交易明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialTransactionItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialTransactionItemImportDto>(fileStream, sheetName ?? "物料交易明细导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialTransactionItem>();
                var importDto = rows[i].Adapt<TaktMaterialTransactionItemCreateDto>();
                await StampMaterialTransactionItemMaterialTransactionAsync(entity, importDto);
                var importKey = $"{entity.MaterialTransactionId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialTransactionId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialTransactionItemRepository,
                    x => x.MaterialTransactionId == entity.MaterialTransactionId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique)
                {
                    throw new TaktBusinessException("物料交易明细的MaterialTransactionId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _materialTransactionItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialTransactionId == entity.MaterialTransactionId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialTransactionCode) ? entity.MaterialTransactionCode : entity.MaterialTransactionId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _materialTransactionItemRepository.CreateAsync(entity);
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
    /// 导出物料交易明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialTransactionItemAsync(TaktMaterialTransactionItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialTransactionItemQueryDto());
        var list = await _materialTransactionItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialTransactionItemExportDto>(),
                sheetName ?? "物料交易明细数据",
                fileName ?? "物料交易明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialTransactionItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料交易明细数据",
            fileName ?? "物料交易明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步物料交易明细主表外键（ManyToOne → 物料交易）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaterialTransactionItemMaterialTransactionAsync(TaktMaterialTransactionItem entity, TaktMaterialTransactionItemCreateDto dto)
    {
        if (dto.MaterialTransactionId <= 0)
        {
            return;
        }
        var master = await _materialTransactionRepository.GetByIdAsync(dto.MaterialTransactionId);
        if (master == null)
        {
            throw new TaktBusinessException("物料交易不存在");
        }
        entity.MaterialTransactionId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料交易明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialTransactionItem, bool>> QueryExpression(TaktMaterialTransactionItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialTransactionItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.MaterialTransactionId).Contains(keywords)
                || (x.MaterialTransactionCode != null && x.MaterialTransactionCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.SourceCode != null && x.SourceCode.Contains(keywords))
                || SqlFunc.ToString(x.SourceLineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.TransactionUnit != null && x.TransactionUnit.Contains(keywords))
                || SqlFunc.ToString(x.TransactionQuantity).Contains(keywords)
                || (x.BatchNo != null && x.BatchNo.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.LocationCode != null && x.LocationCode.Contains(keywords))
                || (x.TargetWarehouseCode != null && x.TargetWarehouseCode.Contains(keywords))
                || (x.TargetLocationCode != null && x.TargetLocationCode.Contains(keywords))
                || SqlFunc.ToString(x.UnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.LineAmount).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MaterialTransactionId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialTransactionId == queryDto.MaterialTransactionId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialTransactionCode))
        {
            exp = exp.And(x => x.MaterialTransactionCode != null && x.MaterialTransactionCode.Contains(queryDto.MaterialTransactionCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCode))
        {
            exp = exp.And(x => x.SourceCode != null && x.SourceCode.Contains(queryDto.SourceCode));
        }

        if (queryDto?.SourceLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.SourceLineNumber == queryDto.SourceLineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialName))
        {
            exp = exp.And(x => x.MaterialName != null && x.MaterialName.Contains(queryDto.MaterialName));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialSpecification))
        {
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(queryDto.MaterialSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.TransactionUnit))
        {
            exp = exp.And(x => x.TransactionUnit != null && x.TransactionUnit.Contains(queryDto.TransactionUnit));
        }

        if (queryDto?.TransactionQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TransactionQuantity == queryDto.TransactionQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchNo))
        {
            exp = exp.And(x => x.BatchNo != null && x.BatchNo.Contains(queryDto.BatchNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.WarehouseCode))
        {
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(queryDto.WarehouseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.LocationCode))
        {
            exp = exp.And(x => x.LocationCode != null && x.LocationCode.Contains(queryDto.LocationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetWarehouseCode))
        {
            exp = exp.And(x => x.TargetWarehouseCode != null && x.TargetWarehouseCode.Contains(queryDto.TargetWarehouseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetLocationCode))
        {
            exp = exp.And(x => x.TargetLocationCode != null && x.TargetLocationCode.Contains(queryDto.TargetLocationCode));
        }

        if (queryDto?.UnitPrice.HasValue == true)
        {
            exp = exp.And(x => x.UnitPrice == queryDto.UnitPrice);
        }

        if (queryDto?.LineAmount.HasValue == true)
        {
            exp = exp.And(x => x.LineAmount == queryDto.LineAmount);
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
}
