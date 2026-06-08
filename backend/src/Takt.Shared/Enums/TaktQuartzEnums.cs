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

/// <summary>
/// 定时任务类型
/// </summary>
public enum TaktQuartzTaskType
{
    /// <summary>
    /// 程序集
    /// </summary>
    [Display(Name = "程序集")]
    Assembly = 1,

    /// <summary>
    /// 网络请求
    /// </summary>
    [Display(Name = "网络请求")]
    HttpRequest = 2,

    /// <summary>
    /// SQL 语句
    /// </summary>
    [Display(Name = "SQL语句")]
    SqlScript = 3,
}

/// <summary>
/// 定时任务触发器类型
/// </summary>
public enum TaktQuartzTriggerType
{
    /// <summary>
    /// Simple（按间隔秒数）
    /// </summary>
    [Display(Name = "Simple")]
    Simple = 0,

    /// <summary>
    /// Cron 表达式
    /// </summary>
    [Display(Name = "Cron")]
    Cron = 1,
}
