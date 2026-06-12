// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：工单应用服务实现（含 ITSM 状态机、自动指派、回复会话）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Entities.Accounting.Financial;
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
    private readonly ITaktCompanyRepository<TaktTicketReply> _ticketReplyRepository;
    private readonly ITaktCompanyRepository<TaktTicketCategoryAssign> _categoryAssignRepository;
    private readonly ITaktCompanyRepository<TaktAsset> _assetRepository;
    private readonly ITaktCompanyRepository<TaktItAsset> _itAssetRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingService? _numberingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktTicketService(
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktCompanyRepository<TaktTicketChangeLog> ticketChangeLogRepository,
        ITaktCompanyRepository<TaktTicketReply> ticketReplyRepository,
        ITaktCompanyRepository<TaktTicketCategoryAssign> categoryAssignRepository,
        ITaktCompanyRepository<TaktAsset> assetRepository,
        ITaktCompanyRepository<TaktItAsset> itAssetRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingService? numberingService = null,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketRepository = ticketRepository;
        _ticketChangeLogRepository = ticketChangeLogRepository;
        _ticketReplyRepository = ticketReplyRepository;
        _categoryAssignRepository = categoryAssignRepository;
        _assetRepository = assetRepository;
        _itAssetRepository = itAssetRepository;
        _uniqueValidator = uniqueValidator;
        _numberingService = numberingService;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktTicketMyAssetDto>> GetMyAssetListAsync(TaktTicketMyAssetQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        if (!CurrentUserId.HasValue || CurrentUserId.Value <= 0)
        {
            ThrowBusinessException("无法确定当前用户");
        }
        queryDto.PageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        queryDto.PageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var userId = CurrentUserId.Value;
        var tickets = await _ticketRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.SubmitterId == userId
                && x.AssetCode != null
                && x.AssetCode != "",
            x => x.CreatedAt,
            true);
        var grouped = tickets
            .GroupBy(x => x.AssetCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => new TaktTicketMyAssetDto
            {
                AssetCode = g.Key,
                TicketCount = g.Count(),
                LastTicketAt = g.Max(t => t.CreatedAt),
            })
            .OrderByDescending(x => x.LastTicketAt)
            .ToList();
        if (grouped.Count > 0)
        {
            var codes = grouped.Select(x => x.AssetCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var assets = await _assetRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && codes.Contains(x.AssetCode));
            var nameMap = assets.ToDictionary(a => a.AssetCode, a => a.AssetName, StringComparer.OrdinalIgnoreCase);
            foreach (var item in grouped)
            {
                if (nameMap.TryGetValue(item.AssetCode, out var name))
                {
                    item.AssetName = name;
                }
            }
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords.Trim();
            grouped = grouped.Where(x =>
                x.AssetCode.Contains(keywords, StringComparison.OrdinalIgnoreCase)
                || (x.AssetName != null && x.AssetName.Contains(keywords, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
        var total = grouped.Count;
        var skip = TaktPagedClamp.ComputeSkip(queryDto.PageIndex, queryDto.PageSize);
        var page = grouped.Skip(skip).Take(queryDto.PageSize).ToList();
        return TaktPagedResult<TaktTicketMyAssetDto>.Create(page, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto?> GetTicketByIdAsync(long id)
    {
        var entity = await GetTicketEntityAsync(id);
        if (entity == null)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTicketDto>();
        await FillTicketDetailsAsync(dto, entity);
        return dto;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetTicketOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ticketRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TicketNo,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TicketNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> CreateTicketAsync(TaktTicketCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktTicket>();
        await ApplyItAssetLinkAsync(entity, dto.ItAssetId, dto.AssetCode);
        entity.TicketStatus = (int)0;
        entity.ResolvedAt = null;
        entity.ClosedAt = null;
        if (string.IsNullOrWhiteSpace(entity.TicketNo))
        {
            entity.TicketNo = await GenerateTicketNoAsync();
        }
        await EnsureTicketNoUniqueAsync(entity.TicketNo);
        entity = await _ticketRepository.CreateAsync(entity);
        await TryAutoAssignAsync(entity);
        await AppendChangeLogAsync(
            entity,
            0,
            "工单创建",
            dto.Remark);
        if (dto.ChangeLogs is { Count: > 0 })
        {
            await SaveTicketChildrenAsync(entity, dto);
        }
        return await GetTicketByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketDto>();
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> SubmitTicketAsync(TaktTicketSubmitDto dto)
    {
        EnsureThreeLayerContext();
        if (!CurrentUserId.HasValue || CurrentUserId.Value <= 0)
        {
            ThrowBusinessException("无法确定当前用户");
        }
        var createDto = dto.Adapt<TaktTicketCreateDto>();
        createDto.TenantCode = CurrentTenantCode;
        createDto.CompanyCode = CurrentCompanyCode;
        createDto.TicketNo = await GenerateTicketNoAsync();
        createDto.TicketSource = (int)0;
        createDto.SubmitterId = CurrentUserId.Value;
        createDto.SubmitterName = CurrentUserName;
        createDto.ApplicantBy = CurrentUserId.Value;
        createDto.Priority = (int)dto.Priority;
        return await CreateTicketAsync(createDto);
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> CreateTicketFromChannelAsync(TaktTicketCreateFromChannelDto dto)
    {
        EnsureThreeLayerContext();
        var createDto = dto.Adapt<TaktTicketCreateDto>();
        createDto.TenantCode = CurrentTenantCode;
        createDto.CompanyCode = CurrentCompanyCode;
        createDto.TicketNo = await GenerateTicketNoAsync();
        createDto.TicketSource = (int)dto.TicketSource;
        createDto.Priority = (int)dto.Priority;
        if (dto.SubmitterId.HasValue && dto.SubmitterId.Value > 0)
        {
            createDto.SubmitterId = dto.SubmitterId.Value;
            createDto.SubmitterName = dto.SubmitterName;
            createDto.ApplicantBy = dto.SubmitterId.Value;
        }
        else if (CurrentUserId.HasValue && CurrentUserId.Value > 0)
        {
            createDto.SubmitterId = CurrentUserId.Value;
            createDto.SubmitterName = CurrentUserName;
            createDto.ApplicantBy = CurrentUserId.Value;
        }
        else
        {
            ThrowBusinessException("渠道建单须指定提交人或登录用户");
        }
        if (!string.IsNullOrWhiteSpace(dto.ExternalMessageId))
        {
            createDto.Remark = string.IsNullOrWhiteSpace(createDto.Remark)
                ? $"ExternalId={dto.ExternalMessageId}"
                : $"{createDto.Remark}; ExternalId={dto.ExternalMessageId}";
        }
        return await CreateTicketAsync(createDto);
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> UpdateTicketAsync(long id, TaktTicketUpdateDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(id);
        EnsureTicketEditable(entity);
        var previousStatus = entity.TicketStatus;
        dto.Adapt(entity);
        entity.TicketStatus = previousStatus;
        entity.AssigneeId = dto.AssigneeId ?? entity.AssigneeId;
        await ApplyItAssetLinkAsync(entity, dto.ItAssetId, dto.AssetCode);
        await EnsureTicketNoUniqueAsync(entity.TicketNo, id);
        await _ticketRepository.UpdateAsync(entity);
        await SaveTicketChildrenAsync(entity, dto);
        return await GetTicketByIdAsync(id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task DeleteTicketByIdAsync(long id)
    {
        var entity = await GetTicketEntityOrThrowAsync(id);
        await _ticketReplyRepository.DeleteAsync(x => x.TicketId == entity.Id);
        await _ticketChangeLogRepository.DeleteAsync(x => x.TicketId == entity.Id);
        var deleted = await _ticketRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单不存在或已删除");
        }
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<TaktTicketDto> UpdateTicketStatusAsync(TaktTicketStatusDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(id: dto.TicketId);
        var current = GetNormalizedStatus(entity);
        var target = TaktTicketWorkflowHelper.NormalizeLegacyStatus(dto.TicketStatus);
        await TransitionTicketStatusAsync(entity, current, target, dto.TicketStatus.ToString(), null);
        return await GetTicketByIdAsync(dto.TicketId) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> AssignTicketAsync(TaktTicketAssignDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanPickOrAssign(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        var assigneeId = dto.AssigneeId ?? CurrentUserId;
        if (!assigneeId.HasValue || assigneeId.Value <= 0)
        {
            ThrowBusinessException("须指定处理人或登录后领取");
        }
        entity.AssigneeId = assigneeId;
        entity.AssigneeName = dto.AssigneeName ?? CurrentUserName;
        var target = dto.StartImmediately
            ? 2
            : 1;
        await TransitionTicketStatusAsync(entity, current, target, "指派/领取工单", dto.Remark, 5);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> StartTicketProgressAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        EnsureAssigneeOrAgent(entity);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanStartProgress(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        await TransitionTicketStatusAsync(entity, current, 2, "开始处理", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> WaitForRequesterAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        EnsureAssigneeOrAgent(entity);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanWaitForRequester(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        await TransitionTicketStatusAsync(entity, current, 3, "等待用户回复", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> ResolveTicketAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        EnsureAssigneeOrAgent(entity);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanResolve(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        entity.ResolvedAt = DateTime.Now;
        await TransitionTicketStatusAsync(entity, current, 4, "标记已解决", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> ConfirmCloseTicketAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        EnsureSubmitter(entity);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanConfirmClose(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        entity.ClosedAt = DateTime.Now;
        await TransitionTicketStatusAsync(entity, current, 5, "用户确认关闭", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketDto> ReopenTicketAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanReopen(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        if (current == 4)
        {
            EnsureSubmitter(entity);
        }
        entity.ResolvedAt = null;
        entity.ClosedAt = null;
        await TransitionTicketStatusAsync(entity, current, 6, "重新打开", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <inheritdoc />
    public async Task<TaktTicketReplyDto> ReplyTicketAsync(TaktTicketReplyCreateDto dto)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(dto.Content))
        {
            ThrowBusinessException("回复内容不能为空");
        }
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        var status = GetNormalizedStatus(entity);
        if (status == 5)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketClosedReadonly);
        }
        var isSubmitter = CurrentUserId.HasValue && entity.SubmitterId == CurrentUserId.Value;
        var isAssignee = entity.AssigneeId.HasValue && CurrentUserId.HasValue && entity.AssigneeId == CurrentUserId.Value;
        if (dto.IsInternal)
        {
            if (!isAssignee)
            {
                ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketAssigneeOnly);
            }
        }
        else if (!isSubmitter && !isAssignee)
        {
            ThrowBusinessException("仅提交人或处理人可回复");
        }
        var authorType = isSubmitter && !isAssignee ? 1 : 0;
        if (dto.IsInternal)
        {
            authorType = 0;
        }
        var reply = new TaktTicketReply
        {
            TicketId = entity.Id,
            AuthorType = authorType,
            AuthorId = CurrentUserId ?? 0,
            AuthorName = CurrentUserName,
            Content = dto.Content.Trim(),
            AttachmentsJson = dto.AttachmentsJson,
            IsInternal = dto.IsInternal ? 1 : 0,
        };
        reply = await _ticketReplyRepository.CreateAsync(reply);
        if (authorType == 0 && !entity.FirstResponseAt.HasValue)
        {
            entity.FirstResponseAt = DateTime.Now;
            await _ticketRepository.UpdateAsync(entity);
        }
        if (TaktTicketWorkflowHelper.ShouldResumeAfterReply(status, authorType))
        {
            await TransitionTicketStatusAsync(
                entity,
                status,
                2,
                "用户回复，继续处理",
                null);
        }
        await AppendChangeLogAsync(entity, 4, "工单回复", dto.Content);
        var replyDto = reply.Adapt<TaktTicketReplyDto>();
        replyDto.IsInternal = reply.IsInternal == 1;
        return replyDto;
    }

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktTicketReplyDto>> GetTicketReplyListAsync(TaktTicketReplyQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktTicketReply, bool>> predicate = x =>
            x.TicketId == queryDto.TicketId
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode;
        if (!queryDto.IncludeInternal)
        {
            predicate = x =>
                x.TicketId == queryDto.TicketId
                && x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.IsInternal == 0;
        }
        var (data, total) = await _ticketReplyRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.CreatedAt,
            false);
        return TaktPagedResult<TaktTicketReplyDto>.Create(
            data.Adapt<List<TaktTicketReplyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] content)> GetTicketTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTicketTemplateDto>(
            sheetName ?? "工单导入模板",
            fileName ?? "工单导入模板.xlsx");
    }

    /// <inheritdoc />
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
                entity.TicketStatus = TaktTicketWorkflowHelper.MapLegacyImportStatus(entity.TicketStatus);
                var importKey = $"{entity.TicketNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TicketNo）");
                }
                await EnsureTicketNoUniqueAsync(entity.TicketNo);
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

    /// <inheritdoc />
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
    // 工作流私有方法
    // ========================================

    /// <summary>
    /// 获取工单实体（租户/公司隔离）
    /// </summary>
    private async Task<TaktTicket?> GetTicketEntityAsync(long id)
    {
        var entity = await _ticketRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity;
    }

    /// <summary>
    /// 获取工单或抛错
    /// </summary>
    private async Task<TaktTicket> GetTicketEntityOrThrowAsync(long id)
    {
        var entity = await GetTicketEntityAsync(id);
        if (entity == null)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.NotFound, "entity.ticket._self");
        }
        return entity!;
    }

    /// <summary>
    /// 读取并规范化状态
    /// </summary>
    private static int GetNormalizedStatus(TaktTicket entity)
    {
        return TaktTicketWorkflowHelper.NormalizeLegacyStatus(entity.TicketStatus);
    }

    /// <summary>
    /// 执行状态流转并写库
    /// </summary>
    private async Task TransitionTicketStatusAsync(
        TaktTicket entity,
        int current,
        int target,
        string summary,
        string? reason,
        int changeType = 3)
    {
        if (!TaktTicketWorkflowHelper.CanTransition(current, target))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        entity.TicketStatus = target;
        await _ticketRepository.UpdateAsync(entity);
        await AppendChangeLogAsync(
            entity,
            changeType,
            $"{summary}：{current} → {target}",
            reason,
            $"{{\"from\":{current},\"to\":{target}}}");
    }

    /// <summary>
    /// 追加变更日志
    /// </summary>
    private async Task AppendChangeLogAsync(
        TaktTicket entity,
        int changeType,
        string summary,
        string? reason,
        string? changeFields = null)
    {
        var log = new TaktTicketChangeLog
        {
            TicketId = entity.Id,
            TicketNo = entity.TicketNo,
            ChangeType = changeType,
            ChangeSummary = summary,
            ChangeReason = reason,
            ChangeFields = changeFields,
        };
        await _ticketChangeLogRepository.CreateAsync(log);
    }

    /// <summary>
    /// 按分类默认处理人自动指派
    /// </summary>
    private async Task TryAutoAssignAsync(TaktTicket entity)
    {
        if (string.IsNullOrWhiteSpace(entity.CategoryCode))
        {
            return;
        }
        var assigns = await _categoryAssignRepository.GetListAsync(x =>
            x.CategoryCode == entity.CategoryCode
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode);
        var assign = assigns.OrderBy(x => x.SortOrder).FirstOrDefault();
        if (assign == null)
        {
            return;
        }
        entity.AssigneeId = assign.AssigneeId;
        entity.AssigneeName = assign.AssigneeName;
        entity.TicketStatus = (int)1;
        await _ticketRepository.UpdateAsync(entity);
        await AppendChangeLogAsync(
            entity,
            5,
            $"系统自动指派：{assign.AssigneeName}",
            entity.CategoryCode);
    }

    /// <summary>
    /// 生成工单编号
    /// </summary>
    private async Task<string> GenerateTicketNoAsync()
    {
        if (_numberingService != null)
        {
            try
            {
                var result = await _numberingService.GenerateNumberingAsync(new TaktNumberingGenerateRequestDto
                {
                    RuleCode = TaktTicketWorkflowHelper.TicketNumberRuleCode,
                });
                if (!string.IsNullOrWhiteSpace(result.BusinessCode))
                {
                    return result.BusinessCode;
                }
            }
            catch (Exception ex)
            {
                LogWarning($"编号规则 {TaktTicketWorkflowHelper.TicketNumberRuleCode} 不可用: {ex.Message}");
            }
        }
        return $"TK{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

    /// <summary>
    /// 校验工单编号唯一
    /// </summary>
    private async Task EnsureTicketNoUniqueAsync(string ticketNo, long? excludeId = null)
    {
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _ticketRepository,
            x => x.TicketNo == ticketNo,
            excludeId);
        if (!isUnique)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.Duplicate, "entity.ticket.no");
        }
    }

    /// <summary>
    /// 已关闭工单不可编辑
    /// </summary>
    private void EnsureTicketEditable(TaktTicket entity)
    {
        if (GetNormalizedStatus(entity) == 5)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketClosedReadonly);
        }
    }

    /// <summary>
    /// 校验当前用户为提交人
    /// </summary>
    private void EnsureSubmitter(TaktTicket entity)
    {
        if (!CurrentUserId.HasValue || entity.SubmitterId != CurrentUserId.Value)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketSubmitterOnly);
        }
    }

    /// <summary>
    /// 校验当前用户为处理人（客服）
    /// </summary>
    private void EnsureAssigneeOrAgent(TaktTicket entity)
    {
        if (!entity.AssigneeId.HasValue || !CurrentUserId.HasValue || entity.AssigneeId != CurrentUserId.Value)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketAssigneeOnly);
        }
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工单详情
    /// </summary>
    private async Task FillTicketDetailsAsync(TaktTicketDto dto, TaktTicket entity)
    {
        if (dto == null)
        {
            return;
        }
        var changelogs = await _ticketChangeLogRepository.GetListAsync(x => x.TicketId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktTicketChangeLogDto>>();
        var replies = await _ticketReplyRepository.GetListAsync(
            x => x.TicketId == entity.Id,
            x => x.CreatedAt,
            false);
        dto.Replies = replies.Adapt<List<TaktTicketReplyDto>>();
        await FillAssetNameAsync(dto);
    }

    /// <summary>
    /// 解析并校验工单与 IT 设备保修扩展、财务资产的关联
    /// </summary>
    /// <param name="entity">工单实体</param>
    /// <param name="itAssetId">IT 设备保修扩展 ID</param>
    /// <param name="assetCode">资产号码</param>
    private async Task ApplyItAssetLinkAsync(TaktTicket entity, long? itAssetId, string? assetCode)
    {
        EnsureThreeLayerContext();
        if (itAssetId.HasValue && itAssetId.Value > 0)
        {
            var itAsset = await _itAssetRepository.GetByIdAsync(itAssetId.Value);
            if (itAsset == null
                || itAsset.TenantCode != CurrentTenantCode
                || itAsset.CompanyCode != CurrentCompanyCode)
            {
                ThrowBusinessException("IT设备保修扩展记录不存在");
            }
            if (!string.IsNullOrWhiteSpace(assetCode)
                && !string.Equals(assetCode.Trim(), itAsset.AssetCode, StringComparison.OrdinalIgnoreCase))
            {
                ThrowBusinessException("资产号码与IT设备保修扩展不一致");
            }
            await EnsureFinancialAssetExistsAsync(itAsset.AssetCode);
            entity.ItAssetId = itAsset.Id;
            entity.AssetCode = itAsset.AssetCode;
            return;
        }
        if (string.IsNullOrWhiteSpace(assetCode))
        {
            entity.ItAssetId = null;
            entity.AssetCode = null;
            return;
        }
        var code = assetCode.Trim();
        await EnsureFinancialAssetExistsAsync(code);
        entity.AssetCode = code;
        var itAssets = await _itAssetRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.AssetCode == code);
        entity.ItAssetId = itAssets.FirstOrDefault()?.Id;
    }

    /// <summary>
    /// 校验财务固定资产是否存在
    /// </summary>
    /// <param name="assetCode">资产号码</param>
    private async Task EnsureFinancialAssetExistsAsync(string assetCode)
    {
        var assets = await _assetRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.AssetCode == assetCode);
        if (assets.Count == 0)
        {
            ThrowBusinessException($"资产号码不存在：{assetCode}");
        }
    }

    /// <summary>
    /// 填充工单 DTO 的资产名称
    /// </summary>
    /// <param name="dto">工单 DTO</param>
    private async Task FillAssetNameAsync(TaktTicketDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.AssetCode))
        {
            return;
        }
        EnsureThreeLayerContext();
        var code = dto.AssetCode.Trim();
        var assets = await _assetRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.AssetCode == code);
        dto.AssetName = assets.FirstOrDefault()?.AssetName;
    }

    /// <summary>
    /// 保存工单子表级联
    /// </summary>
    private async Task SaveTicketChildrenAsync(TaktTicket entity, TaktTicketCreateDto dto)
    {
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            return;
        }
        var changelogs = dto.ChangeLogs.Adapt<List<TaktTicketChangeLog>>();
        foreach (var child in changelogs)
        {
            child.TicketId = entity.Id;
        }
        await _ticketChangeLogRepository.DeleteAsync(x => x.TicketId == entity.Id);
        await _ticketChangeLogRepository.CreateRangeAsync(changelogs);
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工单查询表达式
    /// </summary>
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
                || (x.SubmitterName != null && x.SubmitterName.Contains(keywords))
                || (x.AssigneeName != null && x.AssigneeName.Contains(keywords))
                || (x.AssetCode != null && x.AssetCode.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.AssetCode))
        {
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(queryDto.AssetCode));
        }
        if (queryDto?.ItAssetId.HasValue == true)
        {
            exp = exp.And(x => x.ItAssetId == queryDto.ItAssetId);
        }
        if (!string.IsNullOrEmpty(queryDto?.TicketNo))
        {
            exp = exp.And(x => x.TicketNo != null && x.TicketNo.Contains(queryDto.TicketNo));
        }
        if (!string.IsNullOrEmpty(queryDto?.Title))
        {
            exp = exp.And(x => x.Title != null && x.Title.Contains(queryDto.Title));
        }
        if (queryDto?.TicketStatus.HasValue == true)
        {
            exp = exp.And(x => x.TicketStatus == queryDto.TicketStatus);
        }
        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority.Value);
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
        if (queryDto?.AssigneeId.HasValue == true)
        {
            exp = exp.And(x => x.AssigneeId == queryDto.AssigneeId);
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
