// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：品质问题应对主应用服务实现
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
/// 品质问题应对主应用服务
/// </summary>
public class TaktQualityFailureService : TaktServiceBase, ITaktQualityFailureService
{
    private readonly ITaktCompanyRepository<TaktQualityFailure> _qualityFailureRepository;
    private readonly ITaktCompanyRepository<TaktQualityFailureMeeting> _qualityFailureMeetingRepository;
    private readonly ITaktCompanyRepository<TaktQualityFailureAssyRework> _qualityFailureAssyReworkRepository;
    private readonly ITaktCompanyRepository<TaktQualityFailurePcbaRework> _qualityFailurePcbaReworkRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityFailureRepository">品质问题应对主仓储</param>
    /// <param name="qualityFailureMeetingRepository">QualityFailureMeeting仓储</param>
    /// <param name="qualityFailureAssyReworkRepository">QualityFailureAssyRework仓储</param>
    /// <param name="qualityFailurePcbaReworkRepository">QualityFailurePcbaRework仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityFailureService(
        ITaktCompanyRepository<TaktQualityFailure> qualityFailureRepository,
        ITaktCompanyRepository<TaktQualityFailureMeeting> qualityFailureMeetingRepository,
        ITaktCompanyRepository<TaktQualityFailureAssyRework> qualityFailureAssyReworkRepository,
        ITaktCompanyRepository<TaktQualityFailurePcbaRework> qualityFailurePcbaReworkRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityFailureRepository = qualityFailureRepository;
        _qualityFailureMeetingRepository = qualityFailureMeetingRepository;
        _qualityFailureAssyReworkRepository = qualityFailureAssyReworkRepository;
        _qualityFailurePcbaReworkRepository = qualityFailurePcbaReworkRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质问题应对主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityFailureDto>> GetQualityFailureListAsync(TaktQualityFailureQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityFailureRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityFailureDto>.Create(
            data.Adapt<List<TaktQualityFailureDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityFailureDto?> GetQualityFailureByIdAsync(long id)
    {
        var entity = await _qualityFailureRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityFailureDto>();
        await FillQualityFailureDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质问题应对主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityFailureOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityFailureRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建品质问题应对主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityFailureDto> CreateQualityFailureAsync(TaktQualityFailureCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityFailure>();
        var isUnique_ix_takt_logistics_quality_failure_qf_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityFailureRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityFailureCode == entity.QualityFailureCode
                && x.FailureDate == entity.FailureDate);
        if (!isUnique_ix_takt_logistics_quality_failure_qf_unique)
        {
            throw new TaktBusinessException("品质问题应对主的PlantCode、QualityFailureCode、FailureDate已存在");
        }
        entity = await _qualityFailureRepository.CreateAsync(entity);
                await SaveQualityFailureChildrenAsync(entity, dto);
        return await GetQualityFailureByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityFailureDto>();
    }

    /// <summary>
    /// 更新品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityFailureDto> UpdateQualityFailureAsync(long id, TaktQualityFailureUpdateDto dto)
    {
        var entity = await _qualityFailureRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_failure_qf_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityFailureRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityFailureCode == entity.QualityFailureCode
                && x.FailureDate == entity.FailureDate,
            id);
        if (!isUnique_ix_takt_logistics_quality_failure_qf_unique)
        {
            throw new TaktBusinessException("品质问题应对主的PlantCode、QualityFailureCode、FailureDate已存在");
        }
        await _qualityFailureRepository.UpdateAsync(entity);
                await SaveQualityFailureChildrenAsync(entity, dto);
        return await GetQualityFailureByIdAsync(id) ?? throw new TaktBusinessException("品质问题应对主不存在");
    }

    /// <summary>
    /// 删除品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityFailureByIdAsync(long id)
    {
        var entity = await _qualityFailureRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在或已删除");
        }
        await _qualityFailureMeetingRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
        await _qualityFailureAssyReworkRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
        await _qualityFailurePcbaReworkRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
        var deleted = await _qualityFailureRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质问题应对主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质问题应对主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityFailureBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityFailureByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityFailureTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityFailureTemplateDto>(
            sheetName ?? "品质问题应对主导入模板",
            fileName ?? "品质问题应对主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质问题应对主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityFailureAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityFailureImportDto>(fileStream, sheetName ?? "品质问题应对主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityFailure>();
                var importKey = $"{entity.PlantCode}|{entity.QualityFailureCode}|{entity.FailureDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityFailureCode、FailureDate）");
                }
                var isUnique_ix_takt_logistics_quality_failure_qf_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityFailureRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityFailureCode == entity.QualityFailureCode
                        && x.FailureDate == entity.FailureDate);
                if (!isUnique_ix_takt_logistics_quality_failure_qf_unique)
                {
                    throw new TaktBusinessException("品质问题应对主的PlantCode、QualityFailureCode、FailureDate已存在");
                }
                await _qualityFailureRepository.CreateAsync(entity);
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
    /// 导出品质问题应对主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityFailureAsync(TaktQualityFailureQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityFailureQueryDto());
        var list = await _qualityFailureRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityFailureExportDto>(),
                sheetName ?? "品质问题应对主数据",
                fileName ?? "品质问题应对主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityFailureExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质问题应对主数据",
            fileName ?? "品质问题应对主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充品质问题应对主详情（加载 OneToMany 子表：质量问题会议调查试验费用明细、质量问题组装不良改修费用明细、质量问题PCBA不良改修费用明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityFailureDetailsAsync(TaktQualityFailureDto dto, TaktQualityFailure entity)
    {
        if (dto == null)
        {
            return;
        }
        // 质量问题会议调查试验费用明细 → dto.MeetingItems
        var meetingitems = await _qualityFailureMeetingRepository.GetListAsync(x => x.QualityFailureId == entity.Id);
        dto.MeetingItems = meetingitems.Adapt<List<TaktQualityFailureMeetingDto>>();
        // 质量问题组装不良改修费用明细 → dto.AssyReworkItems
        var assyreworkitems = await _qualityFailureAssyReworkRepository.GetListAsync(x => x.QualityFailureId == entity.Id);
        dto.AssyReworkItems = assyreworkitems.Adapt<List<TaktQualityFailureAssyReworkDto>>();
        // 质量问题PCBA不良改修费用明细 → dto.PcbaReworkItems
        var pcbareworkitems = await _qualityFailurePcbaReworkRepository.GetListAsync(x => x.QualityFailureId == entity.Id);
        dto.PcbaReworkItems = pcbareworkitems.Adapt<List<TaktQualityFailurePcbaReworkDto>>();
    }

    /// <summary>
    /// 保存品质问题应对主子表级联（质量问题会议调查试验费用明细、质量问题组装不良改修费用明细、质量问题PCBA不良改修费用明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityFailureChildrenAsync(TaktQualityFailure entity, TaktQualityFailureCreateDto dto)
    {
        // 质量问题会议调查试验费用明细（MeetingItems）
        if (dto.MeetingItems is not { Count: > 0 })
        {
            await _qualityFailureMeetingRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
        }
        else
        {
            var meetingitems = dto.MeetingItems.Adapt<List<TaktQualityFailureMeeting>>();
            foreach (var child in meetingitems)
            {
                child.QualityFailureId = entity.Id;
            }
            var meetingitemsNeedLine = meetingitems.Where(c => c.LineNumber <= 0).ToList();
            if (meetingitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityFailureCode) ? entity.QualityFailureCode : entity.Id.ToString();
                var maxLine = await _qualityFailureMeetingRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityFailureId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, meetingitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in meetingitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < meetingitems.Count; i++)
                        {
                            var key = $"{meetingitems[i].CompanyCode}|{meetingitems[i].QualityFailureId}|{meetingitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"质量问题会议调查试验费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityFailureId、LineNumber）");
                            }
                        }
            await _qualityFailureMeetingRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
            foreach (var child in meetingitems)
            {
            var isUnique_ix_takt_logistics_quality_failure_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityFailureMeetingRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityFailureId == child.QualityFailureId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_failure_meeting_line_unique)
            {
                throw new TaktBusinessException("质量问题会议调查试验费用明细的CompanyCode、QualityFailureId、LineNumber已存在");
            }
            }
            await _qualityFailureMeetingRepository.CreateRangeAsync(meetingitems);
        }
        // 质量问题组装不良改修费用明细（AssyReworkItems）
        if (dto.AssyReworkItems is not { Count: > 0 })
        {
            await _qualityFailureAssyReworkRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
        }
        else
        {
            var assyreworkitems = dto.AssyReworkItems.Adapt<List<TaktQualityFailureAssyRework>>();
            foreach (var child in assyreworkitems)
            {
                child.QualityFailureId = entity.Id;
            }
            var assyreworkitemsNeedLine = assyreworkitems.Where(c => c.LineNumber <= 0).ToList();
            if (assyreworkitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityFailureCode) ? entity.QualityFailureCode : entity.Id.ToString();
                var maxLine = await _qualityFailureAssyReworkRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityFailureId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, assyreworkitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in assyreworkitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < assyreworkitems.Count; i++)
                        {
                            var key = $"{assyreworkitems[i].CompanyCode}|{assyreworkitems[i].QualityFailureId}|{assyreworkitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"质量问题组装不良改修费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityFailureId、LineNumber）");
                            }
                        }
            await _qualityFailureAssyReworkRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
            foreach (var child in assyreworkitems)
            {
            var isUnique_ix_takt_logistics_quality_failure_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityFailureAssyReworkRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityFailureId == child.QualityFailureId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_failure_assy_rework_line_unique)
            {
                throw new TaktBusinessException("质量问题组装不良改修费用明细的CompanyCode、QualityFailureId、LineNumber已存在");
            }
            }
            await _qualityFailureAssyReworkRepository.CreateRangeAsync(assyreworkitems);
        }
        // 质量问题PCBA不良改修费用明细（PcbaReworkItems）
        if (dto.PcbaReworkItems is not { Count: > 0 })
        {
            await _qualityFailurePcbaReworkRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
        }
        else
        {
            var pcbareworkitems = dto.PcbaReworkItems.Adapt<List<TaktQualityFailurePcbaRework>>();
            foreach (var child in pcbareworkitems)
            {
                child.QualityFailureId = entity.Id;
            }
            var pcbareworkitemsNeedLine = pcbareworkitems.Where(c => c.LineNumber <= 0).ToList();
            if (pcbareworkitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityFailureCode) ? entity.QualityFailureCode : entity.Id.ToString();
                var maxLine = await _qualityFailurePcbaReworkRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityFailureId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, pcbareworkitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in pcbareworkitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < pcbareworkitems.Count; i++)
                        {
                            var key = $"{pcbareworkitems[i].CompanyCode}|{pcbareworkitems[i].QualityFailureId}|{pcbareworkitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"质量问题PCBA不良改修费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityFailureId、LineNumber）");
                            }
                        }
            await _qualityFailurePcbaReworkRepository.DeleteAsync(x => x.QualityFailureId == entity.Id);
            foreach (var child in pcbareworkitems)
            {
            var isUnique_ix_takt_logistics_quality_failure_pcba_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityFailurePcbaReworkRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityFailureId == child.QualityFailureId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_failure_pcba_rework_line_unique)
            {
                throw new TaktBusinessException("质量问题PCBA不良改修费用明细的CompanyCode、QualityFailureId、LineNumber已存在");
            }
            }
            await _qualityFailurePcbaReworkRepository.CreateRangeAsync(pcbareworkitems);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质问题应对主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityFailure, bool>> QueryExpression(TaktQualityFailureQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityFailure>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityFailureCode != null && x.QualityFailureCode.Contains(keywords))
                || (x.Model != null && x.Model.Contains(keywords))
                || (x.Lot != null && x.Lot.Contains(keywords))
                || (x.QualityProblemsResponse != null && x.QualityProblemsResponse.Contains(keywords))
                || (x.ReworkDueToDefects != null && x.ReworkDueToDefects.Contains(keywords))
                || (x.NeedRework != null && x.NeedRework.Contains(keywords))
                || SqlFunc.ToString(x.TotalTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.TotalCost).Contains(keywords)
                || (x.CostCurrency != null && x.CostCurrency.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.FailureDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityFailureCode))
        {
            exp = exp.And(x => x.QualityFailureCode != null && x.QualityFailureCode.Contains(queryDto.QualityFailureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Model))
        {
            exp = exp.And(x => x.Model != null && x.Model.Contains(queryDto.Model));
        }

        if (!string.IsNullOrEmpty(queryDto?.Lot))
        {
            exp = exp.And(x => x.Lot != null && x.Lot.Contains(queryDto.Lot));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityProblemsResponse))
        {
            exp = exp.And(x => x.QualityProblemsResponse != null && x.QualityProblemsResponse.Contains(queryDto.QualityProblemsResponse));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReworkDueToDefects))
        {
            exp = exp.And(x => x.ReworkDueToDefects != null && x.ReworkDueToDefects.Contains(queryDto.ReworkDueToDefects));
        }

        if (!string.IsNullOrEmpty(queryDto?.NeedRework))
        {
            exp = exp.And(x => x.NeedRework != null && x.NeedRework.Contains(queryDto.NeedRework));
        }

        if (queryDto?.TotalTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.TotalTimeMinutes == queryDto.TotalTimeMinutes);
        }

        if (queryDto?.TotalCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalCost == queryDto.TotalCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCurrency))
        {
            exp = exp.And(x => x.CostCurrency != null && x.CostCurrency.Contains(queryDto.CostCurrency));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.FailureDateStart.HasValue == true)
        {
            exp = exp.And(x => x.FailureDate >= queryDto.FailureDateStart);
        }

        if (queryDto?.FailureDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.FailureDate <= queryDto.FailureDateEnd);
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
