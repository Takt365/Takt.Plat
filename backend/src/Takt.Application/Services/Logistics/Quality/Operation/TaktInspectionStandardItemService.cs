// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：检验标准明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 检验标准明细应用服务
/// </summary>
public class TaktInspectionStandardItemService : TaktServiceBase, ITaktInspectionStandardItemService
{
    private readonly ITaktCompanyRepository<TaktInspectionStandardItem> _inspectionStandardItemRepository;
    private readonly ITaktCompanyRepository<TaktInspectionStandard> _inspectionStandardRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inspectionStandardItemRepository">检验标准明细仓储</param>
    /// <param name="inspectionStandardRepository">检验标准仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktInspectionStandardItemService(
        ITaktCompanyRepository<TaktInspectionStandardItem> inspectionStandardItemRepository,
        ITaktCompanyRepository<TaktInspectionStandard> inspectionStandardRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _inspectionStandardItemRepository = inspectionStandardItemRepository;
        _inspectionStandardRepository = inspectionStandardRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取检验标准明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktInspectionStandardItemDto>> GetInspectionStandardItemListAsync(TaktInspectionStandardItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _inspectionStandardItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktInspectionStandardItemDto>.Create(
            data.Adapt<List<TaktInspectionStandardItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto?> GetInspectionStandardItemByIdAsync(long id)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktInspectionStandardItemDto>();
    }

    /// <summary>
    /// 获取检验标准明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetInspectionStandardItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _inspectionStandardItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ItemName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建检验标准明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto> CreateInspectionStandardItemAsync(TaktInspectionStandardItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktInspectionStandardItem>();
        entity.IsObsolete = 0;
        await StampInspectionStandardItemInspectionStandardAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
            _inspectionStandardItemRepository,
            x => x.InspectionStandardId == entity.InspectionStandardId
                && x.LineNumber == entity.LineNumber
                && x.ItemCode == entity.ItemCode
                && x.ItemType == entity.ItemType);
        if (!isUnique_ix_takt_logistics_quality_inspection_standard_item_unique)
        {
            throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _inspectionStandardItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InspectionStandardId == entity.InspectionStandardId,
                x => x.LineNumber);
            var businessCode = entity.InspectionStandardId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _inspectionStandardItemRepository.CreateAsync(entity);
        return await GetInspectionStandardItemByIdAsync(entity.Id) ?? entity.Adapt<TaktInspectionStandardItemDto>();
    }

    /// <summary>
    /// 更新检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto> UpdateInspectionStandardItemAsync(long id, TaktInspectionStandardItemUpdateDto dto)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准明细不存在");
        }
        dto.Adapt(entity);
        await StampInspectionStandardItemInspectionStandardAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
            _inspectionStandardItemRepository,
            x => x.InspectionStandardId == entity.InspectionStandardId
                && x.LineNumber == entity.LineNumber
                && x.ItemCode == entity.ItemCode
                && x.ItemType == entity.ItemType,
            id);
        if (!isUnique_ix_takt_logistics_quality_inspection_standard_item_unique)
        {
            throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
        }
        await _inspectionStandardItemRepository.UpdateAsync(entity);
        return await GetInspectionStandardItemByIdAsync(id) ?? throw new TaktBusinessException("检验标准明细不存在");
    }

    /// <summary>
    /// 删除检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteInspectionStandardItemByIdAsync(long id)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("检验标准明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("检验标准明细已作废");
        }
        entity.IsObsolete = 1;
        await _inspectionStandardItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除检验标准明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteInspectionStandardItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteInspectionStandardItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新检验标准明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto> UpdateInspectionStandardItemObsoleteAsync(TaktInspectionStandardItemObsoleteDto dto)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(dto.InspectionStandardItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("检验标准明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _inspectionStandardItemRepository.UpdateAsync(entity);
        return await GetInspectionStandardItemByIdAsync(dto.InspectionStandardItemId) ?? throw new TaktBusinessException("检验标准明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetInspectionStandardItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktInspectionStandardItemTemplateDto>(
            sheetName ?? "检验标准明细导入模板",
            fileName ?? "检验标准明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入检验标准明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportInspectionStandardItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktInspectionStandardItemImportDto>(fileStream, sheetName ?? "检验标准明细导入模板");
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
                var entity = rows[i].Adapt<TaktInspectionStandardItem>();
                var importDto = rows[i].Adapt<TaktInspectionStandardItemCreateDto>();
                await StampInspectionStandardItemInspectionStandardAsync(entity, importDto);
                var importKey = $"{entity.InspectionStandardId}|{entity.LineNumber}|{entity.ItemCode}|{entity.ItemType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InspectionStandardId、LineNumber、ItemCode、ItemType）");
                }
                var isUnique_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
                    _inspectionStandardItemRepository,
                    x => x.InspectionStandardId == entity.InspectionStandardId
                        && x.LineNumber == entity.LineNumber
                        && x.ItemCode == entity.ItemCode
                        && x.ItemType == entity.ItemType);
                if (!isUnique_ix_takt_logistics_quality_inspection_standard_item_unique)
                {
                    throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _inspectionStandardItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InspectionStandardId == entity.InspectionStandardId,
                        x => x.LineNumber);
                    var businessCode = entity.InspectionStandardId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _inspectionStandardItemRepository.CreateAsync(entity);
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
    /// 导出检验标准明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportInspectionStandardItemAsync(TaktInspectionStandardItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktInspectionStandardItemQueryDto());
        var list = await _inspectionStandardItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktInspectionStandardItemExportDto>(),
                sheetName ?? "检验标准明细数据",
                fileName ?? "检验标准明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktInspectionStandardItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "检验标准明细数据",
            fileName ?? "检验标准明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步检验标准明细主表外键（ManyToOne → 检验标准）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampInspectionStandardItemInspectionStandardAsync(TaktInspectionStandardItem entity, TaktInspectionStandardItemCreateDto dto)
    {
        if (dto.InspectionStandardId <= 0)
        {
            return;
        }
        var master = await _inspectionStandardRepository.GetByIdAsync(dto.InspectionStandardId);
        if (master == null)
        {
            throw new TaktBusinessException("检验标准不存在");
        }
        entity.InspectionStandardId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建检验标准明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktInspectionStandardItem, bool>> QueryExpression(TaktInspectionStandardItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktInspectionStandardItem>();

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
                SqlFunc.ToString(x.InspectionStandardId).Contains(keywords)
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.ItemCode != null && x.ItemCode.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || SqlFunc.ToString(x.ItemType).Contains(keywords)
                || (x.DefectLevel != null && x.DefectLevel.Contains(keywords))
                || SqlFunc.ToString(x.InspectionMode).Contains(keywords)
                || (x.StandardValue != null && x.StandardValue.Contains(keywords))
                || (x.UpperLimit != null && x.UpperLimit.Contains(keywords))
                || (x.LowerLimit != null && x.LowerLimit.Contains(keywords))
                || (x.InspectionTool != null && x.InspectionTool.Contains(keywords))
                || (x.InspectionMethodDescription != null && x.InspectionMethodDescription.Contains(keywords))
                || (x.AcceptanceCriteria != null && x.AcceptanceCriteria.Contains(keywords))
                || (x.RejectionCriteria != null && x.RejectionCriteria.Contains(keywords))
                || SqlFunc.ToString(x.IsQualifiedBasis).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.InspectionStandardId.HasValue == true)
        {
            exp = exp.And(x => x.InspectionStandardId == queryDto.InspectionStandardId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemCode))
        {
            exp = exp.And(x => x.ItemCode != null && x.ItemCode.Contains(queryDto.ItemCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemName))
        {
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(queryDto.ItemName));
        }

        if (queryDto?.ItemType.HasValue == true)
        {
            exp = exp.And(x => x.ItemType == queryDto.ItemType);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectLevel))
        {
            exp = exp.And(x => x.DefectLevel != null && x.DefectLevel.Contains(queryDto.DefectLevel));
        }

        if (queryDto?.InspectionMode.HasValue == true)
        {
            exp = exp.And(x => x.InspectionMode == queryDto.InspectionMode);
        }

        if (!string.IsNullOrEmpty(queryDto?.StandardValue))
        {
            exp = exp.And(x => x.StandardValue != null && x.StandardValue.Contains(queryDto.StandardValue));
        }

        if (!string.IsNullOrEmpty(queryDto?.UpperLimit))
        {
            exp = exp.And(x => x.UpperLimit != null && x.UpperLimit.Contains(queryDto.UpperLimit));
        }

        if (!string.IsNullOrEmpty(queryDto?.LowerLimit))
        {
            exp = exp.And(x => x.LowerLimit != null && x.LowerLimit.Contains(queryDto.LowerLimit));
        }

        if (!string.IsNullOrEmpty(queryDto?.InspectionTool))
        {
            exp = exp.And(x => x.InspectionTool != null && x.InspectionTool.Contains(queryDto.InspectionTool));
        }

        if (!string.IsNullOrEmpty(queryDto?.InspectionMethodDescription))
        {
            exp = exp.And(x => x.InspectionMethodDescription != null && x.InspectionMethodDescription.Contains(queryDto.InspectionMethodDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptanceCriteria))
        {
            exp = exp.And(x => x.AcceptanceCriteria != null && x.AcceptanceCriteria.Contains(queryDto.AcceptanceCriteria));
        }

        if (!string.IsNullOrEmpty(queryDto?.RejectionCriteria))
        {
            exp = exp.And(x => x.RejectionCriteria != null && x.RejectionCriteria.Contains(queryDto.RejectionCriteria));
        }

        if (queryDto?.IsQualifiedBasis.HasValue == true)
        {
            exp = exp.And(x => x.IsQualifiedBasis == queryDto.IsQualifiedBasis);
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
