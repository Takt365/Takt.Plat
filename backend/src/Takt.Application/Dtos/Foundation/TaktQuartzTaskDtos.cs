// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktQuartzTaskDtos.cs
// 创建时间：2026-06-07
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
    /// Cron 表达式
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public string? JobParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktQuartzTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public TaktYesNo Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public TaktQuartzMisfirePolicy MisfirePolicy { get; set; }

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
    /// Cron 表达式
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    public string? JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public string? JobParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktQuartzTaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public TaktYesNo? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public TaktQuartzMisfirePolicy? MisfirePolicy { get; set; }

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
    /// Cron 表达式
    /// </summary>
    [Required(ErrorMessage = "Cron 表达式不能为空")]
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    [Required(ErrorMessage = "任务处理器类型（DI 注册键或完整类型名）不能为空")]
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public string? JobParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktQuartzTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public TaktYesNo Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public TaktQuartzMisfirePolicy MisfirePolicy { get; set; }

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
    public TaktQuartzTaskStatus TaskStatus { get; set; }
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
    /// Cron 表达式
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    public string? JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public string? JobParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktQuartzTaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public TaktYesNo? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public TaktQuartzMisfirePolicy? MisfirePolicy { get; set; }

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
    /// Cron 表达式
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    public string? JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public string? JobParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktQuartzTaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public TaktYesNo? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public TaktQuartzMisfirePolicy? MisfirePolicy { get; set; }

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
    /// Cron 表达式
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 任务处理器类型（DI 注册键或完整类型名）
    /// </summary>
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// 任务参数 JSON
    /// </summary>
    public string? JobParams { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktQuartzTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 是否允许并发执行（0=禁止，1=允许）
    /// </summary>
    public TaktYesNo Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略
    /// </summary>
    public TaktQuartzMisfirePolicy MisfirePolicy { get; set; }

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
