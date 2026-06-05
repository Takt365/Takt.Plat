// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Scheduling
// 文件名称：TaktSampleQuartzJobHandler.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：示例 Quartz 任务处理器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Scheduling;

/// <summary>
/// 示例 Quartz 任务处理器（HandlerKey = sample）
/// </summary>
public class TaktSampleQuartzJobHandler : ITaktQuartzJobHandler
{
    /// <summary>
    /// 处理器键
    /// </summary>
    public string HandlerKey => "sample";

    /// <summary>
    /// 执行示例任务
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        TaktLogger.Information(
            "[Quartz Sample] TaskId={TaskId}, TaskCode={TaskCode}, User={UserName}, Params={Params}",
            context.Task.Id,
            context.Task.TaskCode,
            context.UserName ?? "system",
            context.JobParams ?? string.Empty);
        return Task.CompletedTask;
    }
}
