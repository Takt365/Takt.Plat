// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktConnectHub.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：连接 Hub，用于管理在线用户连接
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Identity;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Services;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// 连接 Hub，用于管理在线用户连接
/// </summary>
[Authorize]
public class TaktConnectHub : Hub
{
    private readonly ITaktOnlineService _onlineService;
    private readonly ITaktCompanyRepository<TaktOnline> _onlineRepository;
    private readonly ITaktSignalRDispatchService _signalRDispatchService;
    private readonly ITaktUserContext _userContext;
    private readonly ITaktAuthService _authService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="onlineService">在线用户服务</param>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="signalRDispatchService">SignalR 推送调度服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="authService">认证服务</param>
    public TaktConnectHub(
        ITaktOnlineService onlineService,
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktUserContext userContext,
        ITaktAuthService authService)
    {
        _onlineService = onlineService;
        _onlineRepository = onlineRepository;
        _signalRDispatchService = signalRDispatchService;
        _userContext = userContext;
        _authService = authService;
    }

    /// <summary>
    /// 客户端连接时调用：写入在线用户记录、加入用户组并推送上线事件
    /// </summary>
    /// <returns>任务</returns>
    public override async Task OnConnectedAsync()
    {
        var prevPrincipal = TaktUserContext.HubInvocationPrincipal;
        TaktUserContext.HubInvocationPrincipal = Context.User;
        try
        {
            var connectionId = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            RequireResolvedLoginUser();

            var userName = _userContext.UserName!.Trim();
            var userId = _userContext.UserId!.Value;
            var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
                _userContext,
                _authService,
                userId,
                userName);
            var (connectIp, connectLocation) = TaktLocationHelper.ResolveClientIpAndLocationForLog(httpContext);
            var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();
            var connectTime = DateTime.Now;
            var deviceType = ParseDeviceType(userAgent);
            var browserType = ParseBrowserType(userAgent);
            var operatingSystem = ParseOperatingSystem(userAgent);

            await _onlineService.RegisterOnlineSessionAsync(new TaktOnlineCreateDto
            {
                ConnectionId = connectionId ?? string.Empty,
                UserName = userName,
                UserId = userId,
                OnlineStatus = 0,
                ConnectIp = connectIp,
                ConnectLocation = connectLocation,
                UserAgent = userAgent,
                DeviceType = deviceType,
                BrowserType = browserType,
                OperatingSystem = operatingSystem,
                ConnectTime = connectTime,
            });

            if (!string.IsNullOrEmpty(connectionId))
            {
                await Groups.AddToGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, userName));
            }

            var userIdStr = userId.ToString();
            await Clients.Caller.SendAsync("OnlineMessage", new
            {
                Message = $"欢迎 {userName} 上线！连接成功，当前时间：{connectTime:yyyy-MM-dd HH:mm:ss}",
                UserName = userName,
                UserId = userIdStr,
                ConnectTime = connectTime,
                ConnectIp = connectIp,
                ConnectLocation = connectLocation,
                DeviceType = deviceType,
            });

            await Clients.Others.SendAsync("UserConnected", new
            {
                UserName = userName,
                UserId = userIdStr,
                ConnectTime = connectTime,
                ConnectIp = connectIp,
                ConnectLocation = connectLocation,
            });

            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(companyCode, userName, userId);

            TaktSignalRLogging.LogHubConnected(
                nameof(TaktConnectHub),
                connectionId,
                userName,
                userId,
                companyCode,
                _userContext.TenantCode,
                connectIp,
                connectLocation);
            await base.OnConnectedAsync();
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 客户端断开连接时调用：更新离线状态、移出用户组并通知其他客户端
    /// </summary>
    /// <param name="exception">断开异常；正常断开时为 null</param>
    /// <returns>任务</returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var prevPrincipal = TaktUserContext.HubInvocationPrincipal;
        TaktUserContext.HubInvocationPrincipal = Context.User;
        try
        {
            var connectionId = Context.ConnectionId;
            RequireResolvedLoginUser();
            var userName = _userContext.UserName!.Trim();
            var userId = _userContext.UserId!.Value;
            var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
                _userContext,
                _authService,
                userId,
                userName);

            var httpContext = Context.GetHttpContext();
            var disconnectIp = TaktLocationHelper.ResolveClientIp(httpContext);

            var online = await _onlineRepository.FirstAsync(o => o.ConnectionId == connectionId);
            string? disconnectLocation = null;
            if (online != null)
            {
                var disconnectTime = DateTime.Now;
                if (string.IsNullOrWhiteSpace(online.ConnectIp) && !string.IsNullOrWhiteSpace(disconnectIp))
                {
                    online.ConnectIp = disconnectIp;
                }

                online.ConnectLocation = TaktLocationHelper.ResolveIpAndLocationForLog(
                    online.ConnectIp,
                    online.ConnectLocation).Location;
                disconnectLocation = online.ConnectLocation;
                online.DisconnectTime = disconnectTime;
                online.ConnectionDuration = (int)(disconnectTime - online.ConnectTime).TotalSeconds;
                online.OnlineStatus = 1;
                await _onlineRepository.UpdateAsync(online);
            }

            if (!string.IsNullOrEmpty(connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, userName));
            }

            await Clients.Others.SendAsync("UserDisconnected", new
            {
                UserName = userName,
                DisconnectTime = DateTime.Now,
            });

            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(companyCode, userName, _userContext.UserId);

            TaktSignalRLogging.LogHubDisconnected(
                nameof(TaktConnectHub),
                connectionId,
                userName,
                _userContext.UserId,
                companyCode,
                _userContext.TenantCode,
                exception,
                online?.ConnectIp ?? disconnectIp,
                disconnectLocation);
            await base.OnDisconnectedAsync(exception);
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 心跳更新
    /// </summary>
    /// <returns>任务</returns>
    public async Task Heartbeat()
    {
        RequireResolvedLoginUser();
        var connectionId = Context.ConnectionId;
        var online = await _onlineRepository.FirstAsync(o => o.ConnectionId == connectionId);
        if (online != null)
        {
            online.LastActiveTime = DateTime.Now;
            await _onlineRepository.UpdateAsync(online);
        }

        var userName = _userContext.UserName!.Trim();
        var userId = _userContext.UserId!.Value;
        var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
            _userContext,
            _authService,
            userId,
            userName);
        if (!string.IsNullOrWhiteSpace(companyCode) && !string.IsNullOrWhiteSpace(userName))
        {
            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(companyCode, userName, _userContext.UserId);
        }
    }

    /// <summary>
    /// 获取在线用户列表
    /// </summary>
    /// <returns>在线用户列表</returns>
    public async Task<List<object>> GetOnlineUsers()
    {
        RequireResolvedLoginUser();
        var userName = _userContext.UserName!.Trim();
        var userId = _userContext.UserId!.Value;
        var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
            _userContext,
            _authService,
            userId,
            userName);
        var onlines = await _onlineRepository.GetListAsync(
            o => o.OnlineStatus == 0 && o.CompanyCode == companyCode);
        return onlines.Select(u => (object)new
        {
            UserName = u.UserName,
            UserId = u.UserId,
            ConnectTime = u.ConnectTime,
            LastActiveTime = u.LastActiveTime,
            ConnectIp = u.ConnectIp,
            ConnectLocation = TaktLocationHelper.ResolveIpAndLocationForLog(u.ConnectIp, u.ConnectLocation).Location,
        }).ToList();
    }

    /// <summary>
    /// 校验当前 Hub 调用已解析登录用户
    /// </summary>
    private void RequireResolvedLoginUser()
    {
        var (userId, userName) = ResolveUserFromContext();
        if (!userId.HasValue || userId.Value <= 0 || string.IsNullOrWhiteSpace(userName))
        {
            throw new HubException("无法解析当前登录用户，请重新登录后重试。");
        }
    }

    /// <summary>
    /// 从 Hub Claims 或用户上下文解析用户
    /// </summary>
    /// <returns>用户 ID 与用户名</returns>
    private (long? UserId, string? UserName) ResolveUserFromContext()
    {
        var principal = Context.User ?? TaktUserContext.HubInvocationPrincipal;
        var sub = principal?.FindFirst("sub")?.Value ?? principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var name = principal?.FindFirst("name")?.Value ?? principal?.FindFirst(ClaimTypes.Name)?.Value;

        long? userId = long.TryParse(sub, out var parsedId) ? parsedId : _userContext.UserId;
        var userName = !string.IsNullOrWhiteSpace(name) ? name : _userContext.UserName;
        return (userId, userName);
    }

    /// <summary>
    /// 解析 User-Agent 中的设备类型
    /// </summary>
    /// <param name="userAgent">User-Agent 字符串</param>
    /// <returns>设备类型（PC、Mobile、Tablet 等）</returns>
    private static int? ParseDeviceType(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
        {
            return null;
        }

        var ua = userAgent.ToLowerInvariant();
        if (ua.Contains("tablet") || ua.Contains("ipad"))
        {
            return 3;
        }

        if (ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone"))
        {
            return 2;
        }

        return 1;
    }

    /// <summary>
    /// 解析 User-Agent 中的浏览器类型
    /// </summary>
    /// <param name="userAgent">User-Agent 字符串</param>
    /// <returns>浏览器类型</returns>
    private static int? ParseBrowserType(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
        {
            return null;
        }

        var ua = userAgent.ToLowerInvariant();
        if (ua.Contains("chrome") && !ua.Contains("edg"))
        {
            return 1;
        }

        if (ua.Contains("firefox"))
        {
            return 2;
        }

        if (ua.Contains("safari") && !ua.Contains("chrome"))
        {
            return 3;
        }

        if (ua.Contains("edg"))
        {
            return 4;
        }

        return 0;
    }

    /// <summary>
    /// 解析 User-Agent 中的操作系统
    /// </summary>
    /// <param name="userAgent">User-Agent 字符串</param>
    /// <returns>操作系统</returns>
    private static int? ParseOperatingSystem(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
        {
            return null;
        }

        var ua = userAgent.ToLowerInvariant();
        if (ua.Contains("windows"))
        {
            return 1;
        }

        if (ua.Contains("mac os") || ua.Contains("macos"))
        {
            return 2;
        }

        if (ua.Contains("linux"))
        {
            return 3;
        }

        if (ua.Contains("android"))
        {
            return 4;
        }

        if (ua.Contains("ios") || ua.Contains("iphone") || ua.Contains("ipad"))
        {
            return 5;
        }

        return 0;
    }
}
