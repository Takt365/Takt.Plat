// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingResultDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingResult 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTrainingResult 生成，请按需审阅）
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
// TrainingResult 响应 DTO
// ========================================

/// <summary>
/// 员工培训结果记录
/// 对应前端 TaktTrainingResultDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTrainingResultDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TrainingResultID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingResultId { get; set; }

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
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程 名称（填充字段）
    /// </summary>
    public string? TrainingCourseName { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int IsPassed { get; set; } = 0;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public TaktCommonStatus TrainingResultStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// TrainingResult 查询 DTO
// ========================================

/// <summary>
/// TrainingResult 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTrainingResultQueryDto : TaktPagedQuery
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
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string? TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string? Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期（范围查询-开始）
    /// </summary>
    public DateTime? TrainingStartDateStart { get; set; }

    /// <summary>
    /// 培训开始日期（范围查询-结束）
    /// </summary>
    public DateTime? TrainingStartDateEnd { get; set; }

    /// <summary>
    /// 培训结束日期（范围查询-开始）
    /// </summary>
    public DateTime? TrainingEndDateStart { get; set; }

    /// <summary>
    /// 培训结束日期（范围查询-结束）
    /// </summary>
    public DateTime? TrainingEndDateEnd { get; set; }

    /// <summary>
    /// 培训日期（范围查询-开始）
    /// </summary>
    public DateTime? TrainingDateStart { get; set; }

    /// <summary>
    /// 培训日期（范围查询-结束）
    /// </summary>
    public DateTime? TrainingDateEnd { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal? TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int? IsPassed { get; set; }

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string? TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public TaktCommonStatus? TrainingResultStatus { get; set; }

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
// 创建TrainingResult DTO
// ========================================

/// <summary>
/// 创建TrainingResult DTO
/// </summary>
public class TaktTrainingResultCreateDto
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
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    [Required(ErrorMessage = "培训课程名称不能为空")]
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    [Required(ErrorMessage = "培训类型不能为空")]
    public string TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    [Required(ErrorMessage = "培训讲师不能为空")]
    public string Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int IsPassed { get; set; } = 0;

    /// <summary>
    /// 证书编号
    /// </summary>
    [Required(ErrorMessage = "证书编号不能为空")]
    public string CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    [Required(ErrorMessage = "培训评价不能为空")]
    public string TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public TaktCommonStatus TrainingResultStatus { get; set; }

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
// 更新TrainingResult DTO
// ========================================

/// <summary>
/// 更新TrainingResult DTO
/// 继承 TaktTrainingResultCreateDto，添加 TrainingResultId 字段
/// </summary>
public class TaktTrainingResultUpdateDto : TaktTrainingResultCreateDto
{
    /// <summary>
    /// TrainingResultID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingResultId { get; set; }

}

// ========================================
// TrainingResult 状态 DTO
// ========================================

/// <summary>
/// TrainingResult 状态更新 DTO
/// </summary>
public class TaktTrainingResultStatusDto
{
    /// <summary>
    /// TrainingResultID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingResultId { get; set; }

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    [Required(ErrorMessage = "状态（1=有效 0=无效）不能为空")]
    public TaktCommonStatus TrainingResultStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TrainingResult 导入模板行 DTO
/// </summary>
public class TaktTrainingResultTemplateDto
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
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string? TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string? Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int? IsPassed { get; set; }

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string? TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public TaktCommonStatus? TrainingResultStatus { get; set; }

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
/// TrainingResult 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTrainingResultImportDto
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
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string? TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string? Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int? IsPassed { get; set; }

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string? TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public TaktCommonStatus? TrainingResultStatus { get; set; }

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
/// TrainingResult 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTrainingResultExportDto
{
    /// <summary>
    /// TrainingResultID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingResultId { get; set; }

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
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int IsPassed { get; set; } = 0;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public TaktCommonStatus TrainingResultStatus { get; set; }

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
