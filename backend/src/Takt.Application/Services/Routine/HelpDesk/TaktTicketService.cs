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
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
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
    private readonly ITaktCompanyRepository<TaktTicketReply> _ticketReplyRepository;
    private readonly ITaktCompanyRepository<TaktTicketCategoryAssign> _categoryAssignRepository;
    private readonly ITaktCompanyRepository<TaktAsset> _assetRepository;
    private readonly ITaktCompanyRepository<TaktItAsset> _itAssetRepository;
    private readonly ITaktNumberingGenerator _numberingGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketRepository">工单仓储</param>
    /// <param name="ticketReplyRepository">工单回复仓储</param>
    /// <param name="categoryAssignRepository">工单分类默认处理人仓储</param>
    /// <param name="assetRepository">财务资产仓储</param>
    /// <param name="itAssetRepository">IT 设备保修扩展仓储</param>
    /// <param name="numberingGenerator">业务编号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketService(
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktCompanyRepository<TaktTicketReply> ticketReplyRepository,
        ITaktCompanyRepository<TaktTicketCategoryAssign> categoryAssignRepository,
        ITaktCompanyRepository<TaktAsset> assetRepository,
        ITaktCompanyRepository<TaktItAsset> itAssetRepository,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketRepository = ticketRepository;
        _ticketReplyRepository = ticketReplyRepository;
        _categoryAssignRepository = categoryAssignRepository;
        _assetRepository = assetRepository;
        _itAssetRepository = itAssetRepository;
        _numberingGenerator = numberingGenerator;
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
    /// 获取当前用户提交的工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketDto>> GetMyTicketListAsync(TaktTicketQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        if (!CurrentUserId.HasValue || CurrentUserId.Value <= 0)
        {
            ThrowBusinessException("无法确定当前用户");
        }
        queryDto.SubmitterId = CurrentUserId.Value;
        return await GetTicketListAsync(queryDto);
    }

    /// <summary>
    /// 获取当前用户提交的工单详情
    /// </summary>
    /// <param name="id">工单 ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto?> GetMyTicketByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        if (!CurrentUserId.HasValue || CurrentUserId.Value <= 0)
        {
            ThrowBusinessException("无法确定当前用户");
        }
        var entity = await GetTicketEntityAsync(id);
        if (entity == null || entity.SubmitterId != CurrentUserId.Value)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTicketDto>();
        await FillTicketDetailsAsync(dto, entity);
        return dto;
    }

    /// <summary>
    /// 获取当前用户工单关联的资产汇总（按 AssetCode 聚合）
    /// </summary>
    /// <param name="queryDto">分页查询</param>
    /// <returns>分页结果</returns>
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

    /// <summary>
    /// 根据ID获取工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>DTO</returns>
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

    /// <summary>
    /// 获取工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ticketRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TicketNo ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TicketNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> CreateTicketAsync(TaktTicketCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktTicket>();
        await ApplyItAssetLinkAsync(entity, dto.ItAssetId, dto.AssetCode);
        entity.TicketStatus = TaktTicketConstants.New;
        entity.ResolvedAt = null;
        entity.ClosedAt = null;
        if (string.IsNullOrWhiteSpace(entity.TicketNo))
        {
            entity.TicketNo = await GenerateTicketNoAsync();
        }
        await EnsureTicketNoUniqueAsync(entity.TicketNo);
        ApplyTicketPriorityFromMatrix(entity, dto.Urgency, dto.Impact);
        entity = await _ticketRepository.CreateAsync(entity);
        await TryAutoAssignAsync(entity);
        return await GetTicketByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketDto>();
    }

    /// <summary>
    /// 门户用户提交工单
    /// </summary>
    /// <param name="dto">提交 DTO</param>
    /// <returns>DTO</returns>
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
        createDto.Urgency = dto.Urgency;
        createDto.Impact = dto.Impact;
        return await CreateTicketAsync(createDto);
    }

    /// <summary>
    /// 邮件/API 渠道建单
    /// </summary>
    /// <param name="dto">渠道建单 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> CreateTicketFromChannelAsync(TaktTicketCreateFromChannelDto dto)
    {
        EnsureThreeLayerContext();
        var createDto = dto.Adapt<TaktTicketCreateDto>();
        createDto.TenantCode = CurrentTenantCode;
        createDto.CompanyCode = CurrentCompanyCode;
        createDto.TicketNo = await GenerateTicketNoAsync();
        createDto.TicketSource = (int)dto.TicketSource;
        createDto.Urgency = dto.Urgency;
        createDto.Impact = dto.Impact;
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

    /// <summary>
    /// 更新工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> UpdateTicketAsync(long id, TaktTicketUpdateDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(id);
        EnsureTicketEditable(entity);
        var previousStatus = entity.TicketStatus;
        dto.Adapt(entity);
        entity.TicketStatus = previousStatus;
        entity.AssigneeId = dto.AssigneeId ?? entity.AssigneeId;
        await ApplyItAssetLinkAsync(entity, dto.ItAssetId, dto.AssetCode);
        ApplyTicketPriorityFromMatrix(entity, dto.Urgency, dto.Impact);
        await EnsureTicketNoUniqueAsync(entity.TicketNo, id);
        await _ticketRepository.UpdateAsync(entity);
        return await GetTicketByIdAsync(id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 删除工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketByIdAsync(long id)
    {
        var entity = await GetTicketEntityOrThrowAsync(id);
        await _ticketReplyRepository.DeleteAsync(x => x.TicketId == entity.Id);        var deleted = await _ticketRepository.DeleteAsync(id);
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
    /// 更新工单状态（受状态机约束）
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> UpdateTicketStatusAsync(TaktTicketStatusDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(id: dto.TicketId);
        var current = GetNormalizedStatus(entity);
        var target = TaktTicketWorkflowHelper.NormalizeLegacyStatus(dto.TicketStatus);
        await TransitionTicketStatusAsync(entity, current, target, dto.TicketStatus.ToString(), null);
        return await GetTicketByIdAsync(dto.TicketId) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 指派或领取工单
    /// </summary>
    /// <param name="dto">指派 DTO</param>
    /// <returns>DTO</returns>
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
            ? TaktTicketConstants.InProgress
            : TaktTicketConstants.Assigned;
        await TransitionTicketStatusAsync(entity, current, target, "指派/领取工单", dto.Remark, 5);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 开始处理（已指派 → 处理中）
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> StartTicketProgressAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        EnsureAssigneeOrAgent(entity);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanStartProgress(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        await TransitionTicketStatusAsync(entity, current, TaktTicketConstants.InProgress, "开始处理", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 请求用户补充信息（处理中 → 等待用户回复）
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> WaitForRequesterAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        EnsureAssigneeOrAgent(entity);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanWaitForRequester(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        await TransitionTicketStatusAsync(entity, current, TaktTicketConstants.PendingConfirm, "等待用户回复", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 标记已解决（处理中 → 已解决）
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>DTO</returns>
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
        await TransitionTicketStatusAsync(entity, current, TaktTicketConstants.Completed, "标记已解决", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 用户确认关闭（已解决 → 已关闭）
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>DTO</returns>
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
        await TransitionTicketStatusAsync(entity, current, TaktTicketConstants.Closed, "用户确认关闭", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 重新打开工单
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> ReopenTicketAsync(TaktTicketWorkflowActionDto dto)
    {
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        var current = GetNormalizedStatus(entity);
        if (!TaktTicketWorkflowHelper.CanReopen(current))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketStatusTransitionInvalid);
        }
        if (current == TaktTicketConstants.Completed)
        {
            EnsureSubmitter(entity);
        }
        entity.ResolvedAt = null;
        entity.ClosedAt = null;
        await TransitionTicketStatusAsync(entity, current, TaktTicketConstants.Reopened, "重新打开", dto.Remark);
        return await GetTicketByIdAsync(entity.Id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 添加工单回复（用户/客服）
    /// </summary>
    /// <param name="dto">回复 DTO</param>
    /// <returns>回复 DTO</returns>
    public async Task<TaktTicketReplyDto> ReplyTicketAsync(TaktTicketSessionReplyCreateDto dto)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(dto.TicketContent))
        {
            ThrowBusinessException("回复内容不能为空");
        }
        var entity = await GetTicketEntityOrThrowAsync(dto.TicketId);
        var status = GetNormalizedStatus(entity);
        if (status == TaktTicketConstants.Closed)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketClosedReadonly);
        }
        var isSubmitter = CurrentUserId.HasValue && entity.SubmitterId == CurrentUserId.Value;
        var isAssignee = entity.AssigneeId.HasValue && CurrentUserId.HasValue && entity.AssigneeId == CurrentUserId.Value;
        var isInternal = dto.IsInternal == 1;
        if (isInternal)
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
        if (isInternal)
        {
            authorType = 0;
        }
        var reply = new TaktTicketReply
        {
            TicketId = entity.Id,
            AuthorType = authorType,
            AuthorId = CurrentUserId ?? 0,
            AuthorName = CurrentUserName,
            TicketReplyContent = dto.TicketContent.Trim(),
            Attachments = dto.Attachments,
            IsInternal = isInternal ? 1 : 0,
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
                TaktTicketConstants.InProgress,
                "用户回复，继续处理",
                null);
        }
        return reply.Adapt<TaktTicketReplyDto>();
    }

    /// <summary>
    /// 获取工单回复列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketReplyDto>> GetTicketReplyListAsync(TaktTicketReplyQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        if (!queryDto.TicketId.HasValue || queryDto.TicketId.Value <= 0)
        {
            ThrowBusinessException("工单ID无效");
        }
        var ticketEntity = await GetTicketEntityOrThrowAsync(queryDto.TicketId.Value);
        EnsureTicketParticipant(ticketEntity);
        queryDto.PageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        queryDto.PageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var ticketId = queryDto.TicketId.Value;
        Expression<Func<TaktTicketReply, bool>> predicate = x =>
            x.TicketId == ticketId
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode;
        if (!queryDto.IncludeInternal)
        {
            predicate = x =>
                x.TicketId == ticketId
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
                entity.TicketStatus = TaktTicketWorkflowHelper.MapLegacyImportStatus(entity.TicketStatus);
                ApplyTicketPriorityFromMatrix(entity, entity.Urgency, entity.Impact);
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
    // 工作流私有方法
    // ========================================

    /// <summary>
    /// 获取工单实体（租户/公司隔离）
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>工单实体；不存在或越权时返回 null</returns>
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
    /// <param name="id">工单ID</param>
    /// <returns>工单实体</returns>
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
    /// <param name="entity">工单实体</param>
    /// <returns>规范化后的状态值</returns>
    private static int GetNormalizedStatus(TaktTicket entity)
    {
        return TaktTicketWorkflowHelper.NormalizeLegacyStatus(entity.TicketStatus);
    }

    /// <summary>
    /// 执行状态流转并写库
    /// </summary>
    /// <param name="entity">工单实体</param>
    /// <param name="current">当前状态</param>
    /// <param name="target">目标状态</param>
    /// <param name="summary">变更摘要</param>
    /// <param name="reason">变更原因</param>
    /// <param name="changeType">变更类型</param>
    /// <returns>任务</returns>
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
    }

    /// <summary>
    /// 按分类默认处理人自动指派
    /// </summary>
    /// <param name="entity">工单实体</param>
    /// <returns>任务</returns>
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
        entity.TicketStatus = TaktTicketConstants.Assigned;
        await _ticketRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 生成工单编号
    /// </summary>
    /// <returns>工单编号</returns>
    private async Task<string> GenerateTicketNoAsync()
    {
        EnsureThreeLayerContext();
        try
        {
            var outcome = await _numberingGenerator.TryGenerateNextAsync(
                TaktTicketWorkflowHelper.TicketNumberRuleCode);
            if (outcome == null || string.IsNullOrWhiteSpace(outcome.BusinessCode))
            {
                return BuildFallbackTicketNo();
            }
            return outcome.BusinessCode;
        }
        catch (Exception ex)
        {
            LogWarning($"编号规则 {TaktTicketWorkflowHelper.TicketNumberRuleCode} 不可用: {ex.Message}");
            return BuildFallbackTicketNo();
        }
    }

    /// <summary>
    /// 编号规则不可用时的兜底工单号
    /// </summary>
    /// <returns>兜底工单编号</returns>
    private static string BuildFallbackTicketNo()
    {
        return $"TK{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

    /// <summary>
    /// 校验工单编号唯一
    /// </summary>
    /// <param name="ticketNo">工单编号</param>
    /// <param name="excludeId">排除的工单ID</param>
    /// <returns>任务</returns>
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
    /// <param name="entity">工单实体</param>
    private void EnsureTicketEditable(TaktTicket entity)
    {
        if (GetNormalizedStatus(entity) == TaktTicketConstants.Closed)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.TicketClosedReadonly);
        }
    }

    /// <summary>
    /// 校验当前用户为提交人或处理人
    /// </summary>
    /// <param name="entity">工单实体</param>
    private void EnsureTicketParticipant(TaktTicket entity)
    {
        if (!CurrentUserId.HasValue)
        {
            ThrowBusinessException("无法确定当前用户");
        }
        var isSubmitter = entity.SubmitterId == CurrentUserId.Value;
        var isAssignee = entity.AssigneeId.HasValue && entity.AssigneeId == CurrentUserId.Value;
        if (!isSubmitter && !isAssignee)
        {
            ThrowBusinessException("仅提交人或处理人可访问");
        }
    }

    /// <summary>
    /// 校验当前用户为提交人
    /// </summary>
    /// <param name="entity">工单实体</param>
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
    /// <param name="entity">工单实体</param>
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
    /// <param name="dto">工单 DTO</param>
    /// <param name="entity">工单实体</param>
    /// <returns>任务</returns>
    private async Task FillTicketDetailsAsync(TaktTicketDto dto, TaktTicket entity)
    {
        if (dto == null)
        {
            return;
        }
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
    /// <returns>任务</returns>
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
    /// <returns>任务</returns>
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
    /// <returns>任务</returns>
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
    /// 获取服务台工单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>服务台工单统计</returns>
    public async Task<TaktHelpDeskTicketStatDto> GetHelpDeskTicketStatAsync(TaktHelpDeskTicketStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.CreatedAtStart,
            queryDto.CreatedAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktTicket, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end;
        var monthTicketCount = await _ticketRepository.CountAsync(predicate);
        Expression<Func<TaktTicket, bool>> openPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && x.TicketStatus >= 0
            && x.TicketStatus <= 3;
        var monthOpenTicketCount = await _ticketRepository.CountAsync(openPredicate);
        Expression<Func<TaktTicket, bool>> closedPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && x.TicketStatus >= 4
            && x.TicketStatus <= 5;
        var monthClosedTicketCount = await _ticketRepository.CountAsync(closedPredicate);
        return new TaktHelpDeskTicketStatDto
        {
            StatMonth = statMonth,
            MonthTicketCount = monthTicketCount,
            MonthOpenTicketCount = monthOpenTicketCount,
            MonthClosedTicketCount = monthClosedTicketCount,
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 依据 ITSM 矩阵写入紧急度、影响范围并自动计算优先级（忽略客户端传入的 Priority）
    /// </summary>
    /// <param name="entity">工单实体</param>
    /// <param name="urgency">紧急度</param>
    /// <param name="impact">影响范围</param>
    private static void ApplyTicketPriorityFromMatrix(TaktTicket entity, int urgency, int impact)
    {
        entity.Urgency = TaktTicketPriorityHelper.NormalizeLevel(urgency);
        entity.Impact = TaktTicketPriorityHelper.NormalizeLevel(impact);
        entity.Priority = TaktTicketPriorityHelper.ResolvePriority(entity.Urgency, entity.Impact);
    }

    /// <summary>
    /// 构建工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicket, bool>> QueryExpression(TaktTicketQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicket>();
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TicketNo != null && x.TicketNo.Contains(keywords))
                || (x.TicketTitle != null && x.TicketTitle.Contains(keywords))
                || (x.TicketContent != null && x.TicketContent.Contains(keywords))
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
        if (!string.IsNullOrEmpty(queryDto?.TicketTitle))
        {
            exp = exp.And(x => x.TicketTitle != null && x.TicketTitle.Contains(queryDto.TicketTitle));
        }
        if (queryDto?.TicketStatus.HasValue == true)
        {
            exp = exp.And(x => x.TicketStatus == queryDto.TicketStatus);
        }
        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority.Value);
        }
        if (queryDto?.Urgency.HasValue == true)
        {
            exp = exp.And(x => x.Urgency == queryDto.Urgency.Value);
        }
        if (queryDto?.Impact.HasValue == true)
        {
            exp = exp.And(x => x.Impact == queryDto.Impact.Value);
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