// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktSignalRDispatchService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 实时推送调度服务接口（强退、消息推送）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Foundation;
using Takt.Shared.Models.Workflow;

namespace Takt.Domain.Interfaces;

/// <summary>
/// SignalR 实时推送调度服务接口
/// </summary>
public interface ITaktSignalRDispatchService
{
    /// <summary>
    /// 强制踢出在线用户（强退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="reason">强退原因</param>
    /// <param name="connectionId">SignalR 连接 ID（主键查无记录时回退定位）</param>
    /// <returns>任务</returns>
    Task ForceKickOnlineAsync(long onlineId, string? reason = null, string? connectionId = null);

    /// <summary>
    /// 批量强制踢出在线用户
    /// </summary>
    /// <param name="onlineIds">在线用户记录 ID 列表</param>
    /// <param name="reason">强退原因</param>
    /// <returns>任务</returns>
    Task ForceKickOnlineBatchAsync(IEnumerable<long> onlineIds, string? reason = null);

    /// <summary>
    /// 推送私信到在线客户端
    /// </summary>
    /// <param name="message">私信推送模型</param>
    /// <returns>任务</returns>
    Task PushPrivateMessageAsync(TaktSignalRPrivateMessagePush message);

    /// <summary>
    /// 推送广播通知到公司内在线客户端
    /// </summary>
    /// <param name="broadcast">广播推送模型</param>
    /// <returns>任务</returns>
    Task PushBroadcastMessageAsync(TaktSignalRBroadcastPush broadcast);

    /// <summary>
    /// 向指定用户推送最新在线统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    Task PushOnlineStatisticsToUserAsync(string companyCode, string userName, long? userId = null);

    /// <summary>
    /// 向指定用户推送最新消息统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    Task PushMessageStatisticsToUserAsync(string companyCode, string userName, long? userId = null);

    /// <summary>
    /// 推送流程定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushFlowSchemeChangedAsync(TaktSignalRFlowSchemeChangedPush push);

    /// <summary>
    /// 向指定用户推送流程实例推进事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <param name="targetUserName">目标用户名</param>
    /// <returns>任务</returns>
    Task PushFlowInstanceProgressedToUserAsync(TaktSignalRFlowInstanceProgressedPush push, string targetUserName);

    /// <summary>
    /// 向指定用户推送最新待办数量
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushFlowTodoCountToUserAsync(TaktSignalRFlowTodoCountPush push);

    /// <summary>
    /// 推送定时任务定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushQuartzTaskChangedAsync(TaktSignalRQuartzTaskChangedPush push);

    /// <summary>
    /// 推送定时任务执行完成到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushQuartzTaskExecutedAsync(TaktSignalRQuartzTaskExecutedPush push);
}
