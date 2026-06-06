// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：工单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 工单应用服务
/// </summary>
public class TaktTicketService : TaktServiceBase, ITaktTicketService
{
    private readonly ITaktCompanyRepository<TaktTicket> _ticketRepository;
    private readonly ITaktCompanyRepository<TaktTicketChangeLog> _ticketChangeLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketRepository">工单仓储</param>
    /// <param name="ticketChangeLogRepository">TicketChangeLog仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketService(
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktCompanyRepository<TaktTicketChangeLog> ticketChangeLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketRepository = ticketRepository;
        _ticketChangeLogRepository = ticketChangeLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketDto>> GetTicketListAsync(TaktTicketQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ticketRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTicketDto>.Create(
            data.Adapt<List<TaktTicketDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto?> GetTicketByIdAsync(long id)
    {
        var entity = await _ticketRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTicketDto>();
        await FillTicketDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ticketRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SubmitterName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SubmitterName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> CreateTicketAsync(TaktTicketCreateDto dto)
    {
        var entity = dto.Adapt<TaktTicket>();
        var isUnique_ix_ticket_no_unique = await _uniqueValidator.IsUniqueAsync(
            _ticketRepository,
            x => x.TicketNo == entity.TicketNo);
        if (!isUnique_ix_ticket_no_unique)
        {
            throw new TaktBusinessException("工单的TicketNo已存在");
        }
        entity = await _ticketRepository.CreateAsync(entity);
                await SaveTicketChildrenAsync(entity, dto);
        return await GetTicketByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketDto>();
    }

    /// <summary>
    /// 更新工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> UpdateTicketAsync(long id, TaktTicketUpdateDto dto)
    {
        var entity = await _ticketRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_ticket_no_unique = await _uniqueValidator.IsUniqueAsync(
            _ticketRepository,
            x => x.TicketNo == entity.TicketNo,
            id);
        if (!isUnique_ix_ticket_no_unique)
        {
            throw new TaktBusinessException("工单的TicketNo已存在");
        }
        await _ticketRepository.UpdateAsync(entity);
                await SaveTicketChildrenAsync(entity, dto);
        return await GetTicketByIdAsync(id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 删除工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketByIdAsync(long id)
    {
        var entity = await _ticketRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单不存在或已删除");
        }
        await _ticketChangeLogRepository.DeleteAsync(x => x.TicketId == entity.Id);
        var deleted = await _ticketRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTicketByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> UpdateTicketStatusAsync(TaktTicketStatusDto dto)
    {
        var entity = await _ticketRepository.GetByIdAsync(dto.TicketId);
        if (entity == null)
        {
            throw new TaktBusinessException("工单不存在");
        }
        entity.TicketStatus = dto.TicketStatus;
        await _ticketRepository.UpdateAsync(entity);
        return await GetTicketByIdAsync(dto.TicketId) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTicketTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTicketTemplateDto>(
            sheetName ?? "工单导入模板",
            fileName ?? "工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTicketAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTicketImportDto>(fileStream, sheetName ?? "工单导入模板");
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
                var entity = rows[i].Adapt<TaktTicket>();
                var importKey = $"{entity.TicketNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TicketNo）");
                }
                var isUnique_ix_ticket_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _ticketRepository,
                    x => x.TicketNo == entity.TicketNo);
                if (!isUnique_ix_ticket_no_unique)
                {
                    throw new TaktBusinessException("工单的TicketNo已存在");
                }
                await _ticketRepository.CreateAsync(entity);
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
    /// 导出工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTicketAsync(TaktTicketQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTicketQueryDto());
        var list = await _ticketRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketExportDto>(),
                sheetName ?? "工单数据",
                fileName ?? "工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTicketExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工单数据",
            fileName ?? "工单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工单详情（加载 OneToMany 子表：工单变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTicketDetailsAsync(TaktTicketDto dto, TaktTicket entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工单变更日志 → dto.ChangeLogs
        var changelogs = await _ticketChangeLogRepository.GetListAsync(x => x.TicketId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktTicketChangeLogDto>>();
    }

    /// <summary>
    /// 保存工单子表级联（工单变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTicketChildrenAsync(TaktTicket entity, TaktTicketCreateDto dto)
    {
        // 工单变更日志（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _ticketChangeLogRepository.DeleteAsync(x => x.TicketId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktTicketChangeLog>>();
            foreach (var child in changelogs)
            {
                child.TicketId = entity.Id;
            }
            await _ticketChangeLogRepository.DeleteAsync(x => x.TicketId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _ticketChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicket, bool>> QueryExpression(TaktTicketQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicket>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TicketNo != null && x.TicketNo.Contains(keywords))
                || (x.Title != null && x.Title.Contains(keywords))
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.AttachmentsJson != null && x.AttachmentsJson.Contains(keywords))
                || SqlFunc.ToString(x.TicketStatus).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || (x.CategoryCode != null && x.CategoryCode.Contains(keywords))
                || SqlFunc.ToString(x.TicketSource).Contains(keywords)
                || SqlFunc.ToString(x.SubmitterId).Contains(keywords)
                || (x.SubmitterName != null && x.SubmitterName.Contains(keywords))
                || SqlFunc.ToString(x.AssigneeId).Contains(keywords)
                || (x.AssigneeName != null && x.AssigneeName.Contains(keywords))
                || SqlFunc.ToString(x.KnowledgeId).Contains(keywords)
                || SqlFunc.ToString(x.ParentTicketId).Contains(keywords)
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || SqlFunc.ToString(x.ApplicantDeptId).Contains(keywords)
                || (x.ApplicantDeptName != null && x.ApplicantDeptName.Contains(keywords))
                || SqlFunc.ToString(x.ApplicantBy).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.FirstResponseAt).Contains(keywords)
                || SqlFunc.ToString(x.FirstResponseDueBy).Contains(keywords)
                || SqlFunc.ToString(x.ResolvedAt).Contains(keywords)
                || SqlFunc.ToString(x.ResolutionDueBy).Contains(keywords)
                || SqlFunc.ToString(x.ClosedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.TicketNo))
        {
            exp = exp.And(x => x.TicketNo != null && x.TicketNo.Contains(queryDto.TicketNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.Title))
        {
            exp = exp.And(x => x.Title != null && x.Title.Contains(queryDto.Title));
        }

        if (!string.IsNullOrEmpty(queryDto?.Content))
        {
            exp = exp.And(x => x.Content != null && x.Content.Contains(queryDto.Content));
        }

        if (!string.IsNullOrEmpty(queryDto?.AttachmentsJson))
        {
            exp = exp.And(x => x.AttachmentsJson != null && x.AttachmentsJson.Contains(queryDto.AttachmentsJson));
        }

        if (queryDto?.TicketStatus.HasValue == true)
        {
            exp = exp.And(x => x.TicketStatus == queryDto.TicketStatus);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (!string.IsNullOrEmpty(queryDto?.CategoryCode))
        {
            exp = exp.And(x => x.CategoryCode != null && x.CategoryCode.Contains(queryDto.CategoryCode));
        }

        if (queryDto?.TicketSource.HasValue == true)
        {
            exp = exp.And(x => x.TicketSource == queryDto.TicketSource);
        }

        if (queryDto?.SubmitterId.HasValue == true)
        {
            exp = exp.And(x => x.SubmitterId == queryDto.SubmitterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SubmitterName))
        {
            exp = exp.And(x => x.SubmitterName != null && x.SubmitterName.Contains(queryDto.SubmitterName));
        }

        if (queryDto?.AssigneeId.HasValue == true)
        {
            exp = exp.And(x => x.AssigneeId == queryDto.AssigneeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssigneeName))
        {
            exp = exp.And(x => x.AssigneeName != null && x.AssigneeName.Contains(queryDto.AssigneeName));
        }

        if (queryDto?.KnowledgeId.HasValue == true)
        {
            exp = exp.And(x => x.KnowledgeId == queryDto.KnowledgeId);
        }

        if (queryDto?.ParentTicketId.HasValue == true)
        {
            exp = exp.And(x => x.ParentTicketId == queryDto.ParentTicketId);
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (queryDto?.ApplicantDeptId.HasValue == true)
        {
            exp = exp.And(x => x.ApplicantDeptId == queryDto.ApplicantDeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ApplicantDeptName))
        {
            exp = exp.And(x => x.ApplicantDeptName != null && x.ApplicantDeptName.Contains(queryDto.ApplicantDeptName));
        }

        if (queryDto?.ApplicantBy.HasValue == true)
        {
            exp = exp.And(x => x.ApplicantBy == queryDto.ApplicantBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.FirstResponseAtStart.HasValue == true)
        {
            exp = exp.And(x => x.FirstResponseAt >= queryDto.FirstResponseAtStart);
        }

        if (queryDto?.FirstResponseAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.FirstResponseAt <= queryDto.FirstResponseAtEnd);
        }

        if (queryDto?.FirstResponseDueByStart.HasValue == true)
        {
            exp = exp.And(x => x.FirstResponseDueBy >= queryDto.FirstResponseDueByStart);
        }

        if (queryDto?.FirstResponseDueByEnd.HasValue == true)
        {
            exp = exp.And(x => x.FirstResponseDueBy <= queryDto.FirstResponseDueByEnd);
        }

        if (queryDto?.ResolvedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ResolvedAt >= queryDto.ResolvedAtStart);
        }

        if (queryDto?.ResolvedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ResolvedAt <= queryDto.ResolvedAtEnd);
        }

        if (queryDto?.ResolutionDueByStart.HasValue == true)
        {
            exp = exp.And(x => x.ResolutionDueBy >= queryDto.ResolutionDueByStart);
        }

        if (queryDto?.ResolutionDueByEnd.HasValue == true)
        {
            exp = exp.And(x => x.ResolutionDueBy <= queryDto.ResolutionDueByEnd);
        }

        if (queryDto?.ClosedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ClosedAt >= queryDto.ClosedAtStart);
        }

        if (queryDto?.ClosedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ClosedAt <= queryDto.ClosedAtEnd);
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
