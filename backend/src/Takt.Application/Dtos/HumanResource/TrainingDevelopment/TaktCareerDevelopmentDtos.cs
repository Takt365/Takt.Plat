// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.TrainingDevelopment
// 文件名称：TaktCareerDevelopmentDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：CareerDevelopment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCareerDevelopment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.TrainingDevelopment;

// ========================================
// CareerDevelopment 响应 DTO
// ========================================

/// <summary>
/// 员工职业发展规划与技能评估
/// 对应前端 TaktCareerDevelopmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCareerDevelopmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CareerDevelopmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CareerDevelopmentId { get; set; }

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
    /// 技能类别
    /// </summary>
    public string SkillCategory { get; set; } = string.Empty;

    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 评估日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 评估方式
    /// </summary>
    public string AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 评估得分
    /// </summary>
    public decimal AssessmentScore { get; set; }

    /// <summary>
    /// 技能等级
    /// </summary>
    public string SkillLevel { get; set; } = string.Empty;

    /// <summary>
    /// 目标岗位
    /// </summary>
    public string TargetPosition { get; set; } = string.Empty;

    /// <summary>
    /// 发展计划
    /// </summary>
    public string DevelopmentPlan { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string ImprovementSuggestions { get; set; } = string.Empty;

    /// <summary>
    /// 下次评估日期
    /// </summary>
    public DateTime NextAssessmentDate { get; set; }

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    public int CareerDevelopmentStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// CareerDevelopment 查询 DTO
// ========================================

/// <summary>
/// CareerDevelopment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCareerDevelopmentQueryDto : TaktPagedQuery
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
    /// 技能类别
    /// </summary>
    public string? SkillCategory { get; set; } = string.Empty;

    /// <summary>
    /// 技能名称
    /// </summary>
    public string? SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 评估日期（范围查询-开始）
    /// </summary>
    public DateTime? AssessmentDateStart { get; set; }

    /// <summary>
    /// 评估日期（范围查询-结束）
    /// </summary>
    public DateTime? AssessmentDateEnd { get; set; }

    /// <summary>
    /// 评估方式
    /// </summary>
    public string? AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 评估得分
    /// </summary>
    public decimal? AssessmentScore { get; set; }

    /// <summary>
    /// 技能等级
    /// </summary>
    public string? SkillLevel { get; set; } = string.Empty;

    /// <summary>
    /// 目标岗位
    /// </summary>
    public string? TargetPosition { get; set; } = string.Empty;

    /// <summary>
    /// 发展计划
    /// </summary>
    public string? DevelopmentPlan { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestions { get; set; } = string.Empty;

    /// <summary>
    /// 下次评估日期（范围查询-开始）
    /// </summary>
    public DateTime? NextAssessmentDateStart { get; set; }

    /// <summary>
    /// 下次评估日期（范围查询-结束）
    /// </summary>
    public DateTime? NextAssessmentDateEnd { get; set; }

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    public int? CareerDevelopmentStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 创建CareerDevelopment DTO
// ========================================

/// <summary>
/// 创建CareerDevelopment DTO
/// </summary>
public class TaktCareerDevelopmentCreateDto
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
    /// 技能类别
    /// </summary>
    [Required(ErrorMessage = "技能类别不能为空")]
    public string SkillCategory { get; set; } = string.Empty;

    /// <summary>
    /// 技能名称
    /// </summary>
    [Required(ErrorMessage = "技能名称不能为空")]
    public string SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 评估日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 评估方式
    /// </summary>
    [Required(ErrorMessage = "评估方式不能为空")]
    public string AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 评估得分
    /// </summary>
    public decimal AssessmentScore { get; set; }

    /// <summary>
    /// 技能等级
    /// </summary>
    [Required(ErrorMessage = "技能等级不能为空")]
    public string SkillLevel { get; set; } = string.Empty;

    /// <summary>
    /// 目标岗位
    /// </summary>
    [Required(ErrorMessage = "目标岗位不能为空")]
    public string TargetPosition { get; set; } = string.Empty;

    /// <summary>
    /// 发展计划
    /// </summary>
    [Required(ErrorMessage = "发展计划不能为空")]
    public string DevelopmentPlan { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    [Required(ErrorMessage = "改进建议不能为空")]
    public string ImprovementSuggestions { get; set; } = string.Empty;

    /// <summary>
    /// 下次评估日期
    /// </summary>
    public DateTime NextAssessmentDate { get; set; }

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    public int CareerDevelopmentStatus { get; set; } = 0;

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
// 更新CareerDevelopment DTO
// ========================================

/// <summary>
/// 更新CareerDevelopment DTO
/// 继承 TaktCareerDevelopmentCreateDto，添加 CareerDevelopmentId 字段
/// </summary>
public class TaktCareerDevelopmentUpdateDto : TaktCareerDevelopmentCreateDto
{
    /// <summary>
    /// CareerDevelopmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CareerDevelopmentId { get; set; }

}

// ========================================
// CareerDevelopment 状态 DTO
// ========================================

/// <summary>
/// CareerDevelopment 状态更新 DTO
/// </summary>
public class TaktCareerDevelopmentStatusDto
{
    /// <summary>
    /// CareerDevelopmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CareerDevelopmentId { get; set; }

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    [Required(ErrorMessage = "状态（1=进行中 0=已归档）不能为空")]
    public int CareerDevelopmentStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CareerDevelopment 导入模板行 DTO
/// </summary>
public class TaktCareerDevelopmentTemplateDto
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
    /// 技能类别
    /// </summary>
    public string? SkillCategory { get; set; } = string.Empty;

    /// <summary>
    /// 技能名称
    /// </summary>
    public string? SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 评估方式
    /// </summary>
    public string? AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级
    /// </summary>
    public string? SkillLevel { get; set; } = string.Empty;

    /// <summary>
    /// 目标岗位
    /// </summary>
    public string? TargetPosition { get; set; } = string.Empty;

    /// <summary>
    /// 发展计划
    /// </summary>
    public string? DevelopmentPlan { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestions { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    public int? CareerDevelopmentStatus { get; set; }

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
/// CareerDevelopment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCareerDevelopmentImportDto
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
    /// 技能类别
    /// </summary>
    public string? SkillCategory { get; set; } = string.Empty;

    /// <summary>
    /// 技能名称
    /// </summary>
    public string? SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 评估方式
    /// </summary>
    public string? AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级
    /// </summary>
    public string? SkillLevel { get; set; } = string.Empty;

    /// <summary>
    /// 目标岗位
    /// </summary>
    public string? TargetPosition { get; set; } = string.Empty;

    /// <summary>
    /// 发展计划
    /// </summary>
    public string? DevelopmentPlan { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestions { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    public int? CareerDevelopmentStatus { get; set; }

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
/// CareerDevelopment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCareerDevelopmentExportDto
{
    /// <summary>
    /// CareerDevelopmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CareerDevelopmentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

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
    /// 技能类别
    /// </summary>
    public string SkillCategory { get; set; } = string.Empty;

    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 评估日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 评估方式
    /// </summary>
    public string AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 评估得分
    /// </summary>
    public decimal AssessmentScore { get; set; }

    /// <summary>
    /// 技能等级
    /// </summary>
    public string SkillLevel { get; set; } = string.Empty;

    /// <summary>
    /// 目标岗位
    /// </summary>
    public string TargetPosition { get; set; } = string.Empty;

    /// <summary>
    /// 发展计划
    /// </summary>
    public string DevelopmentPlan { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string ImprovementSuggestions { get; set; } = string.Empty;

    /// <summary>
    /// 下次评估日期
    /// </summary>
    public DateTime NextAssessmentDate { get; set; }

    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    public int CareerDevelopmentStatus { get; set; } = 0;

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
