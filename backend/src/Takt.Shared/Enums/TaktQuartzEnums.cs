// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktQuartzEnums.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务与执行日志相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 定时任务状态
/// </summary>
public enum TaktQuartzTaskStatus
{
    /// <summary>
    /// 正常（已调度）
    /// </summary>
    [Display(Name = "正常")]
    Normal = 0,

    /// <summary>
    /// 暂停
    /// </summary>
    [Display(Name = "暂停")]
    Paused = 1,
}

/// <summary>
/// 定时任务执行结果
/// </summary>
public enum TaktQuartzExecuteStatus
{
    /// <summary>
    /// 成功
    /// </summary>
    [Display(Name = "成功")]
    Success = 0,

    /// <summary>
    /// 失败
    /// </summary>
    [Display(Name = "失败")]
    Failed = 1,
}

/// <summary>
/// Quartz Misfire 策略
/// </summary>
public enum TaktQuartzMisfirePolicy
{
    /// <summary>
    /// 默认策略
    /// </summary>
    [Display(Name = "默认")]
    Default = 0,

    /// <summary>
    /// 忽略错失触发
    /// </summary>
    [Display(Name = "忽略错失")]
    IgnoreMisfires = 1,

    /// <summary>
    /// 立即触发一次
    /// </summary>
    [Display(Name = "立即触发")]
    FireAndProceed = 2,

    /// <summary>
    /// 不触发，等待下次
    /// </summary>
    [Display(Name = "等待下次")]
    DoNothing = 3,
}
