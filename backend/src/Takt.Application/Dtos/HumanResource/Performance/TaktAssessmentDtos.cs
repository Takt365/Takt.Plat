// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktAssessmentDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Assessment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssessment 生成，请按需审阅）
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
// Assessment 响应 DTO
// ========================================

/// <summary>
/// 员工绩效考核评估
/// 对应前端 TaktAssessmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssessmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssessmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }

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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

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
    /// 自评分数
    /// </summary>
    public decimal SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReviewerId { get; set; }

    /// <summary>
    /// 评审人 名称（填充字段）
    /// </summary>
    public string? ReviewerName { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int AssessmentStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// Assessment 查询 DTO
// ========================================

/// <summary>
/// Assessment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssessmentQueryDto : TaktPagedQuery
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期（范围查询-开始）
    /// </summary>
    public DateTime? AssessmentDateStart { get; set; }

    /// <summary>
    /// 考核日期（范围查询-结束）
    /// </summary>
    public DateTime? AssessmentDateEnd { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal? SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string? SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal? SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string? SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal? FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string? PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期（范围查询-开始）
    /// </summary>
    public DateTime? InterviewDateStart { get; set; }

    /// <summary>
    /// 面谈日期（范围查询-结束）
    /// </summary>
    public DateTime? InterviewDateEnd { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string? InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int? AssessmentStatus { get; set; }

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
// 创建Assessment DTO
// ========================================

/// <summary>
/// 创建Assessment DTO
/// </summary>
public class TaktAssessmentCreateDto
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    [Required(ErrorMessage = "考核周期（如 2026-Q1、2026-Annual）不能为空")]
    public string AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    [Required(ErrorMessage = "自评说明不能为空")]
    public string SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    [Required(ErrorMessage = "主管评语不能为空")]
    public string SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    [Required(ErrorMessage = "绩效等级（A/B/C/D/E）不能为空")]
    public string PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    [Required(ErrorMessage = "面谈记录不能为空")]
    public string InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int AssessmentStatus { get; set; } = 0;

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
// 更新Assessment DTO
// ========================================

/// <summary>
/// 更新Assessment DTO
/// 继承 TaktAssessmentCreateDto，添加 AssessmentId 字段
/// </summary>
public class TaktAssessmentUpdateDto : TaktAssessmentCreateDto
{
    /// <summary>
    /// AssessmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }

}

// ========================================
// Assessment 状态 DTO
// ========================================

/// <summary>
/// Assessment 状态更新 DTO
/// </summary>
public class TaktAssessmentStatusDto
{
    /// <summary>
    /// AssessmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    [Required(ErrorMessage = "状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）不能为空")]
    public int AssessmentStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Assessment 导入模板行 DTO
/// </summary>
public class TaktAssessmentTemplateDto
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string? SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评语
    /// </summary>
    public string? SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string? PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string? InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int? AssessmentStatus { get; set; }

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
/// Assessment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssessmentImportDto
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string? SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评语
    /// </summary>
    public string? SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string? PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string? InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int? AssessmentStatus { get; set; }

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
/// Assessment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssessmentExportDto
{
    /// <summary>
    /// AssessmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }

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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int AssessmentStatus { get; set; } = 0;

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
