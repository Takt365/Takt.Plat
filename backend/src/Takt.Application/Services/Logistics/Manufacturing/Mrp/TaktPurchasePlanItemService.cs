// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktPurchasePlanItemService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购计划明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Domain.Entities.Logistics.Manufacturing.Mrp;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mrp;

/// <summary>
/// 采购计划明细应用服务
/// </summary>
public class TaktPurchasePlanItemService : TaktServiceBase, ITaktPurchasePlanItemService
{
    private readonly ITaktCompanyRepository<TaktPurchasePlanItem> _purchasePlanItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePlanItemRepository">采购计划明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePlanItemService(
        ITaktCompanyRepository<TaktPurchasePlanItem> purchasePlanItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePlanItemRepository = purchasePlanItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购计划明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePlanItemDto>> GetPurchasePlanItemListAsync(TaktPurchasePlanItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePlanItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePlanItemDto>.Create(
            data.Adapt<List<TaktPurchasePlanItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购计划明细
    /// </summary>
    /// <param name="id">采购计划明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanItemDto?> GetPurchasePlanItemByIdAsync(long id)
    {
        var entity = await _purchasePlanItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchasePlanItemDto>();
    }

    /// <summary>
    /// 获取采购计划明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePlanItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePlanItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchasePlanCode,
            DictLabel = e.MaterialDescription ?? e.PurchasePlanCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购计划明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanItemDto> CreatePurchasePlanItemAsync(TaktPurchasePlanItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePlanItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePlanItemRepository,
            x => x.PurchasePlanId == entity.PurchasePlanId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique)
        {
            throw new TaktBusinessException("采购计划明细的PurchasePlanId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchasePlanItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePlanId == entity.PurchasePlanId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchasePlanCode) ? entity.PurchasePlanCode : entity.PurchasePlanId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchasePlanItemRepository.CreateAsync(entity);
        return await GetPurchasePlanItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePlanItemDto>();
    }

    /// <summary>
    /// 更新采购计划明细
    /// </summary>
    /// <param name="id">采购计划明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanItemDto> UpdatePurchasePlanItemAsync(long id, TaktPurchasePlanItemUpdateDto dto)
    {
        var entity = await _purchasePlanItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购计划明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePlanItemRepository,
            x => x.PurchasePlanId == entity.PurchasePlanId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique)
        {
            throw new TaktBusinessException("采购计划明细的PurchasePlanId、LineNumber、MaterialCode已存在");
        }
        await _purchasePlanItemRepository.UpdateAsync(entity);
        return await GetPurchasePlanItemByIdAsync(id) ?? throw new TaktBusinessException("采购计划明细不存在");
    }

    /// <summary>
    /// 删除采购计划明细
    /// </summary>
    /// <param name="id">采购计划明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePlanItemByIdAsync(long id)
    {
        var entity = await _purchasePlanItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购计划明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购计划明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购计划明细已作废");
        }
        entity.IsObsolete = 1;
        await _purchasePlanItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购计划明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePlanItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePlanItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购计划明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanItemDto> UpdatePurchasePlanItemObsoleteAsync(TaktPurchasePlanItemObsoleteDto dto)
    {
        var entity = await _purchasePlanItemRepository.GetByIdAsync(dto.PurchasePlanItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购计划明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购计划明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchasePlanItemRepository.UpdateAsync(entity);
        return await GetPurchasePlanItemByIdAsync(dto.PurchasePlanItemId) ?? throw new TaktBusinessException("采购计划明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePlanItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePlanItemTemplateDto>(
            sheetName ?? "采购计划明细导入模板",
            fileName ?? "采购计划明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购计划明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePlanItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePlanItemImportDto>(fileStream, sheetName ?? "采购计划明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePlanItem>();
                var importKey = $"{entity.PurchasePlanId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasePlanId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePlanItemRepository,
                    x => x.PurchasePlanId == entity.PurchasePlanId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique)
                {
                    throw new TaktBusinessException("采购计划明细的PurchasePlanId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchasePlanItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePlanId == entity.PurchasePlanId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchasePlanCode) ? entity.PurchasePlanCode : entity.PurchasePlanId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchasePlanItemRepository.CreateAsync(entity);
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
    /// 导出采购计划明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePlanItemAsync(TaktPurchasePlanItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchasePlanItemQueryDto());
        var list = await _purchasePlanItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePlanItemExportDto>(),
                sheetName ?? "采购计划明细数据",
                fileName ?? "采购计划明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePlanItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购计划明细数据",
            fileName ?? "采购计划明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购计划明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePlanItem, bool>> QueryExpression(TaktPurchasePlanItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePlanItem>();

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
                SqlFunc.ToString(x.PurchasePlanId).Contains(keywords)
                || (x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.ProductionPlanId).Contains(keywords)
                || (x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(keywords))
                || SqlFunc.ToString(x.ProductionPlanLineNumber).Contains(keywords)
                || SqlFunc.ToString(x.MaterialRequirementsPlanningItemId).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.PlanUnit != null && x.PlanUnit.Contains(keywords))
                || SqlFunc.ToString(x.PlanQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedUnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedAmount).Contains(keywords)
                || SqlFunc.ToString(x.TaxIncludedPrice).Contains(keywords)
                || SqlFunc.ToString(x.UntaxedPrice).Contains(keywords)
                || SqlFunc.ToString(x.TaxAmount).Contains(keywords)
                || (x.ReferenceSupplierCode != null && x.ReferenceSupplierCode.Contains(keywords))
                || (x.ReferenceSupplierName1 != null && x.ReferenceSupplierName1.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedArrivalDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchasePlanId.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePlanId == queryDto.PurchasePlanId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasePlanCode))
        {
            exp = exp.And(x => x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(queryDto.PurchasePlanCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.ProductionPlanId.HasValue == true)
        {
            exp = exp.And(x => x.ProductionPlanId == queryDto.ProductionPlanId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionPlanCode))
        {
            exp = exp.And(x => x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(queryDto.ProductionPlanCode));
        }

        if (queryDto?.ProductionPlanLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.ProductionPlanLineNumber == queryDto.ProductionPlanLineNumber);
        }

        if (queryDto?.MaterialRequirementsPlanningItemId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialRequirementsPlanningItemId == queryDto.MaterialRequirementsPlanningItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialSpecification))
        {
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(queryDto.MaterialSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlanUnit))
        {
            exp = exp.And(x => x.PlanUnit != null && x.PlanUnit.Contains(queryDto.PlanUnit));
        }

        if (queryDto?.PlanQuantity.HasValue == true)
        {
            exp = exp.And(x => x.PlanQuantity == queryDto.PlanQuantity);
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

        if (queryDto?.TaxIncludedPrice.HasValue == true)
        {
            exp = exp.And(x => x.TaxIncludedPrice == queryDto.TaxIncludedPrice);
        }

        if (queryDto?.UntaxedPrice.HasValue == true)
        {
            exp = exp.And(x => x.UntaxedPrice == queryDto.UntaxedPrice);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            exp = exp.And(x => x.TaxAmount == queryDto.TaxAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.ReferenceSupplierCode))
        {
            exp = exp.And(x => x.ReferenceSupplierCode != null && x.ReferenceSupplierCode.Contains(queryDto.ReferenceSupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReferenceSupplierName1))
        {
            exp = exp.And(x => x.ReferenceSupplierName1 != null && x.ReferenceSupplierName1.Contains(queryDto.ReferenceSupplierName1));
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

        if (queryDto?.PlannedArrivalDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedArrivalDate >= queryDto.PlannedArrivalDateStart);
        }

        if (queryDto?.PlannedArrivalDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedArrivalDate <= queryDto.PlannedArrivalDateEnd);
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
