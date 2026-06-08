// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Scheduling
// 文件名称：ITaktQuartzJobHandler.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务处理器接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;

namespace Takt.Infrastructure.Scheduling;

/// <summary>
/// Quartz 任务执行上下文
/// </summary>
public class TaktQuartzJobContext
{
    /// <summary>
    /// 定时任务实体
    /// </summary>
    public TaktQuartzTask Task { get; set; } = null!;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; }

    /// <summary>
    /// 触发用户
    /// </summary>
    public string? UserName { get; set; }
}

/// <summary>
/// Quartz 任务处理器接口
/// </summary>
public interface ITaktQuartzJobHandler
{
    /// <summary>
    /// 处理器键（与 <see cref="TaktQuartzTask.ClassName"/> 或 DI 注册键匹配）
    /// </summary>
    string HandlerKey { get; }

    /// <summary>
    /// 执行任务逻辑
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default);
}
