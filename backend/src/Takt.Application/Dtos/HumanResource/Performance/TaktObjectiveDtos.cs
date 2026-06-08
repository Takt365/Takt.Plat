// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktObjectiveDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：Objective 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktObjective 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Performance;

// ========================================
// Objective 响应 DTO
// ========================================

/// <summary>
/// 员工绩效目标
/// 对应前端 TaktObjectiveDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktObjectiveDto : TaktApprovalDtoBase
{
    /// <summary>
    /// ObjectiveID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ObjectiveId { get; set; }

    /// <summary>
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 方案指标 名称（填充字段）
    /// </summary>
    public string? SchemeMetricName { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int ObjectiveStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// Objective 查询 DTO
// ========================================

/// <summary>
/// Objective 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktObjectiveQueryDto : TaktPagedQuery
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string? ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal? TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal? CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal? ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 截止日期（范围查询-开始）
    /// </summary>
    public DateTime? DueDateStart { get; set; }

    /// <summary>
    /// 截止日期（范围查询-结束）
    /// </summary>
    public DateTime? DueDateEnd { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string? AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int? ObjectiveStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（TaktApprovalStatus）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

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
// 创建Objective DTO
// ========================================

/// <summary>
/// 创建Objective DTO
/// </summary>
public class TaktObjectiveCreateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    [Required(ErrorMessage = "员工姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    [Required(ErrorMessage = "目标周期（如 2026-Q1、2026-Annual）不能为空")]
    public string ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    [Required(ErrorMessage = "目标描述不能为空")]
    public string ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    [Required(ErrorMessage = "目标达成说明不能为空")]
    public string AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int ObjectiveStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 更新Objective DTO
// ========================================

/// <summary>
/// 更新Objective DTO
/// 继承 TaktObjectiveCreateDto，添加 ObjectiveId 字段
/// </summary>
public class TaktObjectiveUpdateDto : TaktObjectiveCreateDto
{
    /// <summary>
    /// ObjectiveID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ObjectiveId { get; set; }

}

// ========================================
// Objective 状态 DTO
// ========================================

/// <summary>
/// Objective 状态更新 DTO
/// </summary>
public class TaktObjectiveStatusDto
{
    /// <summary>
    /// ObjectiveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ObjectiveId { get; set; }

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    [Required(ErrorMessage = "业务状态（0=待确认 1=进行中 2=已完成）不能为空")]
    public int ObjectiveStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Objective 导入模板行 DTO
/// </summary>
public class TaktObjectiveTemplateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string? ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string? AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int? ObjectiveStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
/// Objective 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktObjectiveImportDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string? ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string? AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int? ObjectiveStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
/// Objective 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktObjectiveExportDto
{
    /// <summary>
    /// ObjectiveID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ObjectiveId { get; set; }

    /// <summary>
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int ObjectiveStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
