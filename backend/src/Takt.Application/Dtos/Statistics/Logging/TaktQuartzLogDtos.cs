// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktQuartzLogDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：QuartzLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQuartzLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Dtos.Statistics.Logging;

// ========================================
// QuartzLog 响应 DTO
// ========================================

/// <summary>
/// Quartz 任务执行日志实体
/// 对应前端 TaktQuartzLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQuartzLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QuartzLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzLogId { get; set; }

    /// <summary>
    /// 关联定时任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 关联定时任务 名称（填充字段）
    /// </summary>
    public string? QuartzTaskName { get; set; }

    /// <summary>
    /// 任务名称（执行时快照）
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务组名（执行时快照）
    /// </summary>
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public TaktQuartzTaskType TaskType { get; set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public DateTime ExecuteTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 执行消息
    /// </summary>
    public string? ExecuteMessage { get; set; } = string.Empty;

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorInfo { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器 IP
    /// </summary>
    public string? ExecuteIp { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器名
    /// </summary>
    public string? ExecuteHost { get; set; } = string.Empty;

    /// <summary>
    /// 执行状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus ExecuteStatus { get; set; }

    /// <summary>
    /// 关联的定时任务
    /// （主表：TaktQuartzTask）
    /// </summary>
    public TaktQuartzTaskDto? QuartzTask { get; set; }

}

// ========================================
// QuartzLog 查询 DTO
// ========================================

/// <summary>
/// QuartzLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQuartzLogQueryDto : TaktPagedQuery
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
    /// 关联定时任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 任务名称（执行时快照）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务组名（执行时快照）
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public TaktQuartzTaskType? TaskType { get; set; }

    /// <summary>
    /// 执行时间（范围查询-开始）
    /// </summary>
    public DateTime? ExecuteTimeStart { get; set; }

    /// <summary>
    /// 执行时间（范围查询-结束）
    /// </summary>
    public DateTime? ExecuteTimeEnd { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecuteDuration { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 执行消息
    /// </summary>
    public string? ExecuteMessage { get; set; } = string.Empty;

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorInfo { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器 IP
    /// </summary>
    public string? ExecuteIp { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器名
    /// </summary>
    public string? ExecuteHost { get; set; } = string.Empty;

    /// <summary>
    /// 执行状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus? ExecuteStatus { get; set; }

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
// 创建QuartzLog DTO
// ========================================

/// <summary>
/// 创建QuartzLog DTO
/// </summary>
public class TaktQuartzLogCreateDto
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
    /// 关联定时任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务名称（执行时快照）
    /// </summary>
    [Required(ErrorMessage = "任务名称（执行时快照）不能为空")]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务组名（执行时快照）
    /// </summary>
    [Required(ErrorMessage = "任务组名（执行时快照）不能为空")]
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public TaktQuartzTaskType TaskType { get; set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public DateTime ExecuteTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 执行消息
    /// </summary>
    public string? ExecuteMessage { get; set; } = string.Empty;

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorInfo { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器 IP
    /// </summary>
    public string? ExecuteIp { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器名
    /// </summary>
    public string? ExecuteHost { get; set; } = string.Empty;

    /// <summary>
    /// 执行状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus ExecuteStatus { get; set; }

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
// 更新QuartzLog DTO
// ========================================

/// <summary>
/// 更新QuartzLog DTO
/// 继承 TaktQuartzLogCreateDto，添加 QuartzLogId 字段
/// </summary>
public class TaktQuartzLogUpdateDto : TaktQuartzLogCreateDto
{
    /// <summary>
    /// QuartzLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzLogId { get; set; }

}

// ========================================
// QuartzLog 状态 DTO
// ========================================

/// <summary>
/// QuartzLog 状态更新 DTO
/// </summary>
public class TaktQuartzLogStatusDto
{
    /// <summary>
    /// QuartzLogID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzLogId { get; set; }

    /// <summary>
    /// 执行状态（0=失败，1=成功）
    /// </summary>
    [Required(ErrorMessage = "执行状态（0=失败，1=成功）不能为空")]
    public TaktExecuteStatus ExecuteStatus { get; set; }
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// QuartzLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQuartzLogExportDto
{
    /// <summary>
    /// QuartzLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联定时任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务名称（执行时快照）
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务组名（执行时快照）
    /// </summary>
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（1=程序集 2=网络请求 3=SQL语句）
    /// </summary>
    public TaktQuartzTaskType TaskType { get; set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public DateTime ExecuteTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 执行消息
    /// </summary>
    public string? ExecuteMessage { get; set; } = string.Empty;

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorInfo { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器 IP
    /// </summary>
    public string? ExecuteIp { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器名
    /// </summary>
    public string? ExecuteHost { get; set; } = string.Empty;

    /// <summary>
    /// 执行状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus ExecuteStatus { get; set; }

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
