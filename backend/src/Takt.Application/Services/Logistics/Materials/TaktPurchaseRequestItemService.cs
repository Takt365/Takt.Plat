// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPurchaseRequestItemService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：采购申请明细应用服务实现
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
/// 采购申请明细应用服务
/// </summary>
public class TaktPurchaseRequestItemService : TaktServiceBase, ITaktPurchaseRequestItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseRequestItem> _purchaseRequestItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseRequestItemRepository">采购申请明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseRequestItemService(
        ITaktCompanyRepository<TaktPurchaseRequestItem> purchaseRequestItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseRequestItemRepository = purchaseRequestItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购申请明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseRequestItemDto>> GetPurchaseRequestItemListAsync(TaktPurchaseRequestItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseRequestItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseRequestItemDto>.Create(
            data.Adapt<List<TaktPurchaseRequestItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购申请明细
    /// </summary>
    /// <param name="id">采购申请明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto?> GetPurchaseRequestItemByIdAsync(long id)
    {
        var entity = await _purchaseRequestItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseRequestItemDto>();
    }

    /// <summary>
    /// 获取采购申请明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseRequestItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchaseRequestItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购申请明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto> CreatePurchaseRequestItemAsync(TaktPurchaseRequestItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseRequestItem>();
        var isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestItemRepository,
            x => x.PurchaseRequestId == entity.PurchaseRequestId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique)
        {
            throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseRequestItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseRequestId == entity.PurchaseRequestId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseRequestCode) ? entity.PurchaseRequestCode : entity.PurchaseRequestId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseRequestItemRepository.CreateAsync(entity);
        return await GetPurchaseRequestItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseRequestItemDto>();
    }

    /// <summary>
    /// 更新采购申请明细
    /// </summary>
    /// <param name="id">采购申请明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto> UpdatePurchaseRequestItemAsync(long id, TaktPurchaseRequestItemUpdateDto dto)
    {
        var entity = await _purchaseRequestItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestItemRepository,
            x => x.PurchaseRequestId == entity.PurchaseRequestId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique)
        {
            throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
        }
        await _purchaseRequestItemRepository.UpdateAsync(entity);
        return await GetPurchaseRequestItemByIdAsync(id) ?? throw new TaktBusinessException("采购申请明细不存在");
    }

    /// <summary>
    /// 删除采购申请明细
    /// </summary>
    /// <param name="id">采购申请明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestItemByIdAsync(long id)
    {
        var deleted = await _purchaseRequestItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购申请明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购申请明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseRequestItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseRequestItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseRequestItemTemplateDto>(
            sheetName ?? "采购申请明细导入模板",
            fileName ?? "采购申请明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购申请明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseRequestItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseRequestItemImportDto>(fileStream, sheetName ?? "采购申请明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseRequestItem>();
                var importKey = $"{entity.PurchaseRequestId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseRequestId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseRequestItemRepository,
                    x => x.PurchaseRequestId == entity.PurchaseRequestId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique)
                {
                    throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseRequestItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseRequestId == entity.PurchaseRequestId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseRequestCode) ? entity.PurchaseRequestCode : entity.PurchaseRequestId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseRequestItemRepository.CreateAsync(entity);
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
    /// 导出采购申请明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseRequestItemAsync(TaktPurchaseRequestItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseRequestItemQueryDto());
        var list = await _purchaseRequestItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseRequestItemExportDto>(),
                sheetName ?? "采购申请明细数据",
                fileName ?? "采购申请明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseRequestItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购申请明细数据",
            fileName ?? "采购申请明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购申请明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseRequestItem, bool>> QueryExpression(TaktPurchaseRequestItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseRequestItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PurchaseRequestId).Contains(keywords)
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.RequestUnit != null && x.RequestUnit.Contains(keywords))
                || SqlFunc.ToString(x.RequestQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedUnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedAmount).Contains(keywords)
                || (x.ReferenceSupplierCode != null && x.ReferenceSupplierCode.Contains(keywords))
                || (x.ReferenceSupplierName != null && x.ReferenceSupplierName.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchaseRequestId.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseRequestId == queryDto.PurchaseRequestId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseRequestCode))
        {
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(queryDto.PurchaseRequestCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
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

        if (!string.IsNullOrEmpty(queryDto?.RequestUnit))
        {
            exp = exp.And(x => x.RequestUnit != null && x.RequestUnit.Contains(queryDto.RequestUnit));
        }

        if (queryDto?.RequestQuantity.HasValue == true)
        {
            exp = exp.And(x => x.RequestQuantity == queryDto.RequestQuantity);
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

        if (!string.IsNullOrEmpty(queryDto?.ReferenceSupplierCode))
        {
            exp = exp.And(x => x.ReferenceSupplierCode != null && x.ReferenceSupplierCode.Contains(queryDto.ReferenceSupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReferenceSupplierName))
        {
            exp = exp.And(x => x.ReferenceSupplierName != null && x.ReferenceSupplierName.Contains(queryDto.ReferenceSupplierName));
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
