// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktQuartzTaskDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QuartzTask 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQuartzTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// QuartzTask 响应 DTO
// ========================================

/// <summary>
/// Quartz 定时任务实体
/// 对应前端 TaktQuartzTaskDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQuartzTaskDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QuartzTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public int TaskType { get; set; }

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 语句（任务类型为 SQL 时使用）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（0=Simple 1=Cron）
    /// </summary>
    public int TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int IntervalSeconds { get; set; } = 0;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public int TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public int Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public int MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行时间（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecuteCount { get; set; } = 0;

    /// <summary>
    /// 上次执行时间
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行时间
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 关联的任务执行日志列表（主子表关系：QuartzTaskId）
    /// （子表：TaktQuartzLog）
    /// </summary>
    public List<TaktQuartzLogDto>? QuartzLogs { get; set; }

}

// ========================================
// QuartzTask 查询 DTO
// ========================================

/// <summary>
/// QuartzTask 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQuartzTaskQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string? TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string? JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public int? TaskType { get; set; }

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string? ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 语句（任务类型为 SQL 时使用）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（0=Simple 1=Cron）
    /// </summary>
    public int? TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int? IntervalSeconds { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public int? TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public int? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public int? MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行时间（调度生效开始时间）（范围查询-开始）
    /// </summary>
    public DateTime? FirstRunAtStart { get; set; }

    /// <summary>
    /// 首次执行时间（调度生效开始时间）（范围查询-结束）
    /// </summary>
    public DateTime? FirstRunAtEnd { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int? ExecuteCount { get; set; }

    /// <summary>
    /// 上次执行时间（范围查询-开始）
    /// </summary>
    public DateTime? LastRunAtStart { get; set; }

    /// <summary>
    /// 上次执行时间（范围查询-结束）
    /// </summary>
    public DateTime? LastRunAtEnd { get; set; }

    /// <summary>
    /// 下次执行时间（范围查询-开始）
    /// </summary>
    public DateTime? NextRunAtStart { get; set; }

    /// <summary>
    /// 下次执行时间（范围查询-结束）
    /// </summary>
    public DateTime? NextRunAtEnd { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建QuartzTask DTO
// ========================================

/// <summary>
/// 创建QuartzTask DTO
/// </summary>
public class TaktQuartzTaskCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "任务编码（租户+公司内唯一）不能为空")]
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    [Required(ErrorMessage = "任务名称不能为空")]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    [Required(ErrorMessage = "Quartz Job 名称不能为空")]
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    [Required(ErrorMessage = "Quartz Job 分组不能为空")]
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public int TaskType { get; set; }

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    [Required(ErrorMessage = "程序集名称（任务类型为程序集时使用）不能为空")]
    public string AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    [Required(ErrorMessage = "任务类名（任务类型为程序集时使用）不能为空")]
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 语句（任务类型为 SQL 时使用）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（0=Simple 1=Cron）
    /// </summary>
    public int TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    [Required(ErrorMessage = "Cron 表达式（触发器类型为 Cron 时使用）不能为空")]
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int IntervalSeconds { get; set; } = 0;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public int TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public int Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public int MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行时间（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecuteCount { get; set; } = 0;

    /// <summary>
    /// 上次执行时间
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行时间
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 关联的任务执行日志列表（主子表关系：QuartzTaskId）（子表，级联保存）
    /// </summary>
    public List<TaktQuartzLogCreateDto>? QuartzLogs { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新QuartzTask DTO
// ========================================

/// <summary>
/// 更新QuartzTask DTO
/// 继承 TaktQuartzTaskCreateDto，添加 QuartzTaskId 字段
/// </summary>
public class TaktQuartzTaskUpdateDto : TaktQuartzTaskCreateDto
{
    /// <summary>
    /// QuartzTaskID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

}

// ========================================
// QuartzTask 状态 DTO
// ========================================

/// <summary>
/// QuartzTask 状态更新 DTO
/// </summary>
public class TaktQuartzTaskStatusDto
{
    /// <summary>
    /// QuartzTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    [Required(ErrorMessage = "任务状态不能为空")]
    public int TaskStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QuartzTask 导入模板行 DTO
/// </summary>
public class TaktQuartzTaskTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string? TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string? JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public int? TaskType { get; set; }

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string? ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 语句（任务类型为 SQL 时使用）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（0=Simple 1=Cron）
    /// </summary>
    public int? TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// QuartzTask 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQuartzTaskImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string? TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string? JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public int? TaskType { get; set; }

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string? ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 语句（任务类型为 SQL 时使用）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（0=Simple 1=Cron）
    /// </summary>
    public int? TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// QuartzTask 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQuartzTaskExportDto
{
    /// <summary>
    /// QuartzTaskID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组
    /// </summary>
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public int TaskType { get; set; }

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 语句（任务类型为 SQL 时使用）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（0=Simple 1=Cron）
    /// </summary>
    public int TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int IntervalSeconds { get; set; } = 0;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public int TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public int Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public int MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行时间（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecuteCount { get; set; } = 0;

    /// <summary>
    /// 上次执行时间
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行时间
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
