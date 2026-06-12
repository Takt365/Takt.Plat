// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobSignalRPushService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz Job 执行完成 SignalR 推送（Infrastructure 调度层，失败抛异常）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Shared.Models.Foundation;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz Job 执行完成 SignalR 推送实现
/// </summary>
public sealed class TaktQuartzJobSignalRPushService : ITaktQuartzJobSignalRPushService
{
    private readonly ITaktSignalRDispatchService _signalRDispatchService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="signalRDispatchService">SignalR 调度服务</param>
    public TaktQuartzJobSignalRPushService(ITaktSignalRDispatchService signalRDispatchService)
    {
        ArgumentNullException.ThrowIfNull(signalRDispatchService);
        _signalRDispatchService = signalRDispatchService;
    }

    /// <inheritdoc />
    public Task PushTaskExecutedAsync(
        TaktQuartzTask task,
        TaktQuartzLog log,
        string? triggerUserName)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(log);
        var push = new TaktSignalRQuartzTaskExecutedPush
        {
            TenantCode = task.TenantCode,
            CompanyCode = task.CompanyCode,
            QuartzTaskId = task.Id,
            QuartzLogId = log.Id,
            TaskCode = task.TaskCode ?? string.Empty,
            TaskName = task.TaskName ?? string.Empty,
            ExecuteStatus = (int)log.ExecuteStatus,
            ExecuteDuration = log.ExecuteDuration,
            ExecuteCount = task.ExecuteCount,
            LastRunAt = task.LastRunAt,
            NextRunAt = task.NextRunAt,
            TriggerUserName = triggerUserName?.Trim(),
            ExecutedAt = log.ExecuteTime,
        };
        return _signalRDispatchService.PushQuartzTaskExecutedAsync(push);
    }
}
