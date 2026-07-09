// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgendaService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会议议程纪要应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.ConferenceCenter;
using Takt.Domain.Entities.Routine.ConferenceCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.ConferenceCenter;

/// <summary>
/// 会议议程纪要应用服务
/// </summary>
public class TaktConferenceAgendaService : TaktServiceBase, ITaktConferenceAgendaService
{
    private readonly ITaktCompanyRepository<TaktConferenceAgenda> _conferenceAgendaRepository;
    private readonly ITaktApprovalRepository<TaktConference> _conferenceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceAgendaRepository">会议议程纪要仓储</param>
    /// <param name="conferenceRepository">会议中心仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConferenceAgendaService(
        ITaktCompanyRepository<TaktConferenceAgenda> conferenceAgendaRepository,
        ITaktApprovalRepository<TaktConference> conferenceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _conferenceAgendaRepository = conferenceAgendaRepository;
        _conferenceRepository = conferenceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会议议程纪要列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConferenceAgendaDto>> GetConferenceAgendaListAsync(TaktConferenceAgendaQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _conferenceAgendaRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConferenceAgendaDto>.Create(
            data.Adapt<List<TaktConferenceAgendaDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会议议程纪要
    /// </summary>
    /// <param name="id">会议议程纪要ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceAgendaDto?> GetConferenceAgendaByIdAsync(long id)
    {
        var entity = await _conferenceAgendaRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktConferenceAgendaDto>();
    }

    /// <summary>
    /// 获取会议议程纪要选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetConferenceAgendaOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _conferenceAgendaRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PresenterName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PresenterName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建会议议程纪要
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceAgendaDto> CreateConferenceAgendaAsync(TaktConferenceAgendaCreateDto dto)
    {
        var entity = dto.Adapt<TaktConferenceAgenda>();
        entity.IsObsolete = 0;
        await StampConferenceAgendaConferenceAsync(entity, dto);
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _conferenceAgendaRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConferenceId == entity.ConferenceId,
                x => x.LineNumber);
            var businessCode = entity.ConferenceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _conferenceAgendaRepository.CreateAsync(entity);
        return await GetConferenceAgendaByIdAsync(entity.Id) ?? entity.Adapt<TaktConferenceAgendaDto>();
    }

    /// <summary>
    /// 更新会议议程纪要
    /// </summary>
    /// <param name="id">会议议程纪要ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceAgendaDto> UpdateConferenceAgendaAsync(long id, TaktConferenceAgendaUpdateDto dto)
    {
        var entity = await _conferenceAgendaRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议议程纪要不存在");
        }
        dto.Adapt(entity);
        await StampConferenceAgendaConferenceAsync(entity, dto);
        await _conferenceAgendaRepository.UpdateAsync(entity);
        return await GetConferenceAgendaByIdAsync(id) ?? throw new TaktBusinessException("会议议程纪要不存在");
    }

    /// <summary>
    /// 删除会议议程纪要
    /// </summary>
    /// <param name="id">会议议程纪要ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConferenceAgendaByIdAsync(long id)
    {
        var entity = await _conferenceAgendaRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议议程纪要不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会议议程纪要不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("会议议程纪要已作废");
        }
        entity.IsObsolete = 1;
        await _conferenceAgendaRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除会议议程纪要
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConferenceAgendaBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteConferenceAgendaByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会议议程纪要作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceAgendaDto> UpdateConferenceAgendaObsoleteAsync(TaktConferenceAgendaObsoleteDto dto)
    {
        var entity = await _conferenceAgendaRepository.GetByIdAsync(dto.ConferenceAgendaId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议议程纪要不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会议议程纪要不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _conferenceAgendaRepository.UpdateAsync(entity);
        return await GetConferenceAgendaByIdAsync(dto.ConferenceAgendaId) ?? throw new TaktBusinessException("会议议程纪要不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConferenceAgendaTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConferenceAgendaTemplateDto>(
            sheetName ?? "会议议程纪要导入模板",
            fileName ?? "会议议程纪要导入模板.xlsx");
    }

    /// <summary>
    /// 导入会议议程纪要
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConferenceAgendaAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConferenceAgendaImportDto>(fileStream, sheetName ?? "会议议程纪要导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktConferenceAgenda>();
                var importDto = rows[i].Adapt<TaktConferenceAgendaCreateDto>();
                await StampConferenceAgendaConferenceAsync(entity, importDto);
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _conferenceAgendaRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConferenceId == entity.ConferenceId,
                        x => x.LineNumber);
                    var businessCode = entity.ConferenceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _conferenceAgendaRepository.CreateAsync(entity);
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
    /// 导出会议议程纪要
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConferenceAgendaAsync(TaktConferenceAgendaQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktConferenceAgendaQueryDto());
        var list = await _conferenceAgendaRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConferenceAgendaExportDto>(),
                sheetName ?? "会议议程纪要数据",
                fileName ?? "会议议程纪要导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConferenceAgendaExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会议议程纪要数据",
            fileName ?? "会议议程纪要导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步会议议程纪要主表外键（ManyToOne → 会议中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampConferenceAgendaConferenceAsync(TaktConferenceAgenda entity, TaktConferenceAgendaCreateDto dto)
    {
        if (dto.ConferenceId <= 0)
        {
            return;
        }
        var master = await _conferenceRepository.GetByIdAsync(dto.ConferenceId);
        if (master == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        entity.ConferenceId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会议议程纪要查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktConferenceAgenda, bool>> QueryExpression(TaktConferenceAgendaQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConferenceAgenda>();

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
                SqlFunc.ToString(x.ConferenceId).Contains(keywords)
                || SqlFunc.ToString(x.RecordType).Contains(keywords)
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.ConferenceAgendaTitle != null && x.ConferenceAgendaTitle.Contains(keywords))
                || (x.ConferenceAgendaContent != null && x.ConferenceAgendaContent.Contains(keywords))
                || (x.ConferenceAgendaSummary != null && x.ConferenceAgendaSummary.Contains(keywords))
                || SqlFunc.ToString(x.PresenterId).Contains(keywords)
                || (x.PresenterName != null && x.PresenterName.Contains(keywords))
                || SqlFunc.ToString(x.DurationMinutes).Contains(keywords)
                || SqlFunc.ToString(x.RecorderId).Contains(keywords)
                || (x.RecorderName != null && x.RecorderName.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ConferenceId.HasValue == true)
        {
            exp = exp.And(x => x.ConferenceId == queryDto.ConferenceId);
        }

        if (queryDto?.RecordType.HasValue == true)
        {
            exp = exp.And(x => x.RecordType == queryDto.RecordType);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.ConferenceAgendaTitle))
        {
            exp = exp.And(x => x.ConferenceAgendaTitle != null && x.ConferenceAgendaTitle.Contains(queryDto.ConferenceAgendaTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConferenceAgendaContent))
        {
            exp = exp.And(x => x.ConferenceAgendaContent != null && x.ConferenceAgendaContent.Contains(queryDto.ConferenceAgendaContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConferenceAgendaSummary))
        {
            exp = exp.And(x => x.ConferenceAgendaSummary != null && x.ConferenceAgendaSummary.Contains(queryDto.ConferenceAgendaSummary));
        }

        if (queryDto?.PresenterId.HasValue == true)
        {
            exp = exp.And(x => x.PresenterId == queryDto.PresenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PresenterName))
        {
            exp = exp.And(x => x.PresenterName != null && x.PresenterName.Contains(queryDto.PresenterName));
        }

        if (queryDto?.DurationMinutes.HasValue == true)
        {
            exp = exp.And(x => x.DurationMinutes == queryDto.DurationMinutes);
        }

        if (queryDto?.RecorderId.HasValue == true)
        {
            exp = exp.And(x => x.RecorderId == queryDto.RecorderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.RecorderName))
        {
            exp = exp.And(x => x.RecorderName != null && x.RecorderName.Contains(queryDto.RecorderName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime >= queryDto.PlannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime <= queryDto.PlannedStartTimeEnd);
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
