// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktQuartzLog.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务执行日志实体（Statistics.Logging 域）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// Quartz 任务执行日志实体
/// </summary>
[SugarTable("takt_statistics_logging_quartz_log", "任务执行日志表")]
[SugarIndex("ix_quartz_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_log_task_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuartzTaskId), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_log_execute_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecuteTime), OrderByType.Desc, false)]
public class TaktQuartzLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联定时任务 ID
    /// </summary>
    [SugarColumn(ColumnName = "quartz_task_id", ColumnDescription = "定时任务ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 触发用户（系统任务为 system）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "触发用户", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? UserName { get; set; }

    /// <summary>
    /// Job 名称
    /// </summary>
    [SugarColumn(ColumnName = "job_name", ColumnDescription = "Job名称", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Job 分组
    /// </summary>
    [SugarColumn(ColumnName = "job_group", ColumnDescription = "Job分组", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// Trigger 名称
    /// </summary>
    [SugarColumn(ColumnName = "trigger_name", ColumnDescription = "Trigger名称", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? TriggerName { get; set; }

    /// <summary>
    /// 执行状态（0=成功，1=失败）
    /// </summary>
    [SugarColumn(ColumnName = "execute_status", ColumnDescription = "执行状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktQuartzExecuteStatus ExecuteStatus { get; set; } = TaktQuartzExecuteStatus.Success;

    /// <summary>
    /// 错误消息
    /// </summary>
    [SugarColumn(ColumnName = "error_msg", ColumnDescription = "错误消息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ErrorMsg { get; set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    [SugarColumn(ColumnName = "execute_time", ColumnDescription = "执行时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ExecuteTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "cost_time", ColumnDescription = "耗时毫秒", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CostTime { get; set; }
}
