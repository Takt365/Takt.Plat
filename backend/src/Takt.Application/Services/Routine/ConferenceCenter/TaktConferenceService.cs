// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.ConferenceCenter
// 文件名称：TaktConferenceService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心应用服务实现
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
using Takt.Domain.Entities.Routine.ConferenceCenter;

namespace Takt.Application.Services.Routine.ConferenceCenter;

/// <summary>
/// 会议中心应用服务
/// </summary>
public class TaktConferenceService : TaktServiceBase, ITaktConferenceService
{
    private readonly ITaktCompanyRepository<TaktConference> _conferenceRepository;
    private readonly ITaktCompanyRepository<TaktConferenceParticipant> _conferenceParticipantRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceRepository">会议中心仓储</param>
    /// <param name="conferenceParticipantRepository">ConferenceParticipant仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConferenceService(
        ITaktCompanyRepository<TaktConference> conferenceRepository,
        ITaktCompanyRepository<TaktConferenceParticipant> conferenceParticipantRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _conferenceRepository = conferenceRepository;
        _conferenceParticipantRepository = conferenceParticipantRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会议中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConferenceDto>> GetConferenceListAsync(TaktConferenceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _conferenceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConferenceDto>.Create(
            data.Adapt<List<TaktConferenceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceDto?> GetConferenceByIdAsync(long id)
    {
        var entity = await _conferenceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktConferenceDto>();
        await FillConferenceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取会议中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetConferenceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _conferenceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.OrganizerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.OrganizerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建会议中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceDto> CreateConferenceAsync(TaktConferenceCreateDto dto)
    {
        var entity = dto.Adapt<TaktConference>();
        var isUnique_ix_conference_code_unique = await _uniqueValidator.IsUniqueAsync(
            _conferenceRepository,
            x => x.ConferenceCode == entity.ConferenceCode);
        if (!isUnique_ix_conference_code_unique)
        {
            throw new TaktBusinessException("会议中心的ConferenceCode已存在");
        }
        entity = await _conferenceRepository.CreateAsync(entity);
                await SaveConferenceChildrenAsync(entity, dto);
        return await GetConferenceByIdAsync(entity.Id) ?? entity.Adapt<TaktConferenceDto>();
    }

    /// <summary>
    /// 更新会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceDto> UpdateConferenceAsync(long id, TaktConferenceUpdateDto dto)
    {
        var entity = await _conferenceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_conference_code_unique = await _uniqueValidator.IsUniqueAsync(
            _conferenceRepository,
            x => x.ConferenceCode == entity.ConferenceCode,
            id);
        if (!isUnique_ix_conference_code_unique)
        {
            throw new TaktBusinessException("会议中心的ConferenceCode已存在");
        }
        await _conferenceRepository.UpdateAsync(entity);
                await SaveConferenceChildrenAsync(entity, dto);
        return await GetConferenceByIdAsync(id) ?? throw new TaktBusinessException("会议中心不存在");
    }

    /// <summary>
    /// 删除会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConferenceByIdAsync(long id)
    {
        var entity = await _conferenceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议中心不存在或已删除");
        }
        await _conferenceParticipantRepository.DeleteAsync(x => x.ConferenceId == entity.Id);
        var deleted = await _conferenceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会议中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会议中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConferenceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteConferenceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会议中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceDto> UpdateConferenceStatusAsync(TaktConferenceStatusDto dto)
    {
        var entity = await _conferenceRepository.GetByIdAsync(dto.ConferenceId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        entity.ConferenceStatus = dto.ConferenceStatus;
        await _conferenceRepository.UpdateAsync(entity);
        return await GetConferenceByIdAsync(dto.ConferenceId) ?? throw new TaktBusinessException("会议中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConferenceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConferenceTemplateDto>(
            sheetName ?? "会议中心导入模板",
            fileName ?? "会议中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入会议中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConferenceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConferenceImportDto>(fileStream, sheetName ?? "会议中心导入模板");
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
                var entity = rows[i].Adapt<TaktConference>();
                var importKey = $"{entity.ConferenceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ConferenceCode）");
                }
                var isUnique_ix_conference_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _conferenceRepository,
                    x => x.ConferenceCode == entity.ConferenceCode);
                if (!isUnique_ix_conference_code_unique)
                {
                    throw new TaktBusinessException("会议中心的ConferenceCode已存在");
                }
                await _conferenceRepository.CreateAsync(entity);
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
    /// 导出会议中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConferenceAsync(TaktConferenceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktConferenceQueryDto());
        var list = await _conferenceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConferenceExportDto>(),
                sheetName ?? "会议中心数据",
                fileName ?? "会议中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConferenceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会议中心数据",
            fileName ?? "会议中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充会议中心详情（加载 OneToMany 子表：会议参与人）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillConferenceDetailsAsync(TaktConferenceDto dto, TaktConference entity)
    {
        if (dto == null)
        {
            return;
        }
        // 会议参与人 → dto.Participants
        var participants = await _conferenceParticipantRepository.GetListAsync(x => x.ConferenceId == entity.Id);
        dto.Participants = participants.Adapt<List<TaktConferenceParticipantDto>>();
    }

    /// <summary>
    /// 保存会议中心子表级联（会议参与人；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveConferenceChildrenAsync(TaktConference entity, TaktConferenceCreateDto dto)
    {
        // 会议参与人（Participants）
        if (dto.Participants is not { Count: > 0 })
        {
            await _conferenceParticipantRepository.DeleteAsync(x => x.ConferenceId == entity.Id);
        }
        else
        {
            var participants = dto.Participants.Adapt<List<TaktConferenceParticipant>>();
            foreach (var child in participants)
            {
                child.ConferenceId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < participants.Count; i++)
                        {
                            var key = $"{participants[i].CompanyCode}|{participants[i].ConferenceId}|{participants[i].UserId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"会议参与人第{i + 1}项与本次提交的其他项重复（CompanyCode、ConferenceId、UserId）");
                            }
                        }
            await _conferenceParticipantRepository.DeleteAsync(x => x.ConferenceId == entity.Id);
            foreach (var child in participants)
            {
            var isUnique_ix_conference_participant_unique = await _uniqueValidator.IsUniqueAsync(
                _conferenceParticipantRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ConferenceId == child.ConferenceId
                    && x.UserId == child.UserId);
            if (!isUnique_ix_conference_participant_unique)
            {
                throw new TaktBusinessException("会议参与人的CompanyCode、ConferenceId、UserId已存在");
            }
            }
            await _conferenceParticipantRepository.CreateRangeAsync(participants);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会议中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktConference, bool>> QueryExpression(TaktConferenceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConference>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ConferenceCode != null && x.ConferenceCode.Contains(keywords))
                || (x.Title != null && x.Title.Contains(keywords))
                || SqlFunc.ToString(x.ConferenceType).Contains(keywords)
                || SqlFunc.ToString(x.ConferenceStatus).Contains(keywords)
                || (x.Location != null && x.Location.Contains(keywords))
                || (x.MeetingLink != null && x.MeetingLink.Contains(keywords))
                || (x.Agenda != null && x.Agenda.Contains(keywords))
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.Summary != null && x.Summary.Contains(keywords))
                || (x.Tags != null && x.Tags.Contains(keywords))
                || SqlFunc.ToString(x.OrganizerId).Contains(keywords)
                || (x.OrganizerName != null && x.OrganizerName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.MaxParticipants).Contains(keywords)
                || SqlFunc.ToString(x.ReminderMinutes).Contains(keywords)
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartTime).Contains(keywords)
                || SqlFunc.ToString(x.EndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ConferenceCode))
        {
            exp = exp.And(x => x.ConferenceCode != null && x.ConferenceCode.Contains(queryDto.ConferenceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Title))
        {
            exp = exp.And(x => x.Title != null && x.Title.Contains(queryDto.Title));
        }

        if (queryDto?.ConferenceType.HasValue == true)
        {
            exp = exp.And(x => x.ConferenceType == queryDto.ConferenceType);
        }

        if (queryDto?.ConferenceStatus.HasValue == true)
        {
            exp = exp.And(x => x.ConferenceStatus == queryDto.ConferenceStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.Location))
        {
            exp = exp.And(x => x.Location != null && x.Location.Contains(queryDto.Location));
        }

        if (!string.IsNullOrEmpty(queryDto?.MeetingLink))
        {
            exp = exp.And(x => x.MeetingLink != null && x.MeetingLink.Contains(queryDto.MeetingLink));
        }

        if (!string.IsNullOrEmpty(queryDto?.Agenda))
        {
            exp = exp.And(x => x.Agenda != null && x.Agenda.Contains(queryDto.Agenda));
        }

        if (!string.IsNullOrEmpty(queryDto?.Content))
        {
            exp = exp.And(x => x.Content != null && x.Content.Contains(queryDto.Content));
        }

        if (!string.IsNullOrEmpty(queryDto?.Summary))
        {
            exp = exp.And(x => x.Summary != null && x.Summary.Contains(queryDto.Summary));
        }

        if (!string.IsNullOrEmpty(queryDto?.Tags))
        {
            exp = exp.And(x => x.Tags != null && x.Tags.Contains(queryDto.Tags));
        }

        if (queryDto?.OrganizerId.HasValue == true)
        {
            exp = exp.And(x => x.OrganizerId == queryDto.OrganizerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.OrganizerName))
        {
            exp = exp.And(x => x.OrganizerName != null && x.OrganizerName.Contains(queryDto.OrganizerName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.MaxParticipants.HasValue == true)
        {
            exp = exp.And(x => x.MaxParticipants == queryDto.MaxParticipants);
        }

        if (queryDto?.ReminderMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ReminderMinutes == queryDto.ReminderMinutes);
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.StartTime >= queryDto.StartTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartTime <= queryDto.StartTimeEnd);
        }

        if (queryDto?.EndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.EndTime >= queryDto.EndTimeStart);
        }

        if (queryDto?.EndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndTime <= queryDto.EndTimeEnd);
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
