// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlanItemService.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：销售计划明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Planning;
using Takt.Domain.Entities.Logistics.Manufacturing.Planning;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Planning;

/// <summary>
/// 销售计划明细应用服务
/// </summary>
public class TaktSalesPlanItemService : TaktServiceBase, ITaktSalesPlanItemService
{
    private readonly ITaktCompanyRepository<TaktSalesPlanItem> _salesPlanItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPlanItemRepository">销售计划明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPlanItemService(
        ITaktCompanyRepository<TaktSalesPlanItem> salesPlanItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPlanItemRepository = salesPlanItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售计划明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPlanItemDto>> GetSalesPlanItemListAsync(TaktSalesPlanItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPlanItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPlanItemDto>.Create(
            data.Adapt<List<TaktSalesPlanItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售计划明细
    /// </summary>
    /// <param name="id">销售计划明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanItemDto?> GetSalesPlanItemByIdAsync(long id)
    {
        var entity = await _salesPlanItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesPlanItemDto>();
    }

    /// <summary>
    /// 获取销售计划明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPlanItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPlanItemRepository.GetListAsync(
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
    /// 创建销售计划明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanItemDto> CreateSalesPlanItemAsync(TaktSalesPlanItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPlanItem>();
        var isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPlanItemRepository,
            x => x.SalesPlanId == entity.SalesPlanId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique)
        {
            throw new TaktBusinessException("销售计划明细的SalesPlanId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesPlanItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPlanId == entity.SalesPlanId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPlanCode) ? entity.SalesPlanCode : entity.SalesPlanId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesPlanItemRepository.CreateAsync(entity);
        return await GetSalesPlanItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPlanItemDto>();
    }

    /// <summary>
    /// 更新销售计划明细
    /// </summary>
    /// <param name="id">销售计划明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanItemDto> UpdateSalesPlanItemAsync(long id, TaktSalesPlanItemUpdateDto dto)
    {
        var entity = await _salesPlanItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售计划明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPlanItemRepository,
            x => x.SalesPlanId == entity.SalesPlanId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique)
        {
            throw new TaktBusinessException("销售计划明细的SalesPlanId、LineNumber、MaterialCode已存在");
        }
        await _salesPlanItemRepository.UpdateAsync(entity);
        return await GetSalesPlanItemByIdAsync(id) ?? throw new TaktBusinessException("销售计划明细不存在");
    }

    /// <summary>
    /// 删除销售计划明细
    /// </summary>
    /// <param name="id">销售计划明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPlanItemByIdAsync(long id)
    {
        var deleted = await _salesPlanItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售计划明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售计划明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPlanItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPlanItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPlanItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPlanItemTemplateDto>(
            sheetName ?? "销售计划明细导入模板",
            fileName ?? "销售计划明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售计划明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPlanItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPlanItemImportDto>(fileStream, sheetName ?? "销售计划明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPlanItem>();
                var importKey = $"{entity.SalesPlanId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesPlanId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPlanItemRepository,
                    x => x.SalesPlanId == entity.SalesPlanId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique)
                {
                    throw new TaktBusinessException("销售计划明细的SalesPlanId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesPlanItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPlanId == entity.SalesPlanId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPlanCode) ? entity.SalesPlanCode : entity.SalesPlanId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesPlanItemRepository.CreateAsync(entity);
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
    /// 导出销售计划明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPlanItemAsync(TaktSalesPlanItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPlanItemQueryDto());
        var list = await _salesPlanItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPlanItemExportDto>(),
                sheetName ?? "销售计划明细数据",
                fileName ?? "销售计划明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPlanItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售计划明细数据",
            fileName ?? "销售计划明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售计划明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPlanItem, bool>> QueryExpression(TaktSalesPlanItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPlanItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SalesPlanId).Contains(keywords)
                || (x.SalesPlanCode != null && x.SalesPlanCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.PlanUnit != null && x.PlanUnit.Contains(keywords))
                || SqlFunc.ToString(x.PlanQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedUnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedAmount).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedDeliveryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SalesPlanId.HasValue == true)
        {
            exp = exp.And(x => x.SalesPlanId == queryDto.SalesPlanId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPlanCode))
        {
            exp = exp.And(x => x.SalesPlanCode != null && x.SalesPlanCode.Contains(queryDto.SalesPlanCode));
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

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
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

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlannedDeliveryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDeliveryDate >= queryDto.PlannedDeliveryDateStart);
        }

        if (queryDto?.PlannedDeliveryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDeliveryDate <= queryDto.PlannedDeliveryDateEnd);
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
