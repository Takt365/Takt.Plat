// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Talent
// 文件名称：TaktTalentRecruitmentPlanDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentRecruitmentPlan 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTalentRecruitmentPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Talent;

// ========================================
// TalentRecruitmentPlan 响应 DTO
// ========================================

/// <summary>
/// 招聘计划（审批单，状态见 TaktApprovalEntityBase.ApprovalStatus）
/// 对应前端 TaktTalentRecruitmentPlanDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktTalentRecruitmentPlanDto : TaktApprovalDtoBase
{
    /// <summary>
    /// TalentRecruitmentPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentRecruitmentPlanId { get; set; }

    /// <summary>
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 用人需求名称（填充字段）
    /// </summary>
    public string? StaffingRequirementName { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    public string PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划招聘开始日期
    /// </summary>
    public DateTime PlanStartDate { get; set; }

    /// <summary>
    /// 计划招聘结束日期
    /// </summary>
    public DateTime? PlanEndDate { get; set; }

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    public int PlanHeadcount { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 用人需求
    /// （主表：TaktTalentStaffingRequirement）
    /// </summary>
    public TaktTalentStaffingRequirementDto? StaffingRequirement { get; set; }

    /// <summary>
    /// 职位发布
    /// （子表：TaktTalentJobPosting）
    /// </summary>
    public List<TaktTalentJobPostingDto>? TalentJobPostings { get; set; }

}

// ========================================
// TalentRecruitmentPlan 查询 DTO
// ========================================

/// <summary>
/// TalentRecruitmentPlan 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTalentRecruitmentPlanQueryDto : TaktPagedQuery
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StaffingRequirementId { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    public string? PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanDateStart { get; set; }

    /// <summary>
    /// 计划制定日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanDateEnd { get; set; }

    /// <summary>
    /// 计划招聘开始日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanStartDateStart { get; set; }

    /// <summary>
    /// 计划招聘开始日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanStartDateEnd { get; set; }

    /// <summary>
    /// 计划招聘结束日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanEndDateStart { get; set; }

    /// <summary>
    /// 计划招聘结束日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanEndDateEnd { get; set; }

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    public int? PlanHeadcount { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（TaktApprovalStatus）
    /// </summary>
    public int? ApprovalStatus { get; set; }

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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建TalentRecruitmentPlan DTO
// ========================================

/// <summary>
/// 创建TalentRecruitmentPlan DTO
/// </summary>
public class TaktTalentRecruitmentPlanCreateDto
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    [Required(ErrorMessage = "计划单号（租户+公司内业务编号）不能为空")]
    public string PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划招聘开始日期
    /// </summary>
    public DateTime PlanStartDate { get; set; }

    /// <summary>
    /// 计划招聘结束日期
    /// </summary>
    public DateTime? PlanEndDate { get; set; }

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    public int PlanHeadcount { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布（子表，级联保存）
    /// </summary>
    public List<TaktTalentJobPostingCreateDto>? TalentJobPostings { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新TalentRecruitmentPlan DTO
// ========================================

/// <summary>
/// 更新TalentRecruitmentPlan DTO
/// 继承 TaktTalentRecruitmentPlanCreateDto，添加 TalentRecruitmentPlanId 字段
/// </summary>
public class TaktTalentRecruitmentPlanUpdateDto : TaktTalentRecruitmentPlanCreateDto
{
    /// <summary>
    /// TalentRecruitmentPlanID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentRecruitmentPlanId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TalentRecruitmentPlan 导入模板行 DTO
/// </summary>
public class TaktTalentRecruitmentPlanTemplateDto
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StaffingRequirementId { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    public string? PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    public int? PlanHeadcount { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// TalentRecruitmentPlan 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTalentRecruitmentPlanImportDto
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StaffingRequirementId { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    public string? PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    public int? PlanHeadcount { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// TalentRecruitmentPlan 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTalentRecruitmentPlanExportDto
{
    /// <summary>
    /// TalentRecruitmentPlanID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentRecruitmentPlanId { get; set; }

    /// <summary>
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    public string PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划招聘开始日期
    /// </summary>
    public DateTime PlanStartDate { get; set; }

    /// <summary>
    /// 计划招聘结束日期
    /// </summary>
    public DateTime? PlanEndDate { get; set; }

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    public int PlanHeadcount { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
