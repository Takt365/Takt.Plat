// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureMeetingService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题会议调查试验费用明细应用服务实现
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
/// 质量问题会议调查试验费用明细应用服务
/// </summary>
public class TaktQualityFailureMeetingService : TaktServiceBase, ITaktQualityFailureMeetingService
{
    private readonly ITaktCompanyRepository<TaktQualityFailureMeeting> _qualityFailureMeetingRepository;
    private readonly ITaktCompanyRepository<TaktQualityFailure> _qualityFailureRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityFailureMeetingRepository">质量问题会议调查试验费用明细仓储</param>
    /// <param name="qualityFailureRepository">品质问题应对主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityFailureMeetingService(
        ITaktCompanyRepository<TaktQualityFailureMeeting> qualityFailureMeetingRepository,
        ITaktCompanyRepository<TaktQualityFailure> qualityFailureRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityFailureMeetingRepository = qualityFailureMeetingRepository;
        _qualityFailureRepository = qualityFailureRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取质量问题会议调查试验费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityFailureMeetingDto>> GetQualityFailureMeetingListAsync(TaktQualityFailureMeetingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityFailureMeetingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityFailureMeetingDto>.Create(
            data.Adapt<List<TaktQualityFailureMeetingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityFailureMeetingDto?> GetQualityFailureMeetingByIdAsync(long id)
    {
        var entity = await _qualityFailureMeetingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityFailureMeetingDto>();
    }

    /// <summary>
    /// 获取质量问题会议调查试验费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityFailureMeetingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityFailureMeetingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.QualityFailureCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.QualityFailureCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityFailureMeetingDto> CreateQualityFailureMeetingAsync(TaktQualityFailureMeetingCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityFailureMeeting>();
        await StampQualityFailureMeetingQualityFailureAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_failure_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityFailureMeetingRepository,
            x => x.QualityFailureId == entity.QualityFailureId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_failure_meeting_line_unique)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityFailureId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityFailureMeetingRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityFailureId == entity.QualityFailureId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityFailureCode) ? entity.QualityFailureCode : entity.QualityFailureId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityFailureMeetingRepository.CreateAsync(entity);
        return await GetQualityFailureMeetingByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityFailureMeetingDto>();
    }

    /// <summary>
    /// 更新质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityFailureMeetingDto> UpdateQualityFailureMeetingAsync(long id, TaktQualityFailureMeetingUpdateDto dto)
    {
        var entity = await _qualityFailureMeetingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityFailureMeetingQualityFailureAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_failure_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityFailureMeetingRepository,
            x => x.QualityFailureId == entity.QualityFailureId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_failure_meeting_line_unique)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityFailureId、LineNumber已存在");
        }
        await _qualityFailureMeetingRepository.UpdateAsync(entity);
        return await GetQualityFailureMeetingByIdAsync(id) ?? throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
    }

    /// <summary>
    /// 删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityFailureMeetingByIdAsync(long id)
    {
        var deleted = await _qualityFailureMeetingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityFailureMeetingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityFailureMeetingByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityFailureMeetingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityFailureMeetingTemplateDto>(
            sheetName ?? "质量问题会议调查试验费用明细导入模板",
            fileName ?? "质量问题会议调查试验费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityFailureMeetingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityFailureMeetingImportDto>(fileStream, sheetName ?? "质量问题会议调查试验费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityFailureMeeting>();
                var importDto = rows[i].Adapt<TaktQualityFailureMeetingCreateDto>();
                await StampQualityFailureMeetingQualityFailureAsync(entity, importDto);
                var importKey = $"{entity.QualityFailureId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityFailureId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_failure_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityFailureMeetingRepository,
                    x => x.QualityFailureId == entity.QualityFailureId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_failure_meeting_line_unique)
                {
                    throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityFailureId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityFailureMeetingRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityFailureId == entity.QualityFailureId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityFailureCode) ? entity.QualityFailureCode : entity.QualityFailureId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityFailureMeetingRepository.CreateAsync(entity);
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
    /// 导出质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityFailureMeetingAsync(TaktQualityFailureMeetingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityFailureMeetingQueryDto());
        var list = await _qualityFailureMeetingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityFailureMeetingExportDto>(),
                sheetName ?? "质量问题会议调查试验费用明细数据",
                fileName ?? "质量问题会议调查试验费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityFailureMeetingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "质量问题会议调查试验费用明细数据",
            fileName ?? "质量问题会议调查试验费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步质量问题会议调查试验费用明细主表外键（ManyToOne → 品质问题应对主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityFailureMeetingQualityFailureAsync(TaktQualityFailureMeeting entity, TaktQualityFailureMeetingCreateDto dto)
    {
        if (dto.QualityFailureId <= 0)
        {
            return;
        }
        var master = await _qualityFailureRepository.GetByIdAsync(dto.QualityFailureId);
        if (master == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在");
        }
        entity.QualityFailureId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建质量问题会议调查试验费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityFailureMeeting, bool>> QueryExpression(TaktQualityFailureMeetingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityFailureMeeting>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.QualityFailureId).Contains(keywords)
                || (x.QualityFailureCode != null && x.QualityFailureCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.DirectManpowerCostPerMinute).Contains(keywords)
                || SqlFunc.ToString(x.IndirectManpowerCostPerMinute).Contains(keywords)
                || (x.MeetingInvestigationContent != null && x.MeetingInvestigationContent.Contains(keywords))
                || SqlFunc.ToString(x.MeetingInvestigationCost).Contains(keywords)
                || SqlFunc.ToString(x.MeetingTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.DirectParticipantCount).Contains(keywords)
                || SqlFunc.ToString(x.IndirectParticipantCount).Contains(keywords)
                || SqlFunc.ToString(x.InvestigationWorkTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.TravelCost).Contains(keywords)
                || SqlFunc.ToString(x.OtherExpenses).Contains(keywords)
                || SqlFunc.ToString(x.OtherWorkTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.OtherApparatusCost).Contains(keywords)
                || (x.MeetingRecorder != null && x.MeetingRecorder.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.QualityFailureId.HasValue == true)
        {
            exp = exp.And(x => x.QualityFailureId == queryDto.QualityFailureId);
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityFailureCode))
        {
            exp = exp.And(x => x.QualityFailureCode != null && x.QualityFailureCode.Contains(queryDto.QualityFailureCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.DirectManpowerCostPerMinute.HasValue == true)
        {
            exp = exp.And(x => x.DirectManpowerCostPerMinute == queryDto.DirectManpowerCostPerMinute);
        }

        if (queryDto?.IndirectManpowerCostPerMinute.HasValue == true)
        {
            exp = exp.And(x => x.IndirectManpowerCostPerMinute == queryDto.IndirectManpowerCostPerMinute);
        }

        if (!string.IsNullOrEmpty(queryDto?.MeetingInvestigationContent))
        {
            exp = exp.And(x => x.MeetingInvestigationContent != null && x.MeetingInvestigationContent.Contains(queryDto.MeetingInvestigationContent));
        }

        if (queryDto?.MeetingInvestigationCost.HasValue == true)
        {
            exp = exp.And(x => x.MeetingInvestigationCost == queryDto.MeetingInvestigationCost);
        }

        if (queryDto?.MeetingTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.MeetingTimeMinutes == queryDto.MeetingTimeMinutes);
        }

        if (queryDto?.DirectParticipantCount.HasValue == true)
        {
            exp = exp.And(x => x.DirectParticipantCount == queryDto.DirectParticipantCount);
        }

        if (queryDto?.IndirectParticipantCount.HasValue == true)
        {
            exp = exp.And(x => x.IndirectParticipantCount == queryDto.IndirectParticipantCount);
        }

        if (queryDto?.InvestigationWorkTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.InvestigationWorkTimeMinutes == queryDto.InvestigationWorkTimeMinutes);
        }

        if (queryDto?.TravelCost.HasValue == true)
        {
            exp = exp.And(x => x.TravelCost == queryDto.TravelCost);
        }

        if (queryDto?.OtherExpenses.HasValue == true)
        {
            exp = exp.And(x => x.OtherExpenses == queryDto.OtherExpenses);
        }

        if (queryDto?.OtherWorkTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.OtherWorkTimeMinutes == queryDto.OtherWorkTimeMinutes);
        }

        if (queryDto?.OtherApparatusCost.HasValue == true)
        {
            exp = exp.And(x => x.OtherApparatusCost == queryDto.OtherApparatusCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.MeetingRecorder))
        {
            exp = exp.And(x => x.MeetingRecorder != null && x.MeetingRecorder.Contains(queryDto.MeetingRecorder));
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
