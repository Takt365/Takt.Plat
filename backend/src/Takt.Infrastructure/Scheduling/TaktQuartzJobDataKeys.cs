// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Scheduling
// 文件名称：TaktQuartzJobDataKeys.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz JobDataMap 键名常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Infrastructure.Scheduling;

/// <summary>
/// Quartz JobDataMap 键名
/// </summary>
public static class TaktQuartzJobDataKeys
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public const string TenantCode = "TenantCode";

    /// <summary>
    /// 公司代码
    /// </summary>
    public const string CompanyCode = "CompanyCode";

    /// <summary>
    /// 定时任务 ID
    /// </summary>
    public const string QuartzTaskId = "QuartzTaskId";

    /// <summary>
    /// 任务处理器类型
    /// </summary>
    public const string JobType = "JobType";

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public const string JobParams = "JobParams";

    /// <summary>
    /// 触发用户
    /// </summary>
    public const string UserName = "UserName";

    /// <summary>
    /// 业务 Job 名称
    /// </summary>
    public const string JobName = "JobName";

    /// <summary>
    /// 业务 Job 分组
    /// </summary>
    public const string JobGroup = "JobGroup";

    /// <summary>
    /// Trigger 名称
    /// </summary>
    public const string TriggerName = "TriggerName";
}
