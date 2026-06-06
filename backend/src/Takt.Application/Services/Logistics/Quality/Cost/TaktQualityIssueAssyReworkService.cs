// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueAssyReworkService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题组装不良改修费用明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Quality.Cost;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 质量问题组装不良改修费用明细应用服务
/// </summary>
public class TaktQualityIssueAssyReworkService : TaktServiceBase, ITaktQualityIssueAssyReworkService
{
    private readonly ITaktCompanyRepository<TaktQualityIssueAssyRework> _qualityIssueAssyReworkRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssue> _qualityIssueRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssueAssyReworkRepository">质量问题组装不良改修费用明细仓储</param>
    /// <param name="qualityIssueRepository">品质问题应对主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIssueAssyReworkService(
        ITaktCompanyRepository<TaktQualityIssueAssyRework> qualityIssueAssyReworkRepository,
        ITaktCompanyRepository<TaktQualityIssue> qualityIssueRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIssueAssyReworkRepository = qualityIssueAssyReworkRepository;
        _qualityIssueRepository = qualityIssueRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取质量问题组装不良改修费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIssueAssyReworkDto>> GetQualityIssueAssyReworkListAsync(TaktQualityIssueAssyReworkQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIssueAssyReworkRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIssueAssyReworkDto>.Create(
            data.Adapt<List<TaktQualityIssueAssyReworkDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题组装不良改修费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto?> GetQualityIssueAssyReworkByIdAsync(long id)
    {
        var entity = await _qualityIssueAssyReworkRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityIssueAssyReworkDto>();
    }

    /// <summary>
    /// 获取质量问题组装不良改修费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIssueAssyReworkOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityIssueAssyReworkRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AssyCustomerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AssyCustomerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto> CreateQualityIssueAssyReworkAsync(TaktQualityIssueAssyReworkCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIssueAssyRework>();
                await StampQualityIssueAssyReworkQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueAssyReworkRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityIssueAssyReworkRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityIssueAssyReworkRepository.CreateAsync(entity);
        return await GetQualityIssueAssyReworkByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIssueAssyReworkDto>();
    }

    /// <summary>
    /// 更新质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题组装不良改修费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto> UpdateQualityIssueAssyReworkAsync(long id, TaktQualityIssueAssyReworkUpdateDto dto)
    {
        var entity = await _qualityIssueAssyReworkRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
        }
        dto.Adapt(entity);
                await StampQualityIssueAssyReworkQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueAssyReworkRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
        }
        await _qualityIssueAssyReworkRepository.UpdateAsync(entity);
        return await GetQualityIssueAssyReworkByIdAsync(id) ?? throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
    }

    /// <summary>
    /// 删除质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题组装不良改修费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueAssyReworkByIdAsync(long id)
    {
        var deleted = await _qualityIssueAssyReworkRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueAssyReworkBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIssueAssyReworkByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIssueAssyReworkTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIssueAssyReworkTemplateDto>(
            sheetName ?? "质量问题组装不良改修费用明细导入模板",
            fileName ?? "质量问题组装不良改修费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIssueAssyReworkAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIssueAssyReworkImportDto>(fileStream, sheetName ?? "质量问题组装不良改修费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIssueAssyRework>();
                var importDto = rows[i].Adapt<TaktQualityIssueAssyReworkCreateDto>();
                await StampQualityIssueAssyReworkQualityIssueAsync(entity, importDto);
                var importKey = $"{entity.QualityIssueId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityIssueId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIssueAssyReworkRepository,
                    x => x.QualityIssueId == entity.QualityIssueId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique)
                {
                    throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityIssueAssyReworkRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityIssueAssyReworkRepository.CreateAsync(entity);
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
    /// 导出质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIssueAssyReworkAsync(TaktQualityIssueAssyReworkQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityIssueAssyReworkQueryDto());
        var list = await _qualityIssueAssyReworkRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueAssyReworkExportDto>(),
                sheetName ?? "质量问题组装不良改修费用明细数据",
                fileName ?? "质量问题组装不良改修费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIssueAssyReworkExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "质量问题组装不良改修费用明细数据",
            fileName ?? "质量问题组装不良改修费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步质量问题组装不良改修费用明细主表外键（ManyToOne → 品质问题应对主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityIssueAssyReworkQualityIssueAsync(TaktQualityIssueAssyRework entity, TaktQualityIssueAssyReworkCreateDto dto)
    {
        if (dto.QualityIssueId <= 0)
        {
            return;
        }
        var master = await _qualityIssueRepository.GetByIdAsync(dto.QualityIssueId);
        if (master == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在");
        }
        entity.QualityIssueId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建质量问题组装不良改修费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIssueAssyRework, bool>> QueryExpression(TaktQualityIssueAssyReworkQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIssueAssyRework>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.QualityIssueId).Contains(keywords)
                || (x.QualityIssueCode != null && x.QualityIssueCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.AssyDefectParts != null && x.AssyDefectParts.Contains(keywords))
                || SqlFunc.ToString(x.AssyReworkCost).Contains(keywords)
                || SqlFunc.ToString(x.AssyReworkTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.AssyReinspectionTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.AssyTravelCost).Contains(keywords)
                || SqlFunc.ToString(x.AssyWarehouseCost).Contains(keywords)
                || SqlFunc.ToString(x.AssyOtherExpenses).Contains(keywords)
                || (x.AssyReworkNote != null && x.AssyReworkNote.Contains(keywords))
                || SqlFunc.ToString(x.AssyScrapCost).Contains(keywords)
                || (x.AssyCustomerName != null && x.AssyCustomerName.Contains(keywords))
                || (x.AssyDebitNoteNo != null && x.AssyDebitNoteNo.Contains(keywords))
                || SqlFunc.ToString(x.AssyOtherExpenses2).Contains(keywords)
                || (x.AssyNote != null && x.AssyNote.Contains(keywords))
                || (x.AssyRecorder != null && x.AssyRecorder.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.QualityIssueId.HasValue == true)
        {
            exp = exp.And(x => x.QualityIssueId == queryDto.QualityIssueId);
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityIssueCode))
        {
            exp = exp.And(x => x.QualityIssueCode != null && x.QualityIssueCode.Contains(queryDto.QualityIssueCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssyDefectParts))
        {
            exp = exp.And(x => x.AssyDefectParts != null && x.AssyDefectParts.Contains(queryDto.AssyDefectParts));
        }

        if (queryDto?.AssyReworkCost.HasValue == true)
        {
            exp = exp.And(x => x.AssyReworkCost == queryDto.AssyReworkCost);
        }

        if (queryDto?.AssyReworkTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.AssyReworkTimeMinutes == queryDto.AssyReworkTimeMinutes);
        }

        if (queryDto?.AssyReinspectionTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.AssyReinspectionTimeMinutes == queryDto.AssyReinspectionTimeMinutes);
        }

        if (queryDto?.AssyTravelCost.HasValue == true)
        {
            exp = exp.And(x => x.AssyTravelCost == queryDto.AssyTravelCost);
        }

        if (queryDto?.AssyWarehouseCost.HasValue == true)
        {
            exp = exp.And(x => x.AssyWarehouseCost == queryDto.AssyWarehouseCost);
        }

        if (queryDto?.AssyOtherExpenses.HasValue == true)
        {
            exp = exp.And(x => x.AssyOtherExpenses == queryDto.AssyOtherExpenses);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssyReworkNote))
        {
            exp = exp.And(x => x.AssyReworkNote != null && x.AssyReworkNote.Contains(queryDto.AssyReworkNote));
        }

        if (queryDto?.AssyScrapCost.HasValue == true)
        {
            exp = exp.And(x => x.AssyScrapCost == queryDto.AssyScrapCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssyCustomerName))
        {
            exp = exp.And(x => x.AssyCustomerName != null && x.AssyCustomerName.Contains(queryDto.AssyCustomerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssyDebitNoteNo))
        {
            exp = exp.And(x => x.AssyDebitNoteNo != null && x.AssyDebitNoteNo.Contains(queryDto.AssyDebitNoteNo));
        }

        if (queryDto?.AssyOtherExpenses2.HasValue == true)
        {
            exp = exp.And(x => x.AssyOtherExpenses2 == queryDto.AssyOtherExpenses2);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssyNote))
        {
            exp = exp.And(x => x.AssyNote != null && x.AssyNote.Contains(queryDto.AssyNote));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssyRecorder))
        {
            exp = exp.And(x => x.AssyRecorder != null && x.AssyRecorder.Contains(queryDto.AssyRecorder));
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
