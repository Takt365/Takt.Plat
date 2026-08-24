// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.ConferenceCenter
// 文件名称：TaktConferenceParticipantService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：会议参与人应用服务实现
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
/// 会议参与人应用服务
/// </summary>
public class TaktConferenceParticipantService : TaktServiceBase, ITaktConferenceParticipantService
{
    private readonly ITaktCompanyRepository<TaktConferenceParticipant> _conferenceParticipantRepository;
    private readonly ITaktApprovalRepository<TaktConference> _conferenceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceParticipantRepository">会议参与人仓储</param>
    /// <param name="conferenceRepository">会议中心仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConferenceParticipantService(
        ITaktCompanyRepository<TaktConferenceParticipant> conferenceParticipantRepository,
        ITaktApprovalRepository<TaktConference> conferenceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _conferenceParticipantRepository = conferenceParticipantRepository;
        _conferenceRepository = conferenceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会议参与人列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConferenceParticipantDto>> GetConferenceParticipantListAsync(TaktConferenceParticipantQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktConferenceParticipantDto>.Create(
                new List<TaktConferenceParticipantDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _conferenceParticipantRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConferenceParticipantDto>.Create(
            data.Adapt<List<TaktConferenceParticipantDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会议参与人
    /// </summary>
    /// <param name="id">会议参与人ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceParticipantDto?> GetConferenceParticipantByIdAsync(long id)
    {
        var entity = await _conferenceParticipantRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktConferenceParticipantDto>();
    }

    /// <summary>
    /// 获取会议参与人选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetConferenceParticipantOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _conferenceParticipantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AttendanceStatus == 1,
            x => x.UserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.UserName,
            DictLabel = e.UserName,
        }).ToList();
    }

    /// <summary>
    /// 创建会议参与人
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceParticipantDto> CreateConferenceParticipantAsync(TaktConferenceParticipantCreateDto dto)
    {
        var entity = dto.Adapt<TaktConferenceParticipant>();
        await StampConferenceParticipantConferenceAsync(entity, dto);
        var isUnique_ix_conference_participant_unique = await _uniqueValidator.IsUniqueAsync(
            _conferenceParticipantRepository,
            x => x.ConferenceId == entity.ConferenceId
                && x.UserId == entity.UserId);
        if (!isUnique_ix_conference_participant_unique)
        {
            throw new TaktBusinessException("会议参与人的ConferenceId、UserId已存在");
        }
        entity = await _conferenceParticipantRepository.CreateAsync(entity);
        return await GetConferenceParticipantByIdAsync(entity.Id) ?? entity.Adapt<TaktConferenceParticipantDto>();
    }

    /// <summary>
    /// 更新会议参与人
    /// </summary>
    /// <param name="id">会议参与人ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceParticipantDto> UpdateConferenceParticipantAsync(long id, TaktConferenceParticipantUpdateDto dto)
    {
        var entity = await _conferenceParticipantRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议参与人不存在");
        }
        dto.Adapt(entity);
        await StampConferenceParticipantConferenceAsync(entity, dto);
        var isUnique_ix_conference_participant_unique = await _uniqueValidator.IsUniqueAsync(
            _conferenceParticipantRepository,
            x => x.ConferenceId == entity.ConferenceId
                && x.UserId == entity.UserId,
            id);
        if (!isUnique_ix_conference_participant_unique)
        {
            throw new TaktBusinessException("会议参与人的ConferenceId、UserId已存在");
        }
        await _conferenceParticipantRepository.UpdateAsync(entity);
        return await GetConferenceParticipantByIdAsync(id) ?? throw new TaktBusinessException("会议参与人不存在");
    }

    /// <summary>
    /// 删除会议参与人
    /// </summary>
    /// <param name="id">会议参与人ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConferenceParticipantByIdAsync(long id)
    {
        var deleted = await _conferenceParticipantRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会议参与人不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会议参与人
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConferenceParticipantBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteConferenceParticipantByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会议参与人状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConferenceParticipantDto> UpdateConferenceParticipantStatusAsync(TaktConferenceParticipantStatusDto dto)
    {
        var entity = await _conferenceParticipantRepository.GetByIdAsync(dto.ConferenceParticipantId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议参与人不存在");
        }
        entity.AttendanceStatus = dto.AttendanceStatus;
        await _conferenceParticipantRepository.UpdateAsync(entity);
        return await GetConferenceParticipantByIdAsync(dto.ConferenceParticipantId) ?? throw new TaktBusinessException("会议参与人不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConferenceParticipantTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConferenceParticipantTemplateDto>(
            sheetName ?? "会议参与人导入模板",
            fileName ?? "会议参与人导入模板.xlsx");
    }

    /// <summary>
    /// 导入会议参与人
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConferenceParticipantAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConferenceParticipantImportDto>(fileStream, sheetName ?? "会议参与人导入模板");
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
                var entity = rows[i].Adapt<TaktConferenceParticipant>();
                var importDto = rows[i].Adapt<TaktConferenceParticipantCreateDto>();
                await StampConferenceParticipantConferenceAsync(entity, importDto);
                var importKey = $"{entity.ConferenceId}|{entity.UserId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ConferenceId、UserId）");
                }
                var isUnique_ix_conference_participant_unique = await _uniqueValidator.IsUniqueAsync(
                    _conferenceParticipantRepository,
                    x => x.ConferenceId == entity.ConferenceId
                        && x.UserId == entity.UserId);
                if (!isUnique_ix_conference_participant_unique)
                {
                    throw new TaktBusinessException("会议参与人的ConferenceId、UserId已存在");
                }
                await _conferenceParticipantRepository.CreateAsync(entity);
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
    /// 导出会议参与人
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConferenceParticipantAsync(TaktConferenceParticipantQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktConferenceParticipantQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConferenceParticipantExportDto>(),
                sheetName ?? "会议参与人数据",
                fileName ?? "会议参与人导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _conferenceParticipantRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConferenceParticipantExportDto>(),
                sheetName ?? "会议参与人数据",
                fileName ?? "会议参与人导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConferenceParticipantExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会议参与人数据",
            fileName ?? "会议参与人导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步会议参与人主表外键（ManyToOne → 会议中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampConferenceParticipantConferenceAsync(TaktConferenceParticipant entity, TaktConferenceParticipantCreateDto dto)
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
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会议参与人查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktConferenceParticipant, bool>> QueryExpression(TaktConferenceParticipantQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConferenceParticipant>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
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

        if (queryDto?.ConferenceId.HasValue == true)
        {
            var conferenceId = queryDto.ConferenceId.Value;
            exp = exp.And(x => x.ConferenceId == conferenceId);
        }

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserName))
        {
            var UserName = queryDto.UserName;
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(UserName));
        }

        if (queryDto?.ParticipantRole.HasValue == true)
        {
            var participantRole = queryDto.ParticipantRole.Value;
            exp = exp.And(x => x.ParticipantRole == participantRole);
        }

        if (queryDto?.CheckInMethod.HasValue == true)
        {
            var checkInMethod = queryDto.CheckInMethod.Value;
            exp = exp.And(x => x.CheckInMethod == checkInMethod);
        }

        if (queryDto?.AttendanceStatus.HasValue == true)
        {
            var attendanceStatus = queryDto.AttendanceStatus.Value;
            exp = exp.And(x => x.AttendanceStatus == attendanceStatus);
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

        if (queryDto?.CheckInTimeStart.HasValue == true)
        {
            var checkInTimeStart = queryDto.CheckInTimeStart.Value;
            exp = exp.And(x => x.CheckInTime >= checkInTimeStart);
        }

        if (queryDto?.CheckInTimeEnd.HasValue == true)
        {
            var checkInTimeEnd = queryDto.CheckInTimeEnd.Value;
            exp = exp.And(x => x.CheckInTime <= checkInTimeEnd);
        }

        if (queryDto?.CheckOutTimeStart.HasValue == true)
        {
            var checkOutTimeStart = queryDto.CheckOutTimeStart.Value;
            exp = exp.And(x => x.CheckOutTime >= checkOutTimeStart);
        }

        if (queryDto?.CheckOutTimeEnd.HasValue == true)
        {
            var checkOutTimeEnd = queryDto.CheckOutTimeEnd.Value;
            exp = exp.And(x => x.CheckOutTime <= checkOutTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktConferenceParticipantQueryDto? queryDto)
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
        if (queryDto.ConferenceId.HasValue)
        {
            return true;
        }
        if (queryDto.UserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserName))
        {
            return true;
        }
        if (queryDto.ParticipantRole.HasValue)
        {
            return true;
        }
        if (queryDto.CheckInMethod.HasValue)
        {
            return true;
        }
        if (queryDto.AttendanceStatus.HasValue)
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
        if (queryDto.CheckInTimeStart.HasValue || queryDto.CheckInTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CheckOutTimeStart.HasValue || queryDto.CheckOutTimeEnd.HasValue)
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
