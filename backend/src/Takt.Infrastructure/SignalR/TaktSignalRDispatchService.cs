// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRDispatchService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 实时推送调度服务（强退、私信、广播）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.SignalR;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Shared.Models.Foundation;
using Takt.Shared.Models.Workflow;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR 实时推送调度服务
/// </summary>
public class TaktSignalRDispatchService : ITaktSignalRDispatchService
{
    private readonly IHubContext<TaktConnectHub> _connectHubContext;
    private readonly IHubContext<TaktNotificationHub> _notificationHubContext;
    private readonly ITaktCompanyRepository<TaktOnline> _onlineRepository;
    private readonly ITaktOnlineService _onlineService;
    private readonly Lazy<ITaktMessageService> _messageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectHubContext">连接 Hub 上下文</param>
    /// <param name="notificationHubContext">通知 Hub 上下文</param>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="onlineService">在线用户服务</param>
    /// <param name="messageService">在线消息服务（延迟解析，避免与 TaktMessageService 循环依赖）</param>
    public TaktSignalRDispatchService(
        IHubContext<TaktConnectHub> connectHubContext,
        IHubContext<TaktNotificationHub> notificationHubContext,
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktOnlineService onlineService,
        Lazy<ITaktMessageService> messageService)
    {
        _connectHubContext = connectHubContext;
        _notificationHubContext = notificationHubContext;
        _onlineRepository = onlineRepository;
        _onlineService = onlineService;
        _messageService = messageService;
    }

    /// <summary>
    /// 强制踢出在线用户（强退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="reason">强退原因</param>
    /// <param name="connectionId">SignalR 连接 ID（主键查无记录时回退定位）</param>
    /// <returns>任务</returns>
    public async Task ForceKickOnlineAsync(long onlineId, string? reason = null, string? connectionId = null)
    {
        var online = await ResolveOnlineForForceKickAsync(onlineId, connectionId);

        if (online.OnlineStatus != 0)
        {
            throw new TaktBusinessException("该用户已不在线，无法强退");
        }

        var kickReason = string.IsNullOrWhiteSpace(reason) ? "您已被管理员强制下线" : reason.Trim();
        var payload = new
        {
            Message = kickReason,
            OnlineId = online.Id.ToString(),
            ConnectionId = online.ConnectionId,
            UserName = online.UserName,
            ForceKickTime = DateTime.Now,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(online.CompanyCode, online.UserName);
        var onlineConnectionId = online.ConnectionId?.Trim();
        // 强退仅经 ConnectHub 推送一次（Client 与 Group 二选一，避免同连接重复投递）
        if (!string.IsNullOrEmpty(onlineConnectionId))
        {
            await _connectHubContext.Clients.Client(onlineConnectionId).SendAsync("ForceLogout", payload);
        }
        else
        {
            await _connectHubContext.Clients.Group(userGroup).SendAsync("ForceLogout", payload);
        }

        var disconnectTime = DateTime.Now;
        online.ConnectLocation = TaktLocationHelper.ResolveIpAndLocationForLog(
            online.ConnectIp,
            online.ConnectLocation).Location;
        online.OnlineStatus = 2;
        online.DisconnectTime = disconnectTime;
        online.ConnectionDuration = (int)(disconnectTime - online.ConnectTime).TotalSeconds;
        online.Remark = $"强退: {kickReason}";
        online.UpdatedAt = disconnectTime;
        await _onlineRepository.UpdateAsync(online);

        await PushOnlineStatisticsToUserAsync(online.CompanyCode, online.UserName, online.UserId);

        TaktLogger.Information("强退在线用户成功，用户: {UserName}, ConnectionId: {ConnectionId}", online.UserName, online.ConnectionId);
    }

    /// <summary>
    /// 批量强制踢出在线用户
    /// </summary>
    /// <param name="onlineIds">在线用户记录 ID 列表</param>
    /// <param name="reason">强退原因</param>
    /// <returns>任务</returns>
    public async Task ForceKickOnlineBatchAsync(IEnumerable<long> onlineIds, string? reason = null)
    {
        var idList = onlineIds?.Distinct().ToList() ?? new List<long>();
        foreach (var onlineId in idList)
        {
            try
            {
                await ForceKickOnlineAsync(onlineId, reason, null);
            }
            catch (TaktBusinessException ex)
            {
                TaktLogger.Warning("批量强退跳过记录 {OnlineId}: {Message}", onlineId, ex.Message);
            }
        }
    }

    /// <summary>
    /// 推送私信到在线客户端
    /// </summary>
    /// <param name="message">私信推送模型</param>
    /// <returns>任务</returns>
    public async Task PushPrivateMessageAsync(TaktSignalRPrivateMessagePush message)
    {
        var companyCode = message.CompanyCode?.Trim() ?? string.Empty;
        var toUserName = message.ToUserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(toUserName))
        {
            throw new TaktBusinessException("私信推送缺少公司编码或接收者用户名");
        }

        var payload = new
        {
            MessageId = message.MessageId.ToString(),
            message.FromUserName,
            FromUserId = message.FromUserId?.ToString(),
            ToUserName = toUserName,
            ToUserId = message.ToUserId?.ToString(),
            message.MessageTitle,
            message.MessageContent,
            message.Attachments,
            message.MessageType,
            message.MessageGroup,
            message.SendTime,
            message.ReadTime,
            message.ReadStatus,
        };

        var recipientUserId = message.ToUserId.HasValue && message.ToUserId.Value > 0
            ? message.ToUserId.Value.ToString()
            : null;
        var targetCompanyCodes = await ResolvePrivateMessageTargetCompanyCodesAsync(companyCode, toUserName);
        foreach (var targetCompanyCode in targetCompanyCodes)
        {
            var userGroup = TaktSignalRGroupNames.UserGroup(targetCompanyCode, toUserName);
            await _notificationHubContext.Clients.Group(userGroup).SendAsync("ReceiveMessage", payload);
            TaktSignalRLogging.LogPrivateMessagePushed(toUserName, targetCompanyCode, message.MessageId, userGroup, recipientUserId);
            await PushMessageStatisticsToUserAsync(targetCompanyCode, toUserName, message.ToUserId);
        }
    }

    /// <summary>
    /// 解析私信推送目标公司组（优先接收者当前在线会话所在公司，否则回退消息公司）
    /// </summary>
    /// <param name="messageCompanyCode">消息所属公司编码</param>
    /// <param name="toUserName">接收者登录用户名</param>
    /// <returns>去重后的公司编码列表</returns>
    private async Task<IReadOnlyList<string>> ResolvePrivateMessageTargetCompanyCodesAsync(
        string messageCompanyCode,
        string toUserName)
    {
        var onlineSessions = await _onlineRepository.GetListAsync(online =>
            online.OnlineStatus == 0
            && online.UserName == toUserName);
        var onlineCompanyCodes = onlineSessions
            .Select(online => online.CompanyCode?.Trim())
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Cast<string>()
            .ToList();
        if (onlineCompanyCodes.Count == 0)
        {
            return new[] { messageCompanyCode };
        }

        var matchedMessageCompany = onlineCompanyCodes.FirstOrDefault(code =>
            string.Equals(code, messageCompanyCode, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(matchedMessageCompany))
        {
            return new[] { matchedMessageCompany };
        }

        return onlineCompanyCodes;
    }

    /// <summary>
    /// 推送广播通知到公司内在线客户端
    /// </summary>
    /// <param name="broadcast">广播推送模型</param>
    /// <returns>任务</returns>
    public async Task PushBroadcastMessageAsync(TaktSignalRBroadcastPush broadcast)
    {
        if (string.IsNullOrWhiteSpace(broadcast.CompanyCode))
        {
            throw new TaktBusinessException("广播消息缺少公司编码");
        }

        var payload = new
        {
            broadcast.FromUserName,
            broadcast.MessageTitle,
            broadcast.MessageContent,
            broadcast.MessageType,
            broadcast.MessageGroup,
            SendTime = broadcast.SendTime ?? DateTime.Now,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(broadcast.CompanyCode))
            .SendAsync("ReceiveBroadcast", payload);
    }

    /// <summary>
    /// 向指定用户推送最新在线统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    public async Task PushOnlineStatisticsToUserAsync(string companyCode, string userName, long? userId = null)
    {
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(userName))
        {
            return;
        }

        var statistics = await _onlineService.GetOnlineStatisticsByUserNameAsync(userName, userId);
        var payload = ToOnlineStatisticsPayload(statistics);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, userName);

        await _connectHubContext.Clients.Group(userGroup).SendAsync("OnlineStatisticsUpdated", payload);
        TaktSignalRLogging.LogStatisticsPushed("online", userName, companyCode);
    }

    /// <summary>
    /// 向指定用户推送最新消息统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    public async Task PushMessageStatisticsToUserAsync(string companyCode, string userName, long? userId = null)
    {
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(userName))
        {
            return;
        }

        var statistics = await _messageService.Value.GetMessageStatisticsByUserNameAsync(userName, userId);
        var payload = ToMessageStatisticsPayload(statistics);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, userName);

        await _notificationHubContext.Clients.Group(userGroup).SendAsync("MessageStatisticsUpdated", payload);
        TaktSignalRLogging.LogStatisticsPushed("message", userName, companyCode);
    }

    /// <summary>
    /// 推送流程定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushFlowSchemeChangedAsync(TaktSignalRFlowSchemeChangedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new TaktBusinessException("流程定义推送缺少公司编码");
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            FlowSchemeId = push.FlowSchemeId.ToString(),
            push.ProcessKey,
            push.ProcessName,
            push.ChangeType,
            push.OperatorUserName,
            ChangedAt = push.ChangedAt,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("FlowSchemeChanged", payload);
        TaktSignalRLogging.LogWorkflowPushed("scheme-changed", companyCode, push.ProcessKey);
    }

    /// <summary>
    /// 向指定用户推送流程实例推进事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <param name="targetUserName">目标用户名</param>
    /// <returns>任务</returns>
    public async Task PushFlowInstanceProgressedToUserAsync(TaktSignalRFlowInstanceProgressedPush push, string targetUserName)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        var userName = targetUserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(userName))
        {
            return;
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            FlowInstanceId = push.FlowInstanceId.ToString(),
            push.InstanceCode,
            push.ProcessName,
            push.InstanceStatus,
            push.ActionType,
            push.CurrentActivityName,
            push.StartUserName,
            ProgressedAt = push.ProgressedAt,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, userName);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("FlowInstanceProgressed", payload);
        TaktSignalRLogging.LogWorkflowPushed("instance-progressed", companyCode, push.InstanceCode, userName);
    }

    /// <summary>
    /// 向指定用户推送最新待办数量
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushFlowTodoCountToUserAsync(TaktSignalRFlowTodoCountPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        var userName = push.UserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(userName))
        {
            return;
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            UserName = userName,
            UserId = push.UserId?.ToString(),
            push.TodoCount,
            UpdatedAt = push.UpdatedAt,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, userName);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("FlowTodoCountUpdated", payload);
        TaktSignalRLogging.LogWorkflowPushed("todo-count", companyCode, push.TodoCount.ToString(), userName);
    }

    /// <summary>
    /// 推送定时任务定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushQuartzTaskChangedAsync(TaktSignalRQuartzTaskChangedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new TaktBusinessException("定时任务推送缺少公司编码");
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            QuartzTaskId = push.QuartzTaskId.ToString(),
            push.TaskCode,
            push.TaskName,
            push.ChangeType,
            push.OperatorUserName,
            ChangedAt = push.ChangedAt,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("QuartzTaskChanged", payload);
        TaktSignalRLogging.LogQuartzPushed("task-changed", companyCode, push.TaskCode);
    }

    /// <summary>
    /// 推送定时任务执行完成到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushQuartzTaskExecutedAsync(TaktSignalRQuartzTaskExecutedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new TaktBusinessException("定时任务执行推送缺少公司编码");
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            QuartzTaskId = push.QuartzTaskId.ToString(),
            QuartzLogId = push.QuartzLogId.ToString(),
            push.TaskCode,
            push.TaskName,
            push.ExecuteStatus,
            push.ExecuteDuration,
            push.ExecuteCount,
            push.LastRunAt,
            push.NextRunAt,
            push.TriggerUserName,
            ExecutedAt = push.ExecutedAt,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("QuartzTaskExecuted", payload);
        TaktSignalRLogging.LogQuartzPushed("task-executed", companyCode, push.TaskCode);
    }

    /// <summary>
    /// 解析强退目标在线记录（主键优先，可选按 ConnectionId 回退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <returns>在线实体</returns>
    /// <exception cref="TaktBusinessException">记录不存在时抛出</exception>
    private async Task<TaktOnline> ResolveOnlineForForceKickAsync(long onlineId, string? connectionId)
    {
        TaktOnline? online = null;
        if (onlineId > 0)
        {
            online = await _onlineRepository.GetByIdAsync(onlineId);
        }

        var normalizedConnectionId = connectionId?.Trim();
        if (online == null && !string.IsNullOrEmpty(normalizedConnectionId))
        {
            var matches = await _onlineRepository.GetListAsync(o => o.ConnectionId == normalizedConnectionId);
            online = matches.FirstOrDefault();
        }

        if (online == null)
        {
            throw new TaktBusinessException("在线用户不存在");
        }

        return online;
    }

    /// <summary>
    /// 映射在线统计为 SignalR 推送载荷
    /// </summary>
    /// <param name="statistics">统计 DTO</param>
    /// <returns>推送载荷</returns>
    private static object ToOnlineStatisticsPayload(TaktOnlineStatisticsDto statistics)
    {
        return new
        {
            statistics.UserName,
            UserId = statistics.UserId?.ToString(),
            statistics.OnlineCount,
            statistics.CurrentDurationSeconds,
            statistics.TodayDurationSeconds,
            statistics.MonthDurationSeconds,
        };
    }

    /// <summary>
    /// 映射消息统计为 SignalR 推送载荷
    /// </summary>
    /// <param name="statistics">统计 DTO</param>
    /// <returns>推送载荷</returns>
    private static object ToMessageStatisticsPayload(TaktMessageStatisticsDto statistics)
    {
        return new
        {
            statistics.UserName,
            UserId = statistics.UserId?.ToString(),
            statistics.TotalCount,
            statistics.ReadCount,
            statistics.UnreadCount,
        };
    }
}
