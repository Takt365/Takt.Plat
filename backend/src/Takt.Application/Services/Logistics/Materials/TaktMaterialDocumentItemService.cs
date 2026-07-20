// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemService.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：物料凭证行项目应用服务实现
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
/// 物料凭证行项目应用服务
/// </summary>
public class TaktMaterialDocumentItemService : TaktServiceBase, ITaktMaterialDocumentItemService
{
    private readonly ITaktCompanyRepository<TaktMaterialDocumentItem> _materialDocumentItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialDocument> _materialDocumentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDocumentItemRepository">物料凭证行项目仓储</param>
    /// <param name="materialDocumentRepository">物料凭证仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialDocumentItemService(
        ITaktCompanyRepository<TaktMaterialDocumentItem> materialDocumentItemRepository,
        ITaktCompanyRepository<TaktMaterialDocument> materialDocumentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialDocumentItemRepository = materialDocumentItemRepository;
        _materialDocumentRepository = materialDocumentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料凭证行项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDocumentItemDto>> GetMaterialDocumentItemListAsync(TaktMaterialDocumentItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialDocumentItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDocumentItemDto>.Create(
            data.Adapt<List<TaktMaterialDocumentItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto?> GetMaterialDocumentItemByIdAsync(long id)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialDocumentItemDto>();
    }

    /// <summary>
    /// 获取物料凭证明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialDocumentItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _materialDocumentItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialDocumentCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrWhiteSpace(e.MaterialDocumentCode)
                ? e.Id.ToString()
                : $"{e.MaterialDocumentCode}-{e.LineNumber}",
        }).ToList();
    }

    /// <summary>
    /// 创建物料凭证行项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto> CreateMaterialDocumentItemAsync(TaktMaterialDocumentItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialDocumentItem>();
        entity.IsObsolete = 0;
        await StampMaterialDocumentItemMaterialDocumentAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentItemRepository,
            x => x.MaterialDocumentId == entity.MaterialDocumentId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_materials_material_document_item_line_unique)
        {
            throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _materialDocumentItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialDocumentId == entity.MaterialDocumentId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialDocumentCode) ? entity.MaterialDocumentCode : entity.MaterialDocumentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _materialDocumentItemRepository.CreateAsync(entity);
        return await GetMaterialDocumentItemByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDocumentItemDto>();
    }

    /// <summary>
    /// 更新物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto> UpdateMaterialDocumentItemAsync(long id, TaktMaterialDocumentItemUpdateDto dto)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证行项目不存在");
        }
        dto.Adapt(entity);
        await StampMaterialDocumentItemMaterialDocumentAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentItemRepository,
            x => x.MaterialDocumentId == entity.MaterialDocumentId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_document_item_line_unique)
        {
            throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
        }
        await _materialDocumentItemRepository.UpdateAsync(entity);
        return await GetMaterialDocumentItemByIdAsync(id) ?? throw new TaktBusinessException("物料凭证行项目不存在");
    }

    /// <summary>
    /// 删除物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentItemByIdAsync(long id)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证行项目不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料凭证行项目不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("物料凭证行项目已作废");
        }
        entity.IsObsolete = 1;
        await _materialDocumentItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除物料凭证行项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialDocumentItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料凭证行项目作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto> UpdateMaterialDocumentItemObsoleteAsync(TaktMaterialDocumentItemObsoleteDto dto)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(dto.MaterialDocumentItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证行项目不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料凭证行项目不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _materialDocumentItemRepository.UpdateAsync(entity);
        return await GetMaterialDocumentItemByIdAsync(dto.MaterialDocumentItemId) ?? throw new TaktBusinessException("物料凭证行项目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialDocumentItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialDocumentItemTemplateDto>(
            sheetName ?? "物料凭证行项目导入模板",
            fileName ?? "物料凭证行项目导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料凭证行项目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialDocumentItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialDocumentItemImportDto>(fileStream, sheetName ?? "物料凭证行项目导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialDocumentItem>();
                var importDto = rows[i].Adapt<TaktMaterialDocumentItemCreateDto>();
                await StampMaterialDocumentItemMaterialDocumentAsync(entity, importDto);
                var importKey = $"{entity.MaterialDocumentId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialDocumentId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialDocumentItemRepository,
                    x => x.MaterialDocumentId == entity.MaterialDocumentId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_materials_material_document_item_line_unique)
                {
                    throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _materialDocumentItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialDocumentId == entity.MaterialDocumentId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialDocumentCode) ? entity.MaterialDocumentCode : entity.MaterialDocumentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _materialDocumentItemRepository.CreateAsync(entity);
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
    /// 导出物料凭证行项目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialDocumentItemAsync(TaktMaterialDocumentItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialDocumentItemQueryDto());
        var list = await _materialDocumentItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDocumentItemExportDto>(),
                sheetName ?? "物料凭证行项目数据",
                fileName ?? "物料凭证行项目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialDocumentItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料凭证行项目数据",
            fileName ?? "物料凭证行项目导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步物料凭证行项目主表外键（ManyToOne → 物料凭证）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaterialDocumentItemMaterialDocumentAsync(TaktMaterialDocumentItem entity, TaktMaterialDocumentItemCreateDto dto)
    {
        if (dto.MaterialDocumentId <= 0)
        {
            return;
        }
        var master = await _materialDocumentRepository.GetByIdAsync(dto.MaterialDocumentId);
        if (master == null)
        {
            throw new TaktBusinessException("物料凭证不存在");
        }
        entity.MaterialDocumentId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料凭证行项目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialDocumentItem, bool>> QueryExpression(TaktMaterialDocumentItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialDocumentItem>();

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
                SqlFunc.ToString(x.MaterialDocumentId).Contains(keywords)
                || (x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.MovementType != null && x.MovementType.Contains(keywords))
                || SqlFunc.ToString(x.Quantity).Contains(keywords)
                || (x.SpecialStock != null && x.SpecialStock.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.ProductionOrderCode != null && x.ProductionOrderCode.Contains(keywords))
                || (x.ProjectCode != null && x.ProjectCode.Contains(keywords))
                || SqlFunc.ToString(x.LocalCurrencyAmount).Contains(keywords)
                || (x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PostingDate).Contains(keywords)
                || SqlFunc.ToString(x.DocumentDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MaterialDocumentId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialDocumentId == queryDto.MaterialDocumentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDocumentCode))
        {
            exp = exp.And(x => x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(queryDto.MaterialDocumentCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.WarehouseCode))
        {
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(queryDto.WarehouseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MovementType))
        {
            exp = exp.And(x => x.MovementType != null && x.MovementType.Contains(queryDto.MovementType));
        }

        if (queryDto?.Quantity.HasValue == true)
        {
            exp = exp.And(x => x.Quantity == queryDto.Quantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.SpecialStock))
        {
            exp = exp.And(x => x.SpecialStock != null && x.SpecialStock.Contains(queryDto.SpecialStock));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseOrderCode))
        {
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(queryDto.PurchaseOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionOrderCode))
        {
            exp = exp.And(x => x.ProductionOrderCode != null && x.ProductionOrderCode.Contains(queryDto.ProductionOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProjectCode))
        {
            exp = exp.And(x => x.ProjectCode != null && x.ProjectCode.Contains(queryDto.ProjectCode));
        }

        if (queryDto?.LocalCurrencyAmount.HasValue == true)
        {
            exp = exp.And(x => x.LocalCurrencyAmount == queryDto.LocalCurrencyAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.ReferenceDocumentCode))
        {
            exp = exp.And(x => x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(queryDto.ReferenceDocumentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PostingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PostingDate >= queryDto.PostingDateStart);
        }

        if (queryDto?.PostingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PostingDate <= queryDto.PostingDateEnd);
        }

        if (queryDto?.DocumentDateStart.HasValue == true)
        {
            exp = exp.And(x => x.DocumentDate >= queryDto.DocumentDateStart);
        }

        if (queryDto?.DocumentDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.DocumentDate <= queryDto.DocumentDateEnd);
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
