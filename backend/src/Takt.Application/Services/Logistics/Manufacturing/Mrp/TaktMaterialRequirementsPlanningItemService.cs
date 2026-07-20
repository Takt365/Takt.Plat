// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItemService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划MRP明细应用服务实现
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
/// 物料需求计划MRP明细应用服务
/// </summary>
public class TaktMaterialRequirementsPlanningItemService : TaktServiceBase, ITaktMaterialRequirementsPlanningItemService
{
    private readonly ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> _materialRequirementsPlanningItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialRequirementsPlanningItemRepository">物料需求计划MRP明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialRequirementsPlanningItemService(
        ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> materialRequirementsPlanningItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialRequirementsPlanningItemRepository = materialRequirementsPlanningItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料需求计划MRP明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialRequirementsPlanningItemDto>> GetMaterialRequirementsPlanningItemListAsync(TaktMaterialRequirementsPlanningItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialRequirementsPlanningItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialRequirementsPlanningItemDto>.Create(
            data.Adapt<List<TaktMaterialRequirementsPlanningItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto?> GetMaterialRequirementsPlanningItemByIdAsync(long id)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialRequirementsPlanningItemDto>();
    }

    /// <summary>
    /// 获取物料需求计划MRP明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialRequirementsPlanningItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _materialRequirementsPlanningItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MaterialName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建物料需求计划MRP明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto> CreateMaterialRequirementsPlanningItemAsync(TaktMaterialRequirementsPlanningItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialRequirementsPlanningItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_planning_mrp_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRequirementsPlanningItemRepository,
            x => x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mrp_item_line_unique)
        {
            throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _materialRequirementsPlanningItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialRequirementsPlanningCode) ? entity.MaterialRequirementsPlanningCode : entity.MaterialRequirementsPlanningId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _materialRequirementsPlanningItemRepository.CreateAsync(entity);
        return await GetMaterialRequirementsPlanningItemByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialRequirementsPlanningItemDto>();
    }

    /// <summary>
    /// 更新物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto> UpdateMaterialRequirementsPlanningItemAsync(long id, TaktMaterialRequirementsPlanningItemUpdateDto dto)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mrp_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRequirementsPlanningItemRepository,
            x => x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mrp_item_line_unique)
        {
            throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
        }
        await _materialRequirementsPlanningItemRepository.UpdateAsync(entity);
        return await GetMaterialRequirementsPlanningItemByIdAsync(id) ?? throw new TaktBusinessException("物料需求计划MRP明细不存在");
    }

    /// <summary>
    /// 删除物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialRequirementsPlanningItemByIdAsync(long id)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("物料需求计划MRP明细已作废");
        }
        entity.IsObsolete = 1;
        await _materialRequirementsPlanningItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除物料需求计划MRP明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialRequirementsPlanningItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialRequirementsPlanningItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料需求计划MRP明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto> UpdateMaterialRequirementsPlanningItemObsoleteAsync(TaktMaterialRequirementsPlanningItemObsoleteDto dto)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(dto.MaterialRequirementsPlanningItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _materialRequirementsPlanningItemRepository.UpdateAsync(entity);
        return await GetMaterialRequirementsPlanningItemByIdAsync(dto.MaterialRequirementsPlanningItemId) ?? throw new TaktBusinessException("物料需求计划MRP明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialRequirementsPlanningItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialRequirementsPlanningItemTemplateDto>(
            sheetName ?? "物料需求计划MRP明细导入模板",
            fileName ?? "物料需求计划MRP明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料需求计划MRP明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialRequirementsPlanningItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialRequirementsPlanningItemImportDto>(fileStream, sheetName ?? "物料需求计划MRP明细导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialRequirementsPlanningItem>();
                var importKey = $"{entity.MaterialRequirementsPlanningId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialRequirementsPlanningId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_mrp_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialRequirementsPlanningItemRepository,
                    x => x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_mrp_item_line_unique)
                {
                    throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _materialRequirementsPlanningItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialRequirementsPlanningCode) ? entity.MaterialRequirementsPlanningCode : entity.MaterialRequirementsPlanningId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _materialRequirementsPlanningItemRepository.CreateAsync(entity);
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
    /// 导出物料需求计划MRP明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialRequirementsPlanningItemAsync(TaktMaterialRequirementsPlanningItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialRequirementsPlanningItemQueryDto());
        var list = await _materialRequirementsPlanningItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialRequirementsPlanningItemExportDto>(),
                sheetName ?? "物料需求计划MRP明细数据",
                fileName ?? "物料需求计划MRP明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialRequirementsPlanningItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料需求计划MRP明细数据",
            fileName ?? "物料需求计划MRP明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料需求计划MRP明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialRequirementsPlanningItem, bool>> QueryExpression(TaktMaterialRequirementsPlanningItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialRequirementsPlanningItem>();

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
                SqlFunc.ToString(x.MaterialRequirementsPlanningId).Contains(keywords)
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.ModelName != null && x.ModelName.Contains(keywords))
                || (x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.BomLevel).Contains(keywords)
                || (x.PlanUnit != null && x.PlanUnit.Contains(keywords))
                || SqlFunc.ToString(x.GrossRequirement).Contains(keywords)
                || SqlFunc.ToString(x.ScheduledReceipts).Contains(keywords)
                || SqlFunc.ToString(x.OnHandQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ProjectedOnHand).Contains(keywords)
                || SqlFunc.ToString(x.NetRequirement).Contains(keywords)
                || SqlFunc.ToString(x.ProcurementType).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.RequirementDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MaterialRequirementsPlanningId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialRequirementsPlanningId == queryDto.MaterialRequirementsPlanningId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialRequirementsPlanningCode))
        {
            exp = exp.And(x => x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(queryDto.MaterialRequirementsPlanningCode));
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

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelName))
        {
            exp = exp.And(x => x.ModelName != null && x.ModelName.Contains(queryDto.ModelName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ParentMaterialCode))
        {
            exp = exp.And(x => x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(queryDto.ParentMaterialCode));
        }

        if (queryDto?.BomLevel.HasValue == true)
        {
            exp = exp.And(x => x.BomLevel == queryDto.BomLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlanUnit))
        {
            exp = exp.And(x => x.PlanUnit != null && x.PlanUnit.Contains(queryDto.PlanUnit));
        }

        if (queryDto?.GrossRequirement.HasValue == true)
        {
            exp = exp.And(x => x.GrossRequirement == queryDto.GrossRequirement);
        }

        if (queryDto?.ScheduledReceipts.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledReceipts == queryDto.ScheduledReceipts);
        }

        if (queryDto?.OnHandQuantity.HasValue == true)
        {
            exp = exp.And(x => x.OnHandQuantity == queryDto.OnHandQuantity);
        }

        if (queryDto?.ProjectedOnHand.HasValue == true)
        {
            exp = exp.And(x => x.ProjectedOnHand == queryDto.ProjectedOnHand);
        }

        if (queryDto?.NetRequirement.HasValue == true)
        {
            exp = exp.And(x => x.NetRequirement == queryDto.NetRequirement);
        }

        if (queryDto?.ProcurementType.HasValue == true)
        {
            exp = exp.And(x => x.ProcurementType == queryDto.ProcurementType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.RequirementDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequirementDate >= queryDto.RequirementDateStart);
        }

        if (queryDto?.RequirementDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequirementDate <= queryDto.RequirementDateEnd);
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
