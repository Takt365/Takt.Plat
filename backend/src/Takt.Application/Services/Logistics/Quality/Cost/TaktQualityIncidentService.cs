// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentService.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：品质事故主应用服务实现
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
/// 品质事故主应用服务
/// </summary>
public class TaktQualityIncidentService : TaktServiceBase, ITaktQualityIncidentService
{
    private readonly ITaktCompanyRepository<TaktQualityIncident> _qualityIncidentRepository;
    private readonly ITaktCompanyRepository<TaktQualityIncidentItem> _qualityIncidentItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIncidentRepository">品质事故主仓储</param>
    /// <param name="qualityIncidentItemRepository">QualityIncidentItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIncidentService(
        ITaktCompanyRepository<TaktQualityIncident> qualityIncidentRepository,
        ITaktCompanyRepository<TaktQualityIncidentItem> qualityIncidentItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIncidentRepository = qualityIncidentRepository;
        _qualityIncidentItemRepository = qualityIncidentItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质事故主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIncidentDto>> GetQualityIncidentListAsync(TaktQualityIncidentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIncidentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIncidentDto>.Create(
            data.Adapt<List<TaktQualityIncidentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentDto?> GetQualityIncidentByIdAsync(long id)
    {
        var entity = await _qualityIncidentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityIncidentDto>();
        await FillQualityIncidentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质事故主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIncidentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityIncidentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建品质事故主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentDto> CreateQualityIncidentAsync(TaktQualityIncidentCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIncident>();
        var isUnique_ix_takt_logistics_quality_incident_qi_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityIncidentCode == entity.QualityIncidentCode
                && x.IncidentDate == entity.IncidentDate);
        if (!isUnique_ix_takt_logistics_quality_incident_qi_unique)
        {
            throw new TaktBusinessException("品质事故主的PlantCode、QualityIncidentCode、IncidentDate已存在");
        }
        entity = await _qualityIncidentRepository.CreateAsync(entity);
                await SaveQualityIncidentChildrenAsync(entity, dto);
        return await GetQualityIncidentByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIncidentDto>();
    }

    /// <summary>
    /// 更新品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentDto> UpdateQualityIncidentAsync(long id, TaktQualityIncidentUpdateDto dto)
    {
        var entity = await _qualityIncidentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_incident_qi_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityIncidentCode == entity.QualityIncidentCode
                && x.IncidentDate == entity.IncidentDate,
            id);
        if (!isUnique_ix_takt_logistics_quality_incident_qi_unique)
        {
            throw new TaktBusinessException("品质事故主的PlantCode、QualityIncidentCode、IncidentDate已存在");
        }
        await _qualityIncidentRepository.UpdateAsync(entity);
                await SaveQualityIncidentChildrenAsync(entity, dto);
        return await GetQualityIncidentByIdAsync(id) ?? throw new TaktBusinessException("品质事故主不存在");
    }

    /// <summary>
    /// 删除品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentByIdAsync(long id)
    {
        var entity = await _qualityIncidentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故主不存在或已删除");
        }
        await _qualityIncidentItemRepository.DeleteAsync(x => x.QualityIncidentId == entity.Id);
        var deleted = await _qualityIncidentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质事故主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质事故主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIncidentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIncidentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIncidentTemplateDto>(
            sheetName ?? "品质事故主导入模板",
            fileName ?? "品质事故主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质事故主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIncidentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIncidentImportDto>(fileStream, sheetName ?? "品质事故主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIncident>();
                var importKey = $"{entity.PlantCode}|{entity.QualityIncidentCode}|{entity.IncidentDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityIncidentCode、IncidentDate）");
                }
                var isUnique_ix_takt_logistics_quality_incident_qi_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIncidentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityIncidentCode == entity.QualityIncidentCode
                        && x.IncidentDate == entity.IncidentDate);
                if (!isUnique_ix_takt_logistics_quality_incident_qi_unique)
                {
                    throw new TaktBusinessException("品质事故主的PlantCode、QualityIncidentCode、IncidentDate已存在");
                }
                await _qualityIncidentRepository.CreateAsync(entity);
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
    /// 导出品质事故主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIncidentAsync(TaktQualityIncidentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityIncidentQueryDto());
        var list = await _qualityIncidentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIncidentExportDto>(),
                sheetName ?? "品质事故主数据",
                fileName ?? "品质事故主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIncidentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质事故主数据",
            fileName ?? "品质事故主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充品质事故主详情（加载 OneToMany 子表：品质事故明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityIncidentDetailsAsync(TaktQualityIncidentDto dto, TaktQualityIncident entity)
    {
        if (dto == null)
        {
            return;
        }
        // 品质事故明细 → dto.IncidentItems
        var incidentitems = await _qualityIncidentItemRepository.GetListAsync(x => x.QualityIncidentId == entity.Id);
        dto.IncidentItems = incidentitems.Adapt<List<TaktQualityIncidentItemDto>>();
    }

    /// <summary>
    /// 保存品质事故主子表级联（品质事故明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityIncidentChildrenAsync(TaktQualityIncident entity, TaktQualityIncidentCreateDto dto)
    {
        // 品质事故明细（IncidentItems）
        if (dto.IncidentItems is not { Count: > 0 })
        {
            await _qualityIncidentItemRepository.DeleteAsync(x => x.QualityIncidentId == entity.Id);
        }
        else
        {
            var incidentitems = dto.IncidentItems.Adapt<List<TaktQualityIncidentItem>>();
            foreach (var child in incidentitems)
            {
                child.QualityIncidentId = entity.Id;
            }
            var incidentitemsNeedLine = incidentitems.Where(c => c.LineNumber <= 0).ToList();
            if (incidentitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIncidentCode) ? entity.QualityIncidentCode : entity.Id.ToString();
                var maxLine = await _qualityIncidentItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIncidentId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, incidentitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in incidentitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < incidentitems.Count; i++)
                        {
                            var key = $"{incidentitems[i].CompanyCode}|{incidentitems[i].QualityIncidentId}|{incidentitems[i].LineNumber}|{incidentitems[i].MaterialCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质事故明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityIncidentId、LineNumber、MaterialCode）");
                            }
                        }
            await _qualityIncidentItemRepository.DeleteAsync(x => x.QualityIncidentId == entity.Id);
            foreach (var child in incidentitems)
            {
            var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityIncidentItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityIncidentId == child.QualityIncidentId
                    && x.LineNumber == child.LineNumber
                    && x.MaterialCode == child.MaterialCode);
            if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
            {
                throw new TaktBusinessException("品质事故明细的CompanyCode、QualityIncidentId、LineNumber、MaterialCode已存在");
            }
            }
            await _qualityIncidentItemRepository.CreateRangeAsync(incidentitems);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质事故主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIncident, bool>> QueryExpression(TaktQualityIncidentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIncident>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(keywords))
                || SqlFunc.ToString(x.IndirectManpowerCostPerMinute).Contains(keywords)
                || (x.Model != null && x.Model.Contains(keywords))
                || (x.IncidentReason != null && x.IncidentReason.Contains(keywords))
                || SqlFunc.ToString(x.TotalScrapQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalScrapCost).Contains(keywords)
                || (x.CostCurrency != null && x.CostCurrency.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.IncidentDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityIncidentCode))
        {
            exp = exp.And(x => x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(queryDto.QualityIncidentCode));
        }

        if (queryDto?.IndirectManpowerCostPerMinute.HasValue == true)
        {
            exp = exp.And(x => x.IndirectManpowerCostPerMinute == queryDto.IndirectManpowerCostPerMinute);
        }

        if (!string.IsNullOrEmpty(queryDto?.Model))
        {
            exp = exp.And(x => x.Model != null && x.Model.Contains(queryDto.Model));
        }

        if (!string.IsNullOrEmpty(queryDto?.IncidentReason))
        {
            exp = exp.And(x => x.IncidentReason != null && x.IncidentReason.Contains(queryDto.IncidentReason));
        }

        if (queryDto?.TotalScrapQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalScrapQuantity == queryDto.TotalScrapQuantity);
        }

        if (queryDto?.TotalScrapCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalScrapCost == queryDto.TotalScrapCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCurrency))
        {
            exp = exp.And(x => x.CostCurrency != null && x.CostCurrency.Contains(queryDto.CostCurrency));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.IncidentDateStart.HasValue == true)
        {
            exp = exp.And(x => x.IncidentDate >= queryDto.IncidentDateStart);
        }

        if (queryDto?.IncidentDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.IncidentDate <= queryDto.IncidentDateEnd);
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
