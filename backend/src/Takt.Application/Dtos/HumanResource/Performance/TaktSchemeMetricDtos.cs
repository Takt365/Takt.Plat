// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktSchemeMetricDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SchemeMetric 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSchemeMetric 生成，请按需审阅）
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
// SchemeMetric 响应 DTO
// ========================================

/// <summary>
/// 绩效方案指标（方案维度 + 指标维度合一，每行表示某方案下的一条指标）
/// 对应前端 TaktSchemeMetricDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSchemeMetricDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SchemeMetricID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    public string SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal StandardWeight { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int SchemeMetricStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// SchemeMetric 查询 DTO
// ========================================

/// <summary>
/// SchemeMetric 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSchemeMetricQueryDto : TaktPagedQuery
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
    /// 方案编码
    /// </summary>
    public string? SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal? SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal? SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string? MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string? Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string? MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string? ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal? StandardWeight { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? SchemeMetricStatus { get; set; }

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
// 创建SchemeMetric DTO
// ========================================

/// <summary>
/// 创建SchemeMetric DTO
/// </summary>
public class TaktSchemeMetricCreateDto
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
    /// 方案编码
    /// </summary>
    [Required(ErrorMessage = "方案编码不能为空")]
    public string SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    [Required(ErrorMessage = "方案名称不能为空")]
    public string SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    [Required(ErrorMessage = "适用部门不能为空")]
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    [Required(ErrorMessage = "考核周期类型（月度/季度/半年度/年度）不能为空")]
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    [Required(ErrorMessage = "评分标准（百分制/五分制/等级制）不能为空")]
    public string ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    [Required(ErrorMessage = "指标编码不能为空")]
    public string MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    [Required(ErrorMessage = "指标名称不能为空")]
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    [Required(ErrorMessage = "指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）不能为空")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    [Required(ErrorMessage = "指标类型（定量/定性）不能为空")]
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    [Required(ErrorMessage = "评分标准说明不能为空")]
    public string ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal StandardWeight { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int SchemeMetricStatus { get; set; } = 0;

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
// 更新SchemeMetric DTO
// ========================================

/// <summary>
/// 更新SchemeMetric DTO
/// 继承 TaktSchemeMetricCreateDto，添加 SchemeMetricId 字段
/// </summary>
public class TaktSchemeMetricUpdateDto : TaktSchemeMetricCreateDto
{
    /// <summary>
    /// SchemeMetricID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

}

// ========================================
// SchemeMetric 状态 DTO
// ========================================

/// <summary>
/// SchemeMetric 状态更新 DTO
/// </summary>
public class TaktSchemeMetricStatusDto
{
    /// <summary>
    /// SchemeMetricID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（0=启用 1=停用）不能为空")]
    public int SchemeMetricStatus { get; set; } = 0;
}

// ========================================
// SchemeMetric 排序 DTO
// ========================================

/// <summary>
/// SchemeMetric 排序更新 DTO
/// </summary>
public class TaktSchemeMetricSortDto
{
    /// <summary>
    /// SchemeMetricID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SchemeMetric 导入模板行 DTO
/// </summary>
public class TaktSchemeMetricTemplateDto
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
    /// 方案编码
    /// </summary>
    public string? SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 指标编码
    /// </summary>
    public string? MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string? Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string? MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string? ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? SchemeMetricStatus { get; set; }

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
/// SchemeMetric 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSchemeMetricImportDto
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
    /// 方案编码
    /// </summary>
    public string? SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 指标编码
    /// </summary>
    public string? MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string? Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string? MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string? ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? SchemeMetricStatus { get; set; }

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
/// SchemeMetric 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSchemeMetricExportDto
{
    /// <summary>
    /// SchemeMetricID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案编码
    /// </summary>
    public string SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal StandardWeight { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int SchemeMetricStatus { get; set; } = 0;

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
