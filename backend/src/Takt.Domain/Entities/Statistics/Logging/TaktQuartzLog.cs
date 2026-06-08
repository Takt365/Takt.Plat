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
using Takt.Domain.Entities.Foundation;
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
[SugarIndex("ix_quartz_log_execute_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecuteStatus), OrderByType.Asc, false)]
public class TaktQuartzLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联定时任务 ID
    /// </summary>
    [SugarColumn(ColumnName = "quartz_task_id", ColumnDescription = "定时任务ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务名称（执行时快照）
    /// </summary>
    [SugarColumn(ColumnName = "task_name", ColumnDescription = "任务名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "''")]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务组名（执行时快照）
    /// </summary>
    [SugarColumn(ColumnName = "job_group", ColumnDescription = "任务组名", ColumnDataType = "varchar", Length = 50, IsNullable = false, DefaultValue = "''")]
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    [SugarColumn(ColumnName = "task_type", ColumnDescription = "任务类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktQuartzTaskType TaskType { get; set; } = TaktQuartzTaskType.Assembly;

    /// <summary>
    /// 执行时间
    /// </summary>
    [SugarColumn(ColumnName = "execute_time", ColumnDescription = "执行时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ExecuteTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "execute_duration", ColumnDescription = "执行耗时（毫秒）", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    [SugarColumn(ColumnName = "execute_params", ColumnDescription = "执行参数", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ExecuteParams { get; set; }

    /// <summary>
    /// 执行消息
    /// </summary>
    [SugarColumn(ColumnName = "execute_message", ColumnDescription = "执行消息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ExecuteMessage { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [SugarColumn(ColumnName = "error_info", ColumnDescription = "错误信息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ErrorInfo { get; set; }

    /// <summary>
    /// 执行机器 IP
    /// </summary>
    [SugarColumn(ColumnName = "execute_ip", ColumnDescription = "执行机器IP", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ExecuteIp { get; set; }

    /// <summary>
    /// 执行机器名
    /// </summary>
    [SugarColumn(ColumnName = "execute_host", ColumnDescription = "执行机器名", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ExecuteHost { get; set; }

    /// <summary>
    /// 执行状态（0=失败，1=成功）
    /// </summary>
    [SugarColumn(ColumnName = "execute_status", ColumnDescription = "执行状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktExecuteStatus ExecuteStatus { get; set; } = TaktExecuteStatus.Failed;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 关联的定时任务
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(QuartzTaskId))]
    public TaktQuartzTask? QuartzTask { get; set; }
}
