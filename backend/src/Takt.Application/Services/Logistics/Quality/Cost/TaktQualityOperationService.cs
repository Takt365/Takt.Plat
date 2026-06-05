// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务主应用服务实现
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
/// 品质业务主应用服务
/// </summary>
public class TaktQualityOperationService : TaktServiceBase, ITaktQualityOperationService
{
    private readonly ITaktCompanyRepository<TaktQualityOperation> _qualityOperationRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationIncoming> _qualityOperationIncomingRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationFirstArticle> _qualityOperationFirstArticleRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationCalibration> _qualityOperationCalibrationRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationOther> _qualityOperationOtherRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationOutgoing> _qualityOperationOutgoingRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationReliability> _qualityOperationReliabilityRepository;
    private readonly ITaktCompanyRepository<TaktQualityOperationCustomerResponse> _qualityOperationCustomerResponseRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityOperationRepository">品质业务主仓储</param>
    /// <param name="qualityOperationIncomingRepository">QualityOperationIncoming仓储</param>
    /// <param name="qualityOperationFirstArticleRepository">QualityOperationFirstArticle仓储</param>
    /// <param name="qualityOperationCalibrationRepository">QualityOperationCalibration仓储</param>
    /// <param name="qualityOperationOtherRepository">QualityOperationOther仓储</param>
    /// <param name="qualityOperationOutgoingRepository">QualityOperationOutgoing仓储</param>
    /// <param name="qualityOperationReliabilityRepository">QualityOperationReliability仓储</param>
    /// <param name="qualityOperationCustomerResponseRepository">QualityOperationCustomerResponse仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityOperationService(
        ITaktCompanyRepository<TaktQualityOperation> qualityOperationRepository,
        ITaktCompanyRepository<TaktQualityOperationIncoming> qualityOperationIncomingRepository,
        ITaktCompanyRepository<TaktQualityOperationFirstArticle> qualityOperationFirstArticleRepository,
        ITaktCompanyRepository<TaktQualityOperationCalibration> qualityOperationCalibrationRepository,
        ITaktCompanyRepository<TaktQualityOperationOther> qualityOperationOtherRepository,
        ITaktCompanyRepository<TaktQualityOperationOutgoing> qualityOperationOutgoingRepository,
        ITaktCompanyRepository<TaktQualityOperationReliability> qualityOperationReliabilityRepository,
        ITaktCompanyRepository<TaktQualityOperationCustomerResponse> qualityOperationCustomerResponseRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityOperationRepository = qualityOperationRepository;
        _qualityOperationIncomingRepository = qualityOperationIncomingRepository;
        _qualityOperationFirstArticleRepository = qualityOperationFirstArticleRepository;
        _qualityOperationCalibrationRepository = qualityOperationCalibrationRepository;
        _qualityOperationOtherRepository = qualityOperationOtherRepository;
        _qualityOperationOutgoingRepository = qualityOperationOutgoingRepository;
        _qualityOperationReliabilityRepository = qualityOperationReliabilityRepository;
        _qualityOperationCustomerResponseRepository = qualityOperationCustomerResponseRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质业务主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityOperationDto>> GetQualityOperationListAsync(TaktQualityOperationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityOperationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityOperationDto>.Create(
            data.Adapt<List<TaktQualityOperationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质业务主
    /// </summary>
    /// <param name="id">品质业务主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityOperationDto?> GetQualityOperationByIdAsync(long id)
    {
        var entity = await _qualityOperationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityOperationDto>();
        await FillQualityOperationDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质业务主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityOperationOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityOperationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CustomerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CustomerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建品质业务主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityOperationDto> CreateQualityOperationAsync(TaktQualityOperationCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityOperation>();
        var isUnique_ix_takt_logistics_quality_operation_qo_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityOperationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityOperationCode == entity.QualityOperationCode
                && x.OperationMonth == entity.OperationMonth
                && x.DebitNoteNo == entity.DebitNoteNo);
        if (!isUnique_ix_takt_logistics_quality_operation_qo_unique)
        {
            throw new TaktBusinessException("品质业务主的PlantCode、QualityOperationCode、OperationMonth、DebitNoteNo已存在");
        }
        entity = await _qualityOperationRepository.CreateAsync(entity);
                await SaveQualityOperationChildrenAsync(entity, dto);
        return await GetQualityOperationByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityOperationDto>();
    }

    /// <summary>
    /// 更新品质业务主
    /// </summary>
    /// <param name="id">品质业务主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityOperationDto> UpdateQualityOperationAsync(long id, TaktQualityOperationUpdateDto dto)
    {
        var entity = await _qualityOperationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_operation_qo_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityOperationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityOperationCode == entity.QualityOperationCode
                && x.OperationMonth == entity.OperationMonth
                && x.DebitNoteNo == entity.DebitNoteNo,
            id);
        if (!isUnique_ix_takt_logistics_quality_operation_qo_unique)
        {
            throw new TaktBusinessException("品质业务主的PlantCode、QualityOperationCode、OperationMonth、DebitNoteNo已存在");
        }
        await _qualityOperationRepository.UpdateAsync(entity);
                await SaveQualityOperationChildrenAsync(entity, dto);
        return await GetQualityOperationByIdAsync(id) ?? throw new TaktBusinessException("品质业务主不存在");
    }

    /// <summary>
    /// 删除品质业务主
    /// </summary>
    /// <param name="id">品质业务主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityOperationByIdAsync(long id)
    {
        var entity = await _qualityOperationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务主不存在或已删除");
        }
        await _qualityOperationIncomingRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        await _qualityOperationFirstArticleRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        await _qualityOperationCalibrationRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        await _qualityOperationOtherRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        await _qualityOperationOutgoingRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        await _qualityOperationReliabilityRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        await _qualityOperationCustomerResponseRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        var deleted = await _qualityOperationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质业务主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质业务主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityOperationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityOperationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityOperationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityOperationTemplateDto>(
            sheetName ?? "品质业务主导入模板",
            fileName ?? "品质业务主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质业务主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityOperationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityOperationImportDto>(fileStream, sheetName ?? "品质业务主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityOperation>();
                var importKey = $"{entity.PlantCode}|{entity.QualityOperationCode}|{entity.OperationMonth}|{entity.DebitNoteNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityOperationCode、OperationMonth、DebitNoteNo）");
                }
                var isUnique_ix_takt_logistics_quality_operation_qo_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityOperationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityOperationCode == entity.QualityOperationCode
                        && x.OperationMonth == entity.OperationMonth
                        && x.DebitNoteNo == entity.DebitNoteNo);
                if (!isUnique_ix_takt_logistics_quality_operation_qo_unique)
                {
                    throw new TaktBusinessException("品质业务主的PlantCode、QualityOperationCode、OperationMonth、DebitNoteNo已存在");
                }
                await _qualityOperationRepository.CreateAsync(entity);
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
    /// 导出品质业务主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityOperationAsync(TaktQualityOperationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityOperationQueryDto());
        var list = await _qualityOperationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityOperationExportDto>(),
                sheetName ?? "品质业务主数据",
                fileName ?? "品质业务主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityOperationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质业务主数据",
            fileName ?? "品质业务主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充品质业务主详情（加载 OneToMany 子表：品质业务来料检验费用明细、品质业务初期定期检定费用明细、品质业务设备校正费用明细、品质业务其他通常业务费用明细、品质业务出货检验费用明细、品质业务信赖性评价ORT费用明细、品质业务顾客品质要求对应费用明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityOperationDetailsAsync(TaktQualityOperationDto dto, TaktQualityOperation entity)
    {
        if (dto == null)
        {
            return;
        }
        // 品质业务来料检验费用明细 → dto.IncomingItems
        var incomingitems = await _qualityOperationIncomingRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.IncomingItems = incomingitems.Adapt<List<TaktQualityOperationIncomingDto>>();
        // 品质业务初期定期检定费用明细 → dto.FirstArticleItems
        var firstarticleitems = await _qualityOperationFirstArticleRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.FirstArticleItems = firstarticleitems.Adapt<List<TaktQualityOperationFirstArticleDto>>();
        // 品质业务设备校正费用明细 → dto.CalibrationItems
        var calibrationitems = await _qualityOperationCalibrationRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.CalibrationItems = calibrationitems.Adapt<List<TaktQualityOperationCalibrationDto>>();
        // 品质业务其他通常业务费用明细 → dto.OtherItems
        var otheritems = await _qualityOperationOtherRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.OtherItems = otheritems.Adapt<List<TaktQualityOperationOtherDto>>();
        // 品质业务出货检验费用明细 → dto.OutgoingItems
        var outgoingitems = await _qualityOperationOutgoingRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.OutgoingItems = outgoingitems.Adapt<List<TaktQualityOperationOutgoingDto>>();
        // 品质业务信赖性评价ORT费用明细 → dto.ReliabilityItems
        var reliabilityitems = await _qualityOperationReliabilityRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.ReliabilityItems = reliabilityitems.Adapt<List<TaktQualityOperationReliabilityDto>>();
        // 品质业务顾客品质要求对应费用明细 → dto.CustomerResponseItems
        var customerresponseitems = await _qualityOperationCustomerResponseRepository.GetListAsync(x => x.QualityOperationId == entity.Id);
        dto.CustomerResponseItems = customerresponseitems.Adapt<List<TaktQualityOperationCustomerResponseDto>>();
    }

    /// <summary>
    /// 保存品质业务主子表级联（品质业务来料检验费用明细、品质业务初期定期检定费用明细、品质业务设备校正费用明细、品质业务其他通常业务费用明细、品质业务出货检验费用明细、品质业务信赖性评价ORT费用明细、品质业务顾客品质要求对应费用明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityOperationChildrenAsync(TaktQualityOperation entity, TaktQualityOperationCreateDto dto)
    {
        // 品质业务来料检验费用明细（IncomingItems）
        if (dto.IncomingItems is not { Count: > 0 })
        {
            await _qualityOperationIncomingRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var incomingitems = dto.IncomingItems.Adapt<List<TaktQualityOperationIncoming>>();
            foreach (var child in incomingitems)
            {
                child.QualityOperationId = entity.Id;
            }
            var incomingitemsNeedLine = incomingitems.Where(c => c.LineNumber <= 0).ToList();
            if (incomingitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationIncomingRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, incomingitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in incomingitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < incomingitems.Count; i++)
                        {
                            var key = $"{incomingitems[i].CompanyCode}|{incomingitems[i].QualityOperationId}|{incomingitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务来料检验费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationIncomingRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in incomingitems)
            {
            var isUnique_ix_takt_logistics_quality_operation_incoming_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationIncomingRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_incoming_line_unique)
            {
                throw new TaktBusinessException("品质业务来料检验费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationIncomingRepository.CreateRangeAsync(incomingitems);
        }
        // 品质业务初期定期检定费用明细（FirstArticleItems）
        if (dto.FirstArticleItems is not { Count: > 0 })
        {
            await _qualityOperationFirstArticleRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var firstarticleitems = dto.FirstArticleItems.Adapt<List<TaktQualityOperationFirstArticle>>();
            foreach (var child in firstarticleitems)
            {
                child.QualityOperationId = entity.Id;
            }
            var firstarticleitemsNeedLine = firstarticleitems.Where(c => c.LineNumber <= 0).ToList();
            if (firstarticleitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationFirstArticleRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, firstarticleitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in firstarticleitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < firstarticleitems.Count; i++)
                        {
                            var key = $"{firstarticleitems[i].CompanyCode}|{firstarticleitems[i].QualityOperationId}|{firstarticleitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务初期定期检定费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationFirstArticleRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in firstarticleitems)
            {
            var isUnique_ix_takt_logistics_quality_operation_first_article_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationFirstArticleRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_first_article_line_unique)
            {
                throw new TaktBusinessException("品质业务初期定期检定费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationFirstArticleRepository.CreateRangeAsync(firstarticleitems);
        }
        // 品质业务设备校正费用明细（CalibrationItems）
        if (dto.CalibrationItems is not { Count: > 0 })
        {
            await _qualityOperationCalibrationRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var calibrationitems = dto.CalibrationItems.Adapt<List<TaktQualityOperationCalibration>>();
            foreach (var child in calibrationitems)
            {
                child.QualityOperationId = entity.Id;
            }
            var calibrationitemsNeedLine = calibrationitems.Where(c => c.LineNumber <= 0).ToList();
            if (calibrationitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationCalibrationRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, calibrationitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in calibrationitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < calibrationitems.Count; i++)
                        {
                            var key = $"{calibrationitems[i].CompanyCode}|{calibrationitems[i].QualityOperationId}|{calibrationitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务设备校正费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationCalibrationRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in calibrationitems)
            {
            var isUnique_ix_takt_logistics_quality_operation_calibration_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationCalibrationRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_calibration_line_unique)
            {
                throw new TaktBusinessException("品质业务设备校正费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationCalibrationRepository.CreateRangeAsync(calibrationitems);
        }
        // 品质业务其他通常业务费用明细（OtherItems）
        if (dto.OtherItems is not { Count: > 0 })
        {
            await _qualityOperationOtherRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var otheritems = dto.OtherItems.Adapt<List<TaktQualityOperationOther>>();
            foreach (var child in otheritems)
            {
                child.QualityOperationId = entity.Id;
            }
            var otheritemsNeedLine = otheritems.Where(c => c.LineNumber <= 0).ToList();
            if (otheritemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationOtherRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, otheritemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in otheritems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < otheritems.Count; i++)
                        {
                            var key = $"{otheritems[i].CompanyCode}|{otheritems[i].QualityOperationId}|{otheritems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务其他通常业务费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationOtherRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in otheritems)
            {
            var isUnique_ix_takt_logistics_quality_operation_other_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationOtherRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_other_line_unique)
            {
                throw new TaktBusinessException("品质业务其他通常业务费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationOtherRepository.CreateRangeAsync(otheritems);
        }
        // 品质业务出货检验费用明细（OutgoingItems）
        if (dto.OutgoingItems is not { Count: > 0 })
        {
            await _qualityOperationOutgoingRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var outgoingitems = dto.OutgoingItems.Adapt<List<TaktQualityOperationOutgoing>>();
            foreach (var child in outgoingitems)
            {
                child.QualityOperationId = entity.Id;
            }
            var outgoingitemsNeedLine = outgoingitems.Where(c => c.LineNumber <= 0).ToList();
            if (outgoingitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationOutgoingRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, outgoingitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in outgoingitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < outgoingitems.Count; i++)
                        {
                            var key = $"{outgoingitems[i].CompanyCode}|{outgoingitems[i].QualityOperationId}|{outgoingitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务出货检验费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationOutgoingRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in outgoingitems)
            {
            var isUnique_ix_takt_logistics_quality_operation_outgoing_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationOutgoingRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_outgoing_line_unique)
            {
                throw new TaktBusinessException("品质业务出货检验费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationOutgoingRepository.CreateRangeAsync(outgoingitems);
        }
        // 品质业务信赖性评价ORT费用明细（ReliabilityItems）
        if (dto.ReliabilityItems is not { Count: > 0 })
        {
            await _qualityOperationReliabilityRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var reliabilityitems = dto.ReliabilityItems.Adapt<List<TaktQualityOperationReliability>>();
            foreach (var child in reliabilityitems)
            {
                child.QualityOperationId = entity.Id;
            }
            var reliabilityitemsNeedLine = reliabilityitems.Where(c => c.LineNumber <= 0).ToList();
            if (reliabilityitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationReliabilityRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, reliabilityitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in reliabilityitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < reliabilityitems.Count; i++)
                        {
                            var key = $"{reliabilityitems[i].CompanyCode}|{reliabilityitems[i].QualityOperationId}|{reliabilityitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务信赖性评价ORT费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationReliabilityRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in reliabilityitems)
            {
            var isUnique_ix_takt_logistics_quality_operation_reliability_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationReliabilityRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_reliability_line_unique)
            {
                throw new TaktBusinessException("品质业务信赖性评价ORT费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationReliabilityRepository.CreateRangeAsync(reliabilityitems);
        }
        // 品质业务顾客品质要求对应费用明细（CustomerResponseItems）
        if (dto.CustomerResponseItems is not { Count: > 0 })
        {
            await _qualityOperationCustomerResponseRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
        }
        else
        {
            var customerresponseitems = dto.CustomerResponseItems.Adapt<List<TaktQualityOperationCustomerResponse>>();
            foreach (var child in customerresponseitems)
            {
                child.QualityOperationId = entity.Id;
            }
            var customerresponseitemsNeedLine = customerresponseitems.Where(c => c.LineNumber <= 0).ToList();
            if (customerresponseitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityOperationCode) ? entity.QualityOperationCode : entity.Id.ToString();
                var maxLine = await _qualityOperationCustomerResponseRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityOperationId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, customerresponseitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in customerresponseitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < customerresponseitems.Count; i++)
                        {
                            var key = $"{customerresponseitems[i].CompanyCode}|{customerresponseitems[i].QualityOperationId}|{customerresponseitems[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质业务顾客品质要求对应费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityOperationId、LineNumber）");
                            }
                        }
            await _qualityOperationCustomerResponseRepository.DeleteAsync(x => x.QualityOperationId == entity.Id);
            foreach (var child in customerresponseitems)
            {
            var isUnique_ix_takt_logistics_quality_operation_customer_response_line_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityOperationCustomerResponseRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityOperationId == child.QualityOperationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_operation_customer_response_line_unique)
            {
                throw new TaktBusinessException("品质业务顾客品质要求对应费用明细的CompanyCode、QualityOperationId、LineNumber已存在");
            }
            }
            await _qualityOperationCustomerResponseRepository.CreateRangeAsync(customerresponseitems);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质业务主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityOperation, bool>> QueryExpression(TaktQualityOperationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityOperation>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityOperationCode != null && x.QualityOperationCode.Contains(keywords))
                || (x.OperationMonth != null && x.OperationMonth.Contains(keywords))
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.DebitNoteNo != null && x.DebitNoteNo.Contains(keywords))
                || (x.Recorder != null && x.Recorder.Contains(keywords))
                || SqlFunc.ToString(x.TotalQualityCost).Contains(keywords)
                || (x.CostCurrency != null && x.CostCurrency.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityOperationCode))
        {
            exp = exp.And(x => x.QualityOperationCode != null && x.QualityOperationCode.Contains(queryDto.QualityOperationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperationMonth))
        {
            exp = exp.And(x => x.OperationMonth != null && x.OperationMonth.Contains(queryDto.OperationMonth));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.DebitNoteNo))
        {
            exp = exp.And(x => x.DebitNoteNo != null && x.DebitNoteNo.Contains(queryDto.DebitNoteNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.Recorder))
        {
            exp = exp.And(x => x.Recorder != null && x.Recorder.Contains(queryDto.Recorder));
        }

        if (queryDto?.TotalQualityCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalQualityCost == queryDto.TotalQualityCost);
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
