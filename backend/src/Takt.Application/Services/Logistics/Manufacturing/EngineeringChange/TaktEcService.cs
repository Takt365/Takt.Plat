// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变主应用服务
/// </summary>
public class TaktEcService : TaktServiceBase, ITaktEcService
{
    private readonly ITaktCompanyRepository<TaktEc> _ecRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecRepository">设变主仓储</param>
    /// <param name="ecDetailRepository">EcDetail仓储</param>
    /// <param name="ecAttachmentRepository">EcAttachment仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcService(
        ITaktCompanyRepository<TaktEc> ecRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecRepository = ecRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecAttachmentRepository = ecAttachmentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcDto>> GetEcListAsync(TaktEcQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcDto>.Create(
            data.Adapt<List<TaktEcDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDto?> GetEcByIdAsync(long id)
    {
        var entity = await _ecRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEcDto>();
        await FillEcDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取设变主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ChangeStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDto> CreateEcAsync(TaktEcCreateDto dto)
    {
        var entity = dto.Adapt<TaktEc>();
        var isUnique_ix_takt_logistics_manufacturing_ec_plant_ec_no_unique = await _uniqueValidator.IsUniqueAsync(
            _ecRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EcNo == entity.EcNo);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_plant_ec_no_unique)
        {
            throw new TaktBusinessException("设变主的PlantCode、EcNo已存在");
        }
        entity = await _ecRepository.CreateAsync(entity);
                await SaveEcChildrenAsync(entity, dto);
        return await GetEcByIdAsync(entity.Id) ?? entity.Adapt<TaktEcDto>();
    }

    /// <summary>
    /// 更新设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDto> UpdateEcAsync(long id, TaktEcUpdateDto dto)
    {
        var entity = await _ecRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_ec_plant_ec_no_unique = await _uniqueValidator.IsUniqueAsync(
            _ecRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EcNo == entity.EcNo,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_plant_ec_no_unique)
        {
            throw new TaktBusinessException("设变主的PlantCode、EcNo已存在");
        }
        await _ecRepository.UpdateAsync(entity);
                await SaveEcChildrenAsync(entity, dto);
        return await GetEcByIdAsync(id) ?? throw new TaktBusinessException("设变主不存在");
    }

    /// <summary>
    /// 删除设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcByIdAsync(long id)
    {
        var entity = await _ecRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变主不存在或已删除");
        }
        await _ecDetailRepository.DeleteAsync(x => x.EcId == entity.Id);
        await _ecAttachmentRepository.DeleteAsync(x => x.EcId == entity.Id);
        var deleted = await _ecRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDto> UpdateEcStatusAsync(TaktEcStatusDto dto)
    {
        var entity = await _ecRepository.GetByIdAsync(dto.EcId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变主不存在");
        }
        entity.ChangeStatus = dto.ChangeStatus;
        await _ecRepository.UpdateAsync(entity);
        return await GetEcByIdAsync(dto.EcId) ?? throw new TaktBusinessException("设变主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcTemplateDto>(
            sheetName ?? "设变主导入模板",
            fileName ?? "设变主导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcImportDto>(fileStream, sheetName ?? "设变主导入模板");
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
                var entity = rows[i].Adapt<TaktEc>();
                var importKey = $"{entity.PlantCode}|{entity.EcNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EcNo）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_plant_ec_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EcNo == entity.EcNo);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_plant_ec_no_unique)
                {
                    throw new TaktBusinessException("设变主的PlantCode、EcNo已存在");
                }
                await _ecRepository.CreateAsync(entity);
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
    /// 导出设变主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcAsync(TaktEcQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcQueryDto());
        var list = await _ecRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcExportDto>(),
                sheetName ?? "设变主数据",
                fileName ?? "设变主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变主数据",
            fileName ?? "设变主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充设变主详情（加载 OneToMany 子表：设变明细、设变附件）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEcDetailsAsync(TaktEcDto dto, TaktEc entity)
    {
        if (dto == null)
        {
            return;
        }
        // 设变明细 → dto.EcDetails
        var ecdetails = await _ecDetailRepository.GetListAsync(x => x.EcId == entity.Id);
        dto.EcDetails = ecdetails.Adapt<List<TaktEcDetailDto>>();
        // 设变附件 → dto.Attachments
        var attachments = await _ecAttachmentRepository.GetListAsync(x => x.EcId == entity.Id);
        dto.Attachments = attachments.Adapt<List<TaktEcAttachmentDto>>();
    }

    /// <summary>
    /// 保存设变主子表级联（设变明细、设变附件；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEcChildrenAsync(TaktEc entity, TaktEcCreateDto dto)
    {
        // 设变明细（EcDetails）
        if (dto.EcDetails is not { Count: > 0 })
        {
            await _ecDetailRepository.DeleteAsync(x => x.EcId == entity.Id);
        }
        else
        {
            var ecdetails = dto.EcDetails.Adapt<List<TaktEcDetail>>();
            foreach (var child in ecdetails)
            {
                child.EcId = entity.Id;
            }
            var ecdetailsNeedLine = ecdetails.Where(c => c.LineNumber <= 0).ToList();
            if (ecdetailsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, ecdetailsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in ecdetails)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < ecdetails.Count; i++)
                        {
                            var key = $"{ecdetails[i].CompanyCode}|{ecdetails[i].EcId}|{ecdetails[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"设变明细第{i + 1}项与本次提交的其他项重复（CompanyCode、EcId、LineNumber）");
                            }
                        }
            await _ecDetailRepository.DeleteAsync(x => x.EcId == entity.Id);
            foreach (var child in ecdetails)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                _ecDetailRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcId == child.EcId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
            {
                throw new TaktBusinessException("设变明细的CompanyCode、EcId、LineNumber已存在");
            }
            }
            await _ecDetailRepository.CreateRangeAsync(ecdetails);
        }
        // 设变附件（Attachments）
        if (dto.Attachments is not { Count: > 0 })
        {
            await _ecAttachmentRepository.DeleteAsync(x => x.EcId == entity.Id);
        }
        else
        {
            var attachments = dto.Attachments.Adapt<List<TaktEcAttachment>>();
            foreach (var child in attachments)
            {
                child.EcId = entity.Id;
            }
            var attachmentsNeedLine = attachments.Where(c => c.LineNumber <= 0).ToList();
            if (attachmentsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, attachmentsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in attachments)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < attachments.Count; i++)
                        {
                            var key = $"{attachments[i].CompanyCode}|{attachments[i].EcId}|{attachments[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"设变附件第{i + 1}项与本次提交的其他项重复（CompanyCode、EcId、LineNumber）");
                            }
                        }
            await _ecAttachmentRepository.DeleteAsync(x => x.EcId == entity.Id);
            foreach (var child in attachments)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique = await _uniqueValidator.IsUniqueAsync(
                _ecAttachmentRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcId == child.EcId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique)
            {
                throw new TaktBusinessException("设变附件的CompanyCode、EcId、LineNumber已存在");
            }
            }
            await _ecAttachmentRepository.CreateRangeAsync(attachments);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEc, bool>> QueryExpression(TaktEcQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEc>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EcNo != null && x.EcNo.Contains(keywords))
                || SqlFunc.ToString(x.ChangeStatus).Contains(keywords)
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcDetailText != null && x.EcDetailText.Contains(keywords))
                || (x.EcLeader != null && x.EcLeader.Contains(keywords))
                || SqlFunc.ToString(x.EcLossAmount).Contains(keywords)
                || (x.EcDistinction != null && x.EcDistinction.Contains(keywords))
                || SqlFunc.ToString(x.EcStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EcIssueDate).Contains(keywords)
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.EcEntryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }

        if (queryDto?.ChangeStatus.HasValue == true)
        {
            exp = exp.And(x => x.ChangeStatus == queryDto.ChangeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcTitle))
        {
            exp = exp.And(x => x.EcTitle != null && x.EcTitle.Contains(queryDto.EcTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcDetailText))
        {
            exp = exp.And(x => x.EcDetailText != null && x.EcDetailText.Contains(queryDto.EcDetailText));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcLeader))
        {
            exp = exp.And(x => x.EcLeader != null && x.EcLeader.Contains(queryDto.EcLeader));
        }

        if (queryDto?.EcLossAmount.HasValue == true)
        {
            exp = exp.And(x => x.EcLossAmount == queryDto.EcLossAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcDistinction))
        {
            exp = exp.And(x => x.EcDistinction != null && x.EcDistinction.Contains(queryDto.EcDistinction));
        }

        if (queryDto?.EcStatus.HasValue == true)
        {
            exp = exp.And(x => x.EcStatus == queryDto.EcStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EcIssueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcIssueDate >= queryDto.EcIssueDateStart);
        }

        if (queryDto?.EcIssueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcIssueDate <= queryDto.EcIssueDateEnd);
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.EcEntryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate >= queryDto.EcEntryDateStart);
        }

        if (queryDto?.EcEntryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate <= queryDto.EcEntryDateEnd);
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
