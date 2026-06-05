// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktQuartzTask.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务实体（Foundation 域）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// Quartz 定时任务实体
/// </summary>
[SugarTable("takt_foundation_quartz_task", "定时任务表")]
[SugarIndex("ix_quartz_task_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_task_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_task_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TaskCode), OrderByType.Asc, true)]
public class TaktQuartzTask : TaktCompanyEntityBase
{
    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "task_code", ColumnDescription = "任务编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    [SugarColumn(ColumnName = "task_name", ColumnDescription = "任务名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    [SugarColumn(ColumnName = "job_name", ColumnDescription = "Job名称", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    [SugarColumn(ColumnName = "job_group", ColumnDescription = "Job分组", ColumnDataType = "varchar", Length = 50, IsNullable = false, DefaultValue = "DEFAULT")]
    public string JobGroup { get; set; } = "DEFAULT";

    /// <summary>
    /// Cron 表达式
    /// </summary>
    [SugarColumn(ColumnName = "cron_expression", ColumnDescription = "Cron表达式", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    [SugarColumn(ColumnName = "job_type", ColumnDescription = "任务处理器", ColumnDataType = "varchar", Length = 200, IsNullable = false)]
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    [SugarColumn(ColumnName = "job_params", ColumnDescription = "任务参数JSON", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? JobParams { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    [SugarColumn(ColumnName = "task_status", ColumnDescription = "任务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktQuartzTaskStatus TaskStatus { get; set; } = TaktQuartzTaskStatus.Normal;

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    [SugarColumn(ColumnName = "concurrent", ColumnDescription = "是否并发", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo Concurrent { get; set; } = TaktYesNo.No;

    /// <summary>
    /// Misfire 策略
    /// </summary>
    [SugarColumn(ColumnName = "misfire_policy", ColumnDescription = "Misfire策略", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktQuartzMisfirePolicy MisfirePolicy { get; set; } = TaktQuartzMisfirePolicy.Default;

    /// <summary>
    /// 上次执行时间
    /// </summary>
    [SugarColumn(ColumnName = "last_run_at", ColumnDescription = "上次执行时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行时间
    /// </summary>
    [SugarColumn(ColumnName = "next_run_at", ColumnDescription = "下次执行时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "任务描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Description { get; set; }
}
