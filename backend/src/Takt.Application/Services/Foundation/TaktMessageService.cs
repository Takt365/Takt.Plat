// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktMessageService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Models.Foundation;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 在线消息应用服务
/// </summary>
public class TaktMessageService : TaktServiceBase, ITaktMessageService
{
    /// <summary>
    /// 单批最大接收者数量
    /// </summary>
    private const int MaxBatchRecipients = 500;

    /// <summary>
    /// 指定用户列表模式最大接收者数量
    /// </summary>
    private const int MaxListRecipients = 5;

    private readonly ITaktCompanyRepository<TaktMessage> _messageRepository;
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktSignalRDispatchService _signalRDispatchService;
    private readonly ITaktPermissionService _permissionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="messageRepository">在线消息仓储</param>
    /// <param name="userRepository">用户仓储（解析 SignalR 组名用登录用户名）</param>
    /// <param name="signalRDispatchService">SignalR 推送调度</param>
    /// <param name="permissionService">权限服务（解析当前公司可接收用户）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMessageService(
        ITaktCompanyRepository<TaktMessage> messageRepository,
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktPermissionService permissionService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _signalRDispatchService = signalRDispatchService;
        _permissionService = permissionService;
    }

    /// <summary>
    /// 获取在线消息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMessageDto>> GetMessageListAsync(TaktMessageQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (items, total) = await _messageRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            orderBy: x => x.CreatedAt,
            isDesc: true);
        var dtos = items.Adapt<List<TaktMessageDto>>();
        return TaktPagedResult<TaktMessageDto>.Create(dtos, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 获取当前登录用户已读消息列表（分页）
    /// </summary>
    /// <param name="queryDto">已读列表查询 DTO</param>
    /// <returns>分页结果</returns>
    public Task<TaktPagedResult<TaktMessageDto>> GetMessageReadListAsync(TaktMessageInboxListQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        return GetMessageListAsync(ToInboxMessageQuery(queryDto, 1));
    }

    /// <summary>
    /// 获取当前登录用户未读消息列表（分页）
    /// </summary>
    /// <param name="queryDto">未读列表查询 DTO</param>
    /// <returns>分页结果</returns>
    public Task<TaktPagedResult<TaktMessageDto>> GetMessageUnreadListAsync(TaktMessageInboxListQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        return GetMessageListAsync(ToInboxMessageQuery(queryDto, 0));
    }

    /// <summary>
    /// 根据ID获取在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto?> GetMessageByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        var entity = await _messageRepository.GetByIdAsync(id);
        if (entity == null)
        {
            return null;
        }
        return entity.Adapt<TaktMessageDto>();
    }

    /// <summary>
    /// 获取在线消息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMessageOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _messageRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FromUserName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FromUserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建在线消息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> CreateMessageAsync(TaktMessageCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktMessage>();
        entity.TenantCode = CurrentTenantCode;
        entity.CompanyCode = CurrentCompanyCode;
        entity.ToUserId = await ResolveMessageToUserIdAsync(dto.ToUserId, dto.ToUserName);
        entity.ToUserName = await ResolveMessageUserNameAsync(entity.ToUserId, dto.ToUserName);
        entity.FromUserId = await ResolveMessageFromUserIdAsync(dto.FromUserId, dto.FromUserName);
        entity.FromUserName = await ResolveMessageUserNameAsync(entity.FromUserId, dto.FromUserName);
        entity.IsCc = dto.IsCc;
        entity.MessageTitle = dto.MessageTitle ?? string.Empty;
        if (entity.MessageGroup == default)
        {
            entity.MessageGroup = 1;
        }
        if (entity.SendTime == default)
        {
            entity.SendTime = DateTime.Now;
        }
        entity = await _messageRepository.CreateAsync(entity);
        var result = await GetMessageByIdAsync(entity.Id);
        if (result == null)
        {
            throw new TaktBusinessException("在线消息不存在");
        }
        return result;
    }

    /// <summary>
    /// 批量创建在线消息并 SignalR 推送给各接收者（全员或指定用户列表，逐人落库；自审计抄送仅一条）
    /// </summary>
    /// <param name="dto">批量创建 DTO</param>
    /// <returns>已落库主送消息列表（不含自审计抄送记录）</returns>
    public async Task<List<TaktMessageDto>> CreateAndSendMessagesAsync(TaktMessageBatchCreateDto dto)
    {
        EnsureThreeLayerContext();
        if (dto.SendToAll)
        {
            await EnsureSendToAllPermissionAsync();
        }
        else if (dto.ToUserIds != null && dto.ToUserIds.Count > MaxListRecipients)
        {
            throw new TaktBusinessException($"指定用户最多选择 {MaxListRecipients} 位");
        }

        var recipientUserIds = await ResolveBatchRecipientUserIdsAsync(dto);
        var senderUserId = await ResolveMessageFromUserIdAsync(dto.FromUserId, dto.FromUserName);
        var senderUserName = await ResolveMessageUserNameAsync(senderUserId, dto.FromUserName);
        recipientUserIds = recipientUserIds
            .Where(id => id != senderUserId)
            .Distinct()
            .ToList();
        if (recipientUserIds.Count == 0)
        {
            throw new TaktBusinessException(dto.SendToAll
                ? "当前公司下未找到可发送的接收者"
                : "所选接收者无效或无权访问当前公司");
        }

        if (!dto.SendToAll && recipientUserIds.Count > MaxListRecipients)
        {
            throw new TaktBusinessException($"指定用户最多选择 {MaxListRecipients} 位");
        }

        if (recipientUserIds.Count > MaxBatchRecipients)
        {
            throw new TaktBusinessException($"单次最多向 {MaxBatchRecipients} 位用户发送消息");
        }

        var sendTime = dto.SendTime == default ? DateTime.Now : dto.SendTime;
        var enableSelfAuditCc = dto.IsCc == 1;
        var createdList = new List<TaktMessageDto>(recipientUserIds.Count);
        foreach (var toUserId in recipientUserIds)
        {
            var toUserName = await ResolveMessageUserNameAsync(toUserId, null);
            var createDto = dto.Adapt<TaktMessageCreateDto>();
            createDto.FromUserId = senderUserId;
            createDto.FromUserName = senderUserName;
            createDto.ToUserId = toUserId;
            createDto.ToUserName = toUserName;
            createDto.SendTime = sendTime;
            createDto.IsCc = 0;
            createdList.Add(await CreateMessageAsync(createDto));
        }

        TaktMessageDto? selfAuditMessage = null;
        if (enableSelfAuditCc)
        {
            selfAuditMessage = await CreateSelfAuditCcMessageAsync(dto, senderUserId, senderUserName, sendTime);
        }

        var pushFailures = new List<string>();
        foreach (var created in createdList)
        {
            try
            {
                await SendMessageByIdAsync(created.MessageId);
            }
            catch (Exception ex)
            {
                var recipientName = created.ToUserName ?? created.MessageId.ToString();
                pushFailures.Add($"{recipientName}: {ex.Message}");
            }
        }

        if (selfAuditMessage != null)
        {
            try
            {
                await SendMessageByIdAsync(selfAuditMessage.MessageId);
            }
            catch (Exception ex)
            {
                pushFailures.Add($"{senderUserName}(自审计): {ex.Message}");
            }
        }

        if (pushFailures.Count > 0)
        {
            var totalTargets = createdList.Count + (selfAuditMessage != null ? 1 : 0);
            ThrowIfPushFailures(pushFailures, totalTargets);
        }

        return createdList;
    }

    /// <summary>
    /// 创建发送者自审计抄送消息（批量发送整批仅一条，主送为发送者本人）
    /// </summary>
    /// <param name="dto">批量创建 DTO</param>
    /// <param name="senderUserId">发送者用户 ID</param>
    /// <param name="senderUserName">发送者登录名</param>
    /// <param name="sendTime">发送时间</param>
    /// <returns>自审计消息 DTO</returns>
    private async Task<TaktMessageDto> CreateSelfAuditCcMessageAsync(
        TaktMessageBatchCreateDto dto,
        long senderUserId,
        string senderUserName,
        DateTime sendTime)
    {
        var createDto = dto.Adapt<TaktMessageCreateDto>();
        createDto.FromUserId = senderUserId;
        createDto.FromUserName = senderUserName;
        createDto.ToUserId = senderUserId;
        createDto.ToUserName = senderUserName;
        createDto.SendTime = sendTime;
        createDto.IsCc = 0;
        return await CreateMessageAsync(createDto);
    }

    /// <summary>
    /// 按消息 ID 经 SignalR 推送给接收者（须已落库）
    /// </summary>
    /// <param name="id">在线消息 ID</param>
    /// <returns>任务</returns>
    public async Task SendMessageByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        var message = await GetMessageByIdAsync(id);
        if (message == null)
        {
            throw new TaktBusinessException("在线消息不存在");
        }
        if (string.IsNullOrWhiteSpace(message.CompanyCode))
        {
            throw new TaktBusinessException("消息缺少公司编码，无法实时推送");
        }

        var pushTargets = TaktMessageRecipientHelper.CollectPushTargets(
            message.ToUserName,
            message.ToUserId,
            message.IsCc,
            message.FromUserName,
            message.FromUserId);
        if (pushTargets.Count == 0)
        {
            throw new TaktBusinessException("消息缺少接收者或抄送用户名");
        }

        var pushFailures = new List<string>();
        foreach (var target in pushTargets)
        {
            var toUserName = target.Name;
            var toUserIdToken = target.IdToken;
            try
            {
                var push = await BuildPrivateMessagePushForRecipientAsync(message, toUserName, toUserIdToken);
                await _signalRDispatchService.PushPrivateMessageAsync(push);
            }
            catch (Exception ex)
            {
                pushFailures.Add($"{toUserName}: {ex.Message}");
            }
        }

        if (pushFailures.Count > 0)
        {
            ThrowIfPushFailures(pushFailures, pushTargets.Count);
        }
    }

    /// <summary>
    /// 删除在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMessageByIdAsync(long id)
    {
        var deleted = await _messageRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("在线消息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除在线消息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMessageBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMessageByIdAsync(id);
        }
    }

    /// <summary>
    /// 标记在线消息为已读
    /// </summary>
    /// <param name="dto">已读 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> MarkMessageReadAsync(TaktMessageReadDto dto)
    {
        var entity = await _messageRepository.GetByIdAsync(dto.MessageId);
        if (entity == null)
        {
            throw new TaktBusinessException("在线消息不存在");
        }
        entity.ReadStatus = dto.ReadStatus;
        entity.ReadTime = dto.ReadTime ?? DateTime.Now;
        await _messageRepository.UpdateAsync(entity);
        return await GetMessageByIdAsync(dto.MessageId) ?? throw new TaktBusinessException("在线消息不存在");
    }

    /// <summary>
    /// 标记在线消息为未读
    /// </summary>
    /// <param name="dto">未读 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> MarkMessageUnreadAsync(TaktMessageUnreadDto dto)
    {
        var entity = await _messageRepository.GetByIdAsync(dto.MessageId);
        if (entity == null)
        {
            throw new TaktBusinessException("在线消息不存在");
        }
        entity.ReadStatus = dto.ReadStatus;
        entity.ReadTime = dto.ReadTime;
        await _messageRepository.UpdateAsync(entity);
        return await GetMessageByIdAsync(dto.MessageId) ?? throw new TaktBusinessException("在线消息不存在");
    }

    /// <summary>
    /// 获取指定用户未读消息数量（SignalR Hub 调用）
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>未读数量</returns>
    public Task<int> GetUnreadMessageCountAsync(string userName)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new TaktBusinessException("用户名不能为空");
        }

        return CountInboxMessagesAsync(userName.Trim(), recipientUserId: null, 0);
    }

    /// <summary>
    /// 导出在线消息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMessageAsync(TaktMessageQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMessageQueryDto());
        var list = await _messageRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMessageExportDto>(),
                sheetName ?? "在线消息数据",
                fileName ?? "在线消息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMessageExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "在线消息数据",
            fileName ?? "在线消息导出.xlsx");
    }

    /// <summary>
    /// 获取当前登录用户在线消息统计（接收消息：总数/已读/未读）
    /// </summary>
    /// <returns>统计 DTO</returns>
    public Task<TaktMessageStatisticsDto> GetMessageStatisticsAsync()
    {
        EnsureThreeLayerContext();
        return GetMessageStatisticsByUserNameAsync(RequireCurrentUserName(), CurrentUserId);
    }

    /// <summary>
    /// 获取指定用户在线消息统计（SignalR 实时推送调用）
    /// </summary>
    /// <param name="userName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>统计 DTO</returns>
    public async Task<TaktMessageStatisticsDto> GetMessageStatisticsByUserNameAsync(string userName, long? userId = null)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new TaktBusinessException("用户名不能为空");
        }

        var normalizedUserName = userName.Trim();
        long? recipientUserId = null;
        if (userId is > 0)
        {
            recipientUserId = userId;
        }
        else if (CurrentUserId is > 0)
        {
            recipientUserId = CurrentUserId;
        }

        var totalCount = await CountInboxMessagesAsync(normalizedUserName, recipientUserId);
        var readCount = await CountInboxMessagesAsync(normalizedUserName, recipientUserId, 1);
        var unreadCount = await CountInboxMessagesAsync(normalizedUserName, recipientUserId, 0);

        return new TaktMessageStatisticsDto
        {
            UserName = normalizedUserName,
            UserId = userId ?? CurrentUserId,
            TotalCount = totalCount,
            ReadCount = readCount,
            UnreadCount = unreadCount,
        };
    }

    /// <summary>
    /// 收件箱列表查询 Adapt 为通用消息查询，并固定当前用户为接收者与读取状态
    /// </summary>
    /// <param name="queryDto">收件箱列表查询 DTO</param>
    /// <param name="readStatus">读取状态</param>
    /// <returns>消息分页查询 DTO</returns>
    private TaktMessageQueryDto ToInboxMessageQuery(TaktMessageInboxListQueryDto queryDto, int readStatus)
    {
        var query = queryDto.Adapt<TaktMessageQueryDto>();
        query.ToUserName = RequireCurrentUserName();
        if (CurrentUserId is > 0)
        {
            query.ToUserId = CurrentUserId.Value;
        }
        query.ReadStatus = readStatus;
        return query;
    }

    /// <summary>
    /// 统计当前租户+公司下指定收件箱用户的消息数量
    /// </summary>
    /// <param name="normalizedUserName">规范化登录用户名</param>
    /// <param name="recipientUserId">接收者用户 ID；未知时可 null</param>
    /// <param name="readStatus">读取状态过滤；null 表示不限</param>
    /// <returns>消息数量</returns>
    private async Task<int> CountInboxMessagesAsync(
        string normalizedUserName,
        long? recipientUserId = null,
        int? readStatus = null)
    {
        var exp = Expressionable.Create<TaktMessage>()
            .And(message => message.TenantCode == CurrentTenantCode)
            .And(message => message.CompanyCode == CurrentCompanyCode);
        if (readStatus.HasValue)
        {
            exp = exp.And(message => message.ReadStatus == readStatus.Value);
        }

        exp = ApplyInboxRecipientFilter(exp, normalizedUserName, recipientUserId);
        return await _messageRepository.CountAsync(exp.ToExpression());
    }

    /// <summary>
    /// 解析并校验当前登录用户名
    /// </summary>
    /// <returns>用户名</returns>
    private string RequireCurrentUserName()
    {
        if (string.IsNullOrWhiteSpace(CurrentUserName))
        {
            throw new TaktBusinessException("无法解析当前登录用户");
        }

        return CurrentUserName.Trim();
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建在线消息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMessage, bool>> QueryExpression(TaktMessageQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMessage>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.FromUserName != null && x.FromUserName.Contains(keywords))
                || SqlFunc.ToString(x.FromUserId).Contains(keywords)
                || (x.ToUserName != null && x.ToUserName.Contains(keywords))
                || SqlFunc.ToString(x.ToUserId).Contains(keywords)
                || SqlFunc.ToString(x.IsCc).Contains(keywords)
                || (x.MessageTitle != null && x.MessageTitle.Contains(keywords))
                || (x.MessageContent != null && x.MessageContent.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.MessageType).Contains(keywords)
                || SqlFunc.ToString(x.MessageGroup).Contains(keywords)
                || SqlFunc.ToString(x.ReadStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ReadTime).Contains(keywords)
                || SqlFunc.ToString(x.SendTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.FromUserName))
        {
            exp = exp.And(x => x.FromUserName != null && x.FromUserName.Contains(queryDto.FromUserName));
        }

        if (queryDto?.FromUserId.HasValue == true)
        {
            exp = exp.And(x => x.FromUserId == queryDto.FromUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ToUserName))
        {
            exp = ApplyQueryRecipientUserNameFilter(exp, queryDto.ToUserName.Trim());
        }

        if (queryDto?.ToUserId.HasValue == true && queryDto.ToUserId.Value > 0)
        {
            exp = ApplyQueryRecipientUserIdFilter(exp, queryDto.ToUserId.Value);
        }

        if (queryDto?.IsCc.HasValue == true)
        {
            var isCc = queryDto.IsCc.Value;
            exp = exp.And(x => x.IsCc == isCc);
        }

        if (!string.IsNullOrEmpty(queryDto?.MessageTitle))
        {
            exp = exp.And(x => x.MessageTitle != null && x.MessageTitle.Contains(queryDto.MessageTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.MessageContent))
        {
            exp = exp.And(x => x.MessageContent != null && x.MessageContent.Contains(queryDto.MessageContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.MessageType.HasValue == true)
        {
            exp = exp.And(x => x.MessageType == queryDto.MessageType);
        }

        if (queryDto?.MessageGroup.HasValue == true)
        {
            exp = exp.And(x => x.MessageGroup == queryDto.MessageGroup);
        }

        if (queryDto?.ReadStatus.HasValue == true)
        {
            exp = exp.And(x => x.ReadStatus == queryDto.ReadStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ReadTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ReadTime >= queryDto.ReadTimeStart);
        }

        if (queryDto?.ReadTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ReadTime <= queryDto.ReadTimeEnd);
        }

        if (queryDto?.SendTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.SendTime >= queryDto.SendTimeStart);
        }

        if (queryDto?.SendTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.SendTime <= queryDto.SendTimeEnd);
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

    /// <summary>
    /// 构建 SignalR 私信推送模型（单接收者）
    /// </summary>
    /// <param name="message">在线消息 DTO</param>
    /// <param name="toUserName">接收者登录用户名</param>
    /// <param name="toUserIdToken">接收者用户 ID token</param>
    /// <returns>推送模型</returns>
    private async Task<TaktSignalRPrivateMessagePush> BuildPrivateMessagePushForRecipientAsync(
        TaktMessageDto message,
        string toUserName,
        string? toUserIdToken)
    {
        long? toUserId = null;
        if (!string.IsNullOrWhiteSpace(toUserIdToken)
            && long.TryParse(toUserIdToken.Trim(), out var parsedUserId)
            && parsedUserId > 0)
        {
            toUserId = parsedUserId;
        }

        if (!toUserId.HasValue || toUserId.Value <= 0)
        {
            var recipient = await _userRepository.FirstAsync(user =>
                user.TenantCode == CurrentTenantCode && user.Username == toUserName);
            toUserId = recipient?.Id;
        }

        return new TaktSignalRPrivateMessagePush
        {
            CompanyCode = string.IsNullOrWhiteSpace(message.CompanyCode)
                ? CurrentCompanyCode
                : message.CompanyCode.Trim(),
            MessageId = message.MessageId,
            FromUserName = message.FromUserName,
            FromUserId = message.FromUserId,
            ToUserName = toUserName,
            ToUserId = toUserId,
            MessageTitle = message.MessageTitle,
            MessageContent = message.MessageContent,
            Attachments = message.Attachments,
            MessageType = message.MessageType,
            MessageGroup = message.MessageGroup,
            SendTime = message.SendTime,
            ReadTime = message.ReadTime,
            ReadStatus = message.ReadStatus,
        };
    }

    /// <summary>
    /// 校验当前用户是否为超级管理员（全员发送仅 SuperAdmin 可用）
    /// </summary>
    private async Task EnsureSendToAllPermissionAsync()
    {
        var userId = CurrentUserId ?? 0;
        if (userId <= 0)
        {
            throw new TaktBusinessException("未登录，无法全员发送");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || user.UserType != 2)
        {
            throw new TaktBusinessException("全员发送仅超级管理员可用");
        }
    }

    /// <summary>
    /// 解析批量发送接收者用户 ID 列表
    /// </summary>
    /// <param name="dto">批量创建 DTO</param>
    /// <returns>去重后的接收者用户 ID</returns>
    private async Task<List<long>> ResolveBatchRecipientUserIdsAsync(TaktMessageBatchCreateDto dto)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(CurrentCompanyCode))
        {
            throw new TaktBusinessException("缺少公司编码，无法解析接收者");
        }

        if (dto.SendToAll)
        {
            return await GetCurrentCompanyAccessibleUserIdsAsync();
        }

        var parsedIds = ParseBatchRecipientUserIds(dto.ToUserIds);
        return await FilterCurrentCompanyAccessibleUserIdsAsync(parsedIds);
    }

    /// <summary>
    /// 解析批量 DTO 中的接收者用户 ID 列表（兼容前端 string 雪花 ID）
    /// </summary>
    /// <param name="toUserIds">接收者 ID token 列表</param>
    /// <returns>去重后的用户 ID</returns>
    private static List<long> ParseBatchRecipientUserIds(IEnumerable<string>? toUserIds)
    {
        if (toUserIds == null)
        {
            return new List<long>();
        }

        return toUserIds
            .Select(token =>
            {
                var trimmed = token?.Trim();
                return long.TryParse(trimmed, out var parsedId) ? parsedId : 0L;
            })
            .Where(id => id > 0)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// 获取当前公司下全部可接收用户 ID
    /// </summary>
    /// <returns>用户 ID 列表</returns>
    private async Task<List<long>> GetCurrentCompanyAccessibleUserIdsAsync()
    {
        var tenantUsers = await _userRepository.GetListAsync(user => user.TenantCode == CurrentTenantCode);
        var accessibleIds = new List<long>();
        foreach (var user in tenantUsers)
        {
            if (user.Id <= 0)
            {
                continue;
            }

            if (await _permissionService.HasCompanyAccessAsync(user.Id, CurrentTenantCode, CurrentCompanyCode))
            {
                accessibleIds.Add(user.Id);
            }
        }

        return accessibleIds.Distinct().ToList();
    }

    /// <summary>
    /// 过滤出有权访问当前公司的接收者用户 ID
    /// </summary>
    /// <param name="candidateUserIds">候选用户 ID</param>
    /// <returns>过滤后的用户 ID 列表</returns>
    private async Task<List<long>> FilterCurrentCompanyAccessibleUserIdsAsync(IReadOnlyList<long> candidateUserIds)
    {
        var result = new List<long>();
        foreach (var userId in candidateUserIds)
        {
            if (await _permissionService.HasCompanyAccessAsync(userId, CurrentTenantCode, CurrentCompanyCode))
            {
                result.Add(userId);
            }
        }

        return result.Distinct().ToList();
    }

    /// <summary>
    /// 解析消息接收者用户 ID
    /// </summary>
    /// <param name="userId">表单或 DTO 传入的用户 ID</param>
    /// <param name="userName">接收者用户名</param>
    /// <returns>用户 ID</returns>
    /// <exception cref="TaktBusinessException">无法解析时抛出</exception>
    private async Task<long> ResolveMessageToUserIdAsync(long userId, string? userName)
    {
        if (userId > 0)
        {
            return userId;
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var normalizedUserName = NormalizeMessageUserNameLabel(userName);
            var recipient = await _userRepository.FirstAsync(user =>
                user.TenantCode == CurrentTenantCode && user.Username == normalizedUserName);
            if (recipient != null && recipient.Id > 0)
            {
                return recipient.Id;
            }
        }

        throw new TaktBusinessException("无法解析接收者用户 ID");
    }

    /// <summary>
    /// 解析消息发送者用户 ID
    /// </summary>
    /// <param name="userId">表单或 DTO 传入的用户 ID</param>
    /// <param name="userName">发送者用户名</param>
    /// <returns>用户 ID</returns>
    /// <exception cref="TaktBusinessException">无法解析时抛出</exception>
    private async Task<long> ResolveMessageFromUserIdAsync(long userId, string? userName)
    {
        if (userId > 0)
        {
            return userId;
        }

        if (CurrentUserId is > 0)
        {
            return CurrentUserId.Value;
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var normalizedUserName = userName.Trim();
            var sender = await _userRepository.FirstAsync(user =>
                user.TenantCode == CurrentTenantCode && user.Username == normalizedUserName);
            if (sender != null && sender.Id > 0)
            {
                return sender.Id;
            }
        }

        throw new TaktBusinessException("无法解析发送者用户 ID");
    }

    /// <summary>
    /// 按登录用户名解析用户 ID 字符串
    /// </summary>
    /// <param name="userName">登录用户名</param>
    /// <returns>用户 ID 字符串；查无用户时返回 null</returns>
    private async Task<string?> ResolveMessageUserIdFromUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return null;
        }

        var recipient = await _userRepository.FirstAsync(user =>
            user.TenantCode == CurrentTenantCode && user.Username == userName.Trim());
        return recipient == null || recipient.Id <= 0 ? null : recipient.Id.ToString();
    }

    /// <summary>
    /// 解析消息收发用户名（SignalR 组名须与登录 Username 一致，非下拉展示文案）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="userName">表单传入用户名或展示标签</param>
    /// <returns>规范化登录用户名</returns>
    /// <exception cref="TaktBusinessException">无法解析时抛出</exception>
    private async Task<string> ResolveMessageUserNameAsync(long? userId, string? userName)
    {
        if (userId.HasValue && userId.Value > 0)
        {
            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user != null && !string.IsNullOrWhiteSpace(user.Username))
            {
                return user.Username.Trim();
            }
        }

        var normalized = NormalizeMessageUserNameLabel(userName);
        if (!string.IsNullOrWhiteSpace(normalized))
        {
            return normalized;
        }

        throw new TaktBusinessException("无法解析消息用户登录名");
    }

    /// <summary>
    /// 从下拉展示标签提取登录用户名（如 admin (昵称) → admin）
    /// </summary>
    /// <param name="userName">用户名或展示标签</param>
    /// <returns>登录用户名；无法解析时返回空串</returns>
    private static string NormalizeMessageUserNameLabel(string? userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return string.Empty;
        }

        var trimmed = userName.Trim();
        var parenIndex = trimmed.IndexOf('(');
        if (parenIndex > 0)
        {
            return trimmed[..parenIndex].Trim();
        }

        return trimmed;
    }

    /// <summary>
    /// 推送失败时抛出业务异常（含部分失败摘要）
    /// </summary>
    /// <param name="failures">失败明细</param>
    /// <param name="totalTargets">目标总数</param>
    private static void ThrowIfPushFailures(IReadOnlyList<string> failures, int totalTargets)
    {
        if (failures.Count == 0)
        {
            return;
        }

        var sample = string.Join("; ", failures.Take(5));
        throw new TaktBusinessException(
            failures.Count == totalTargets
                ? $"消息推送失败: {sample}"
                : $"部分接收者推送失败（{failures.Count}/{totalTargets}）: {sample}");
    }

    /// <summary>
    /// 收件箱查询：主送用户名或 IsCc 自审计发送者匹配
    /// </summary>
    /// <param name="exp">已有表达式</param>
    /// <param name="recipientName">接收者登录名</param>
    /// <returns>叠加后的表达式</returns>
    private static Expressionable<TaktMessage> ApplyQueryRecipientUserNameFilter(
        Expressionable<TaktMessage> exp,
        string recipientName)
    {
        return exp.And(message =>
            message.ToUserName == recipientName
            || (message.IsCc == 1 && message.FromUserName == recipientName));
    }

    /// <summary>
    /// 收件箱查询：主送用户 ID 或 IsCc 自审计发送者 ID 匹配
    /// </summary>
    /// <param name="exp">已有表达式</param>
    /// <param name="recipientUserId">接收者用户 ID</param>
    /// <returns>叠加后的表达式</returns>
    private static Expressionable<TaktMessage> ApplyQueryRecipientUserIdFilter(
        Expressionable<TaktMessage> exp,
        long recipientUserId)
    {
        return exp.And(message =>
            message.ToUserId == recipientUserId
            || (message.IsCc == 1 && message.FromUserId == recipientUserId));
    }

    /// <summary>
    /// 收件箱统计/计数：主送 + IsCc 自审计（用户名与 ID 均可匹配）
    /// </summary>
    /// <param name="exp">已有表达式</param>
    /// <param name="normalizedUserName">规范化登录用户名</param>
    /// <param name="recipientUserId">接收者用户 ID；未知时可 null</param>
    /// <returns>叠加后的表达式</returns>
    private static Expressionable<TaktMessage> ApplyInboxRecipientFilter(
        Expressionable<TaktMessage> exp,
        string normalizedUserName,
        long? recipientUserId)
    {
        if (recipientUserId is > 0)
        {
            var userId = recipientUserId.Value;
            return exp.And(message =>
                message.ToUserName == normalizedUserName
                || message.ToUserId == userId
                || (message.IsCc == 1 && message.FromUserName == normalizedUserName)
                || (message.IsCc == 1 && message.FromUserId == userId));
        }

        return exp.And(message =>
            message.ToUserName == normalizedUserName
            || (message.IsCc == 1 && message.FromUserName == normalizedUserName));
    }
}
