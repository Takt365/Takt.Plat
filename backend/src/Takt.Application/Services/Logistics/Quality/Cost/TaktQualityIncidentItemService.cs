// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentItemService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：品质事故明细应用服务实现
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

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质事故明细应用服务
/// </summary>
public class TaktQualityIncidentItemService : TaktServiceBase, ITaktQualityIncidentItemService
{
    private readonly ITaktCompanyRepository<TaktQualityIncidentItem> _qualityIncidentItemRepository;
    private readonly ITaktCompanyRepository<TaktQualityIncident> _qualityIncidentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIncidentItemRepository">品质事故明细仓储</param>
    /// <param name="qualityIncidentRepository">品质事故主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIncidentItemService(
        ITaktCompanyRepository<TaktQualityIncidentItem> qualityIncidentItemRepository,
        ITaktCompanyRepository<TaktQualityIncident> qualityIncidentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIncidentItemRepository = qualityIncidentItemRepository;
        _qualityIncidentRepository = qualityIncidentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质事故明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIncidentItemDto>> GetQualityIncidentItemListAsync(TaktQualityIncidentItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIncidentItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIncidentItemDto>.Create(
            data.Adapt<List<TaktQualityIncidentItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质事故明细
    /// </summary>
    /// <param name="id">品质事故明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto?> GetQualityIncidentItemByIdAsync(long id)
    {
        var entity = await _qualityIncidentItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityIncidentItemDto>();
    }

    /// <summary>
    /// 获取品质事故明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIncidentItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityIncidentItemRepository.GetListAsync(
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
    /// 创建品质事故明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto> CreateQualityIncidentItemAsync(TaktQualityIncidentItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIncidentItem>();
        await StampQualityIncidentItemQualityIncidentAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentItemRepository,
            x => x.QualityIncidentId == entity.QualityIncidentId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
        {
            throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityIncidentItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIncidentId == entity.QualityIncidentId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIncidentCode) ? entity.QualityIncidentCode : entity.QualityIncidentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityIncidentItemRepository.CreateAsync(entity);
        return await GetQualityIncidentItemByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIncidentItemDto>();
    }

    /// <summary>
    /// 更新品质事故明细
    /// </summary>
    /// <param name="id">品质事故明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto> UpdateQualityIncidentItemAsync(long id, TaktQualityIncidentItemUpdateDto dto)
    {
        var entity = await _qualityIncidentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityIncidentItemQualityIncidentAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentItemRepository,
            x => x.QualityIncidentId == entity.QualityIncidentId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
        {
            throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
        }
        await _qualityIncidentItemRepository.UpdateAsync(entity);
        return await GetQualityIncidentItemByIdAsync(id) ?? throw new TaktBusinessException("品质事故明细不存在");
    }

    /// <summary>
    /// 删除品质事故明细
    /// </summary>
    /// <param name="id">品质事故明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentItemByIdAsync(long id)
    {
        var deleted = await _qualityIncidentItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质事故明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质事故明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIncidentItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIncidentItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIncidentItemTemplateDto>(
            sheetName ?? "品质事故明细导入模板",
            fileName ?? "品质事故明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质事故明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIncidentItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIncidentItemImportDto>(fileStream, sheetName ?? "品质事故明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIncidentItem>();
                var importDto = rows[i].Adapt<TaktQualityIncidentItemCreateDto>();
                await StampQualityIncidentItemQualityIncidentAsync(entity, importDto);
                var importKey = $"{entity.QualityIncidentId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityIncidentId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIncidentItemRepository,
                    x => x.QualityIncidentId == entity.QualityIncidentId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
                {
                    throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityIncidentItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIncidentId == entity.QualityIncidentId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIncidentCode) ? entity.QualityIncidentCode : entity.QualityIncidentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityIncidentItemRepository.CreateAsync(entity);
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
    /// 导出品质事故明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIncidentItemAsync(TaktQualityIncidentItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityIncidentItemQueryDto());
        var list = await _qualityIncidentItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIncidentItemExportDto>(),
                sheetName ?? "品质事故明细数据",
                fileName ?? "品质事故明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIncidentItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质事故明细数据",
            fileName ?? "品质事故明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步品质事故明细主表外键（ManyToOne → 品质事故主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityIncidentItemQualityIncidentAsync(TaktQualityIncidentItem entity, TaktQualityIncidentItemCreateDto dto)
    {
        if (dto.QualityIncidentId <= 0)
        {
            return;
        }
        var master = await _qualityIncidentRepository.GetByIdAsync(dto.QualityIncidentId);
        if (master == null)
        {
            throw new TaktBusinessException("品质事故主不存在");
        }
        entity.QualityIncidentId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质事故明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIncidentItem, bool>> QueryExpression(TaktQualityIncidentItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIncidentItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.QualityIncidentId).Contains(keywords)
                || (x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || SqlFunc.ToString(x.ScrapCost).Contains(keywords)
                || SqlFunc.ToString(x.ScrapSize).Contains(keywords)
                || SqlFunc.ToString(x.PartPrice).Contains(keywords)
                || SqlFunc.ToString(x.ScrapReasonCost).Contains(keywords)
                || SqlFunc.ToString(x.FreightCharges).Contains(keywords)
                || SqlFunc.ToString(x.OtherExpenses).Contains(keywords)
                || SqlFunc.ToString(x.ReasonWorkTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.Tax).Contains(keywords)
                || SqlFunc.ToString(x.ReasonOtherExpenses).Contains(keywords)
                || (x.ScrapNote != null && x.ScrapNote.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.QualityIncidentId.HasValue == true)
        {
            exp = exp.And(x => x.QualityIncidentId == queryDto.QualityIncidentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityIncidentCode))
        {
            exp = exp.And(x => x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(queryDto.QualityIncidentCode));
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

        if (queryDto?.ScrapCost.HasValue == true)
        {
            exp = exp.And(x => x.ScrapCost == queryDto.ScrapCost);
        }

        if (queryDto?.ScrapSize.HasValue == true)
        {
            exp = exp.And(x => x.ScrapSize == queryDto.ScrapSize);
        }

        if (queryDto?.PartPrice.HasValue == true)
        {
            exp = exp.And(x => x.PartPrice == queryDto.PartPrice);
        }

        if (queryDto?.ScrapReasonCost.HasValue == true)
        {
            exp = exp.And(x => x.ScrapReasonCost == queryDto.ScrapReasonCost);
        }

        if (queryDto?.FreightCharges.HasValue == true)
        {
            exp = exp.And(x => x.FreightCharges == queryDto.FreightCharges);
        }

        if (queryDto?.OtherExpenses.HasValue == true)
        {
            exp = exp.And(x => x.OtherExpenses == queryDto.OtherExpenses);
        }

        if (queryDto?.ReasonWorkTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ReasonWorkTimeMinutes == queryDto.ReasonWorkTimeMinutes);
        }

        if (queryDto?.Tax.HasValue == true)
        {
            exp = exp.And(x => x.Tax == queryDto.Tax);
        }

        if (queryDto?.ReasonOtherExpenses.HasValue == true)
        {
            exp = exp.And(x => x.ReasonOtherExpenses == queryDto.ReasonOtherExpenses);
        }

        if (!string.IsNullOrEmpty(queryDto?.ScrapNote))
        {
            exp = exp.And(x => x.ScrapNote != null && x.ScrapNote.Contains(queryDto.ScrapNote));
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
