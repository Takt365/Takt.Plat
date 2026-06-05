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
    private readonly ITaktMessageService _messageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectHubContext">连接 Hub 上下文</param>
    /// <param name="notificationHubContext">通知 Hub 上下文</param>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="onlineService">在线用户服务</param>
    /// <param name="messageService">在线消息服务</param>
    public TaktSignalRDispatchService(
        IHubContext<TaktConnectHub> connectHubContext,
        IHubContext<TaktNotificationHub> notificationHubContext,
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktOnlineService onlineService,
        ITaktMessageService messageService)
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
    /// <returns>任务</returns>
    public async Task ForceKickOnlineAsync(long onlineId, string? reason = null)
    {
        var online = await _onlineRepository.GetByIdAsync(onlineId);
        if (online == null)
        {
            throw new TaktBusinessException("在线用户不存在");
        }

        if (online.OnlineStatus != TaktOnlineStatus.Online)
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

        if (!string.IsNullOrEmpty(online.ConnectionId))
        {
            await _connectHubContext.Clients.Client(online.ConnectionId).SendAsync("ForceLogout", payload);
            await _notificationHubContext.Clients.Client(online.ConnectionId).SendAsync("ForceLogout", payload);
        }

        await _connectHubContext.Clients.Group(userGroup).SendAsync("ForceLogout", payload);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("ForceLogout", payload);

        var disconnectTime = DateTime.Now;
        online.ConnectLocation = TaktLocationHelper.ResolveIpLocationForLogOrKeep(
            online.ConnectIp,
            online.ConnectLocation);
        online.OnlineStatus = TaktOnlineStatus.Away;
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
                await ForceKickOnlineAsync(onlineId, reason);
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
        var payload = new
        {
            MessageId = message.MessageId.ToString(),
            message.FromUserName,
            FromUserId = message.FromUserId?.ToString(),
            message.ToUserName,
            ToUserId = message.ToUserId?.ToString(),
            message.MessageTitle,
            message.MessageContent,
            message.MessageType,
            message.MessageGroup,
            message.ReadStatus,
            message.ReadTime,
            message.SendTime,
            message.MessageExtData,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.UserGroup(message.CompanyCode, message.ToUserName))
            .SendAsync("ReceiveMessage", payload);

        await PushMessageStatisticsToUserAsync(message.CompanyCode, message.ToUserName, message.ToUserId);
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
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("OnlineStatisticsUpdated", payload);
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

        var statistics = await _messageService.GetMessageStatisticsByUserNameAsync(userName, userId);
        var payload = ToMessageStatisticsPayload(statistics);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, userName);

        await _connectHubContext.Clients.Group(userGroup).SendAsync("MessageStatisticsUpdated", payload);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("MessageStatisticsUpdated", payload);
        TaktSignalRLogging.LogStatisticsPushed("message", userName, companyCode);
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
