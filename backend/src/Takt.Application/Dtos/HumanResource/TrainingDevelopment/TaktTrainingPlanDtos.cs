// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingPlanDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingPlan 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTrainingPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.HumanResource.TrainingDevelopment;

// ========================================
// TrainingPlan 响应 DTO
// ========================================

/// <summary>
/// 培训计划（年度/季度/专项）
/// 对应前端 TaktTrainingPlanDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktTrainingPlanDto : TaktApprovalDtoBase
{
    /// <summary>
    /// TrainingPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int PlanYear { get; set; } = 0;

    /// <summary>
    /// 计划类型（年度/季度/月度/专项）
    /// </summary>
    public string PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int PlannedHeadcount { get; set; } = 0;

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    public TaktCommonStatus TrainingPlanStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// TrainingPlan 查询 DTO
// ========================================

/// <summary>
/// TrainingPlan 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTrainingPlanQueryDto : TaktPagedQuery
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
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int? PlanYear { get; set; }

    /// <summary>
    /// 计划类型（年度/季度/月度/专项）
    /// </summary>
    public string? PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 计划开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 计划结束日期（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 计划结束日期（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string? TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int? PlannedHeadcount { get; set; }

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal? TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    public TaktCommonStatus? TrainingPlanStatus { get; set; }

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
// 创建TrainingPlan DTO
// ========================================

/// <summary>
/// 创建TrainingPlan DTO
/// </summary>
public class TaktTrainingPlanCreateDto
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
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "计划编码（租户+公司内唯一）不能为空")]
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    [Required(ErrorMessage = "计划名称不能为空")]
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int PlanYear { get; set; } = 0;

    /// <summary>
    /// 计划类型（年度/季度/月度/专项）
    /// </summary>
    [Required(ErrorMessage = "计划类型（年度/季度/月度/专项）不能为空")]
    public string PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    [Required(ErrorMessage = "适用部门不能为空")]
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    [Required(ErrorMessage = "培训目标不能为空")]
    public string TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int PlannedHeadcount { get; set; } = 0;

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    [Required(ErrorMessage = "计划说明不能为空")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    public TaktCommonStatus TrainingPlanStatus { get; set; }

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
// 更新TrainingPlan DTO
// ========================================

/// <summary>
/// 更新TrainingPlan DTO
/// 继承 TaktTrainingPlanCreateDto，添加 TrainingPlanId 字段
/// </summary>
public class TaktTrainingPlanUpdateDto : TaktTrainingPlanCreateDto
{
    /// <summary>
    /// TrainingPlanID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

}

// ========================================
// TrainingPlan 状态 DTO
// ========================================

/// <summary>
/// TrainingPlan 状态更新 DTO
/// </summary>
public class TaktTrainingPlanStatusDto
{
    /// <summary>
    /// TrainingPlanID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "业务状态（1=启用 0=禁用）不能为空")]
    public TaktCommonStatus TrainingPlanStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TrainingPlan 导入模板行 DTO
/// </summary>
public class TaktTrainingPlanTemplateDto
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
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int? PlanYear { get; set; }

    /// <summary>
    /// 计划类型（年度/季度/月度/专项）
    /// </summary>
    public string? PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 培训目标
    /// </summary>
    public string? TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int? PlannedHeadcount { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    public TaktCommonStatus? TrainingPlanStatus { get; set; }

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
/// TrainingPlan 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTrainingPlanImportDto
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
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int? PlanYear { get; set; }

    /// <summary>
    /// 计划类型（年度/季度/月度/专项）
    /// </summary>
    public string? PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 培训目标
    /// </summary>
    public string? TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int? PlannedHeadcount { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    public TaktCommonStatus? TrainingPlanStatus { get; set; }

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
/// TrainingPlan 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTrainingPlanExportDto
{
    /// <summary>
    /// TrainingPlanID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int PlanYear { get; set; } = 0;

    /// <summary>
    /// 计划类型（年度/季度/月度/专项）
    /// </summary>
    public string PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int PlannedHeadcount { get; set; } = 0;

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（1=启用 0=禁用）
    /// </summary>
    public TaktCommonStatus TrainingPlanStatus { get; set; }

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
