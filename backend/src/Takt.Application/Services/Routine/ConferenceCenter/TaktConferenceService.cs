// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.ConferenceCenter
// 文件名称：TaktConferenceService.cs
// 创建时间：2026-08-22
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

namespace Takt.Application.Services.Routine.ConferenceCenter;

/// <summary>
/// 会议中心应用服务
/// </summary>
public class TaktConferenceService : TaktServiceBase, ITaktConferenceService
{
    private readonly ITaktApprovalRepository<TaktConference> _conferenceRepository;
    private readonly ITaktCompanyRepository<TaktConferenceParticipant> _conferenceParticipantRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceRepository">会议中心仓储</param>
    /// <param name="conferenceParticipantRepository">ConferenceParticipant仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConferenceService(
        ITaktApprovalRepository<TaktConference> conferenceRepository,
        ITaktCompanyRepository<TaktConferenceParticipant> conferenceParticipantRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _conferenceRepository = conferenceRepository;
        _conferenceParticipantRepository = conferenceParticipantRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取会议中心列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConferenceDto>> GetConferenceListAsync(TaktConferenceQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktConferenceDto>.Create(
                new List<TaktConferenceDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
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
    /// 获取会议中心选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetConferenceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _conferenceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConferenceStatus == 1,
            x => x.OrganizerName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ConferenceCode,
            DictLabel = e.OrganizerName ?? e.ConferenceCode,
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
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.ConferenceCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.ConferenceCode))
        {
            throw new TaktBusinessException("会议编码不能为空");
        }
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
        var queryDto = query ?? new TaktConferenceQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConferenceExportDto>(),
                sheetName ?? "会议中心数据",
                fileName ?? "会议中心导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
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
    /// 保存会议中心子表级联（会议参与人；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveConferenceChildrenAsync(TaktConference entity, TaktConferenceCreateDto dto)
    {
        // 会议参与人（Participants）
        List<TaktConferenceParticipantUpdateDto>? participantsForSave;
        if (dto is TaktConferenceUpdateDto updateDtoForParticipants && updateDtoForParticipants.Participants != null)
        {
            participantsForSave = updateDtoForParticipants.Participants;
        }
        else if (dto.Participants != null)
        {
            participantsForSave = dto.Participants.Adapt<List<TaktConferenceParticipantUpdateDto>>();
        }
        else
        {
            participantsForSave = null;
        }
        if (participantsForSave is not { Count: > 0 })
        {
            await MarkConferenceParticipantsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _conferenceParticipantRepository.GetListAsync(x => x.ConferenceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktConferenceParticipant>();
            for (var i = 0; i < participantsForSave.Count; i++)
            {
                var childDto = participantsForSave[i];
                childDto.ConferenceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.ConferenceParticipantId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ConferenceParticipantId, out var target))
                    {
                        throw new TaktBusinessException("会议参与人不存在（ConferenceParticipantId={childDto.ConferenceParticipantId}）");
                    }
                    if (target.ConferenceId != entity.Id)
                    {
                        throw new TaktBusinessException("会议参与人不属于当前主表（ConferenceParticipantId={childDto.ConferenceParticipantId}）");
                    }
                    submittedIds.Add(childDto.ConferenceParticipantId);
                    childDto.Adapt(target);
                    target.Id = childDto.ConferenceParticipantId;
                    target.ConferenceId = entity.Id;
                    target.IsObsolete = 0;
                    await _conferenceParticipantRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktConferenceParticipant>();
                    child.Id = 0;
                    child.ConferenceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _conferenceParticipantRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ConferenceCode) ? entity.ConferenceCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _conferenceParticipantRepository.CreateRangeAsync(toCreate);
            }
        }
    }

    /// <summary>
    /// 将会议参与人子表未作废行全部标记作废
    /// </summary>
    /// <param name="conferenceId">会议 ID</param>
    /// <returns>任务</returns>
    private async Task MarkConferenceParticipantsObsoleteAsync(long conferenceId)
    {
        if (conferenceId <= 0)
        {
            return;
        }
        var rows = await _conferenceParticipantRepository.GetListAsync(
            x => x.ConferenceId == conferenceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _conferenceParticipantRepository.UpdateRangeAsync(rows);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ConferenceCode != null && x.ConferenceCode.Contains(keywords))
                || (x.ConferenceTitle != null && x.ConferenceTitle.Contains(keywords))
                || (x.Location != null && x.Location.Contains(keywords))
                || (x.MeetingLink != null && x.MeetingLink.Contains(keywords))
                || (x.Agenda != null && x.Agenda.Contains(keywords))
                || (x.ConferenceContent != null && x.ConferenceContent.Contains(keywords))
                || (x.ConferenceSummary != null && x.ConferenceSummary.Contains(keywords))
                || (x.ConferenceTags != null && x.ConferenceTags.Contains(keywords))
                || (x.OrganizerName != null && x.OrganizerName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.ConferenceRoomName != null && x.ConferenceRoomName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConferenceCode))
        {
            var conferenceCode = queryDto.ConferenceCode;
            exp = exp.And(x => x.ConferenceCode != null && x.ConferenceCode.Contains(conferenceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConferenceTitle))
        {
            var conferenceTitle = queryDto.ConferenceTitle;
            exp = exp.And(x => x.ConferenceTitle != null && x.ConferenceTitle.Contains(conferenceTitle));
        }

        if (queryDto?.ConferenceType.HasValue == true)
        {
            var conferenceType = queryDto.ConferenceType.Value;
            exp = exp.And(x => x.ConferenceType == conferenceType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Location))
        {
            var location = queryDto.Location;
            exp = exp.And(x => x.Location != null && x.Location.Contains(location));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingLink))
        {
            var meetingLink = queryDto.MeetingLink;
            exp = exp.And(x => x.MeetingLink != null && x.MeetingLink.Contains(meetingLink));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Agenda))
        {
            var agenda = queryDto.Agenda;
            exp = exp.And(x => x.Agenda != null && x.Agenda.Contains(agenda));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConferenceContent))
        {
            var conferenceContent = queryDto.ConferenceContent;
            exp = exp.And(x => x.ConferenceContent != null && x.ConferenceContent.Contains(conferenceContent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConferenceSummary))
        {
            var conferenceSummary = queryDto.ConferenceSummary;
            exp = exp.And(x => x.ConferenceSummary != null && x.ConferenceSummary.Contains(conferenceSummary));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConferenceTags))
        {
            var conferenceTags = queryDto.ConferenceTags;
            exp = exp.And(x => x.ConferenceTags != null && x.ConferenceTags.Contains(conferenceTags));
        }

        if (queryDto?.OrganizerId.HasValue == true)
        {
            var organizerId = queryDto.OrganizerId.Value;
            exp = exp.And(x => x.OrganizerId == organizerId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrganizerName))
        {
            var organizerName = queryDto.OrganizerName;
            exp = exp.And(x => x.OrganizerName != null && x.OrganizerName.Contains(organizerName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.MaxParticipants.HasValue == true)
        {
            var maxParticipants = queryDto.MaxParticipants.Value;
            exp = exp.And(x => x.MaxParticipants == maxParticipants);
        }

        if (queryDto?.ReminderMinutes.HasValue == true)
        {
            var reminderMinutes = queryDto.ReminderMinutes.Value;
            exp = exp.And(x => x.ReminderMinutes == reminderMinutes);
        }

        if (queryDto?.ConferenceRoomId.HasValue == true)
        {
            var conferenceRoomId = queryDto.ConferenceRoomId.Value;
            exp = exp.And(x => x.ConferenceRoomId == conferenceRoomId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConferenceRoomName))
        {
            var conferenceRoomName = queryDto.ConferenceRoomName;
            exp = exp.And(x => x.ConferenceRoomName != null && x.ConferenceRoomName.Contains(conferenceRoomName));
        }

        if (queryDto?.ConferenceStatus.HasValue == true)
        {
            var conferenceStatus = queryDto.ConferenceStatus.Value;
            exp = exp.And(x => x.ConferenceStatus == conferenceStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            var startTimeStart = queryDto.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime >= startTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            var startTimeEnd = queryDto.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime <= startTimeEnd);
        }

        if (queryDto?.EndTimeStart.HasValue == true)
        {
            var endTimeStart = queryDto.EndTimeStart.Value;
            exp = exp.And(x => x.EndTime >= endTimeStart);
        }

        if (queryDto?.EndTimeEnd.HasValue == true)
        {
            var endTimeEnd = queryDto.EndTimeEnd.Value;
            exp = exp.And(x => x.EndTime <= endTimeEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktConferenceQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConferenceTitle))
        {
            return true;
        }
        if (queryDto.ConferenceType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Location))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingLink))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Agenda))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConferenceContent))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConferenceSummary))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConferenceTags))
        {
            return true;
        }
        if (queryDto.OrganizerId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrganizerName))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.MaxParticipants.HasValue)
        {
            return true;
        }
        if (queryDto.ReminderMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.ConferenceRoomId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConferenceRoomName))
        {
            return true;
        }
        if (queryDto.ConferenceStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.StartTimeStart.HasValue || queryDto.StartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndTimeStart.HasValue || queryDto.EndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
