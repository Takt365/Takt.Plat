// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationFirstArticleDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityOperationFirstArticle 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityOperationFirstArticle 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

// ========================================
// QualityOperationFirstArticle 响应 DTO
// ========================================

/// <summary>
/// 品质业务明细 - 初期检定・定期检定费用
/// 对应前端 TaktQualityOperationFirstArticleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityOperationFirstArticleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityOperationFirstArticleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationFirstArticleId { get; set; }

    /// <summary>
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationId { get; set; }

    /// <summary>
    /// 品质业务主表名称（填充字段）
    /// </summary>
    public string? QualityOperationName { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 初期检定・定期检定业务费用(元)
    /// </summary>
    public decimal QualificationCost { get; set; }

    /// <summary>
    /// 检定作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 检定其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 检定备注
    /// </summary>
    public string? QualificationNote { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务主表(导航属性)
    /// （主表：TaktQualityOperation）
    /// </summary>
    public TaktQualityOperationDto? Operation { get; set; }

}

// ========================================
// QualityOperationFirstArticle 查询 DTO
// ========================================

/// <summary>
/// QualityOperationFirstArticle 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityOperationFirstArticleQueryDto : TaktPagedQuery
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
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityOperationId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 初期检定・定期检定业务费用(元)
    /// </summary>
    public decimal? QualificationCost { get; set; }

    /// <summary>
    /// 检定作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 检定其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 检定备注
    /// </summary>
    public string? QualificationNote { get; set; } = string.Empty;

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
// 创建QualityOperationFirstArticle DTO
// ========================================

/// <summary>
/// 创建QualityOperationFirstArticle DTO
/// </summary>
public class TaktQualityOperationFirstArticleCreateDto
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
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "品质业务编码（冗余字段,便于查询）不能为空")]
    public string QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 初期检定・定期检定业务费用(元)
    /// </summary>
    public decimal QualificationCost { get; set; }

    /// <summary>
    /// 检定作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 检定其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 检定备注
    /// </summary>
    public string? QualificationNote { get; set; } = string.Empty;

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
// 更新QualityOperationFirstArticle DTO
// ========================================

/// <summary>
/// 更新QualityOperationFirstArticle DTO
/// 继承 TaktQualityOperationFirstArticleCreateDto，添加 QualityOperationFirstArticleId 字段
/// </summary>
public class TaktQualityOperationFirstArticleUpdateDto : TaktQualityOperationFirstArticleCreateDto
{
    /// <summary>
    /// QualityOperationFirstArticleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationFirstArticleId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityOperationFirstArticle 导入模板行 DTO
/// </summary>
public class TaktQualityOperationFirstArticleTemplateDto
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
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityOperationId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检定作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 检定备注
    /// </summary>
    public string? QualificationNote { get; set; } = string.Empty;

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
/// QualityOperationFirstArticle 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityOperationFirstArticleImportDto
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
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityOperationId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检定作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 检定备注
    /// </summary>
    public string? QualificationNote { get; set; } = string.Empty;

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
/// QualityOperationFirstArticle 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityOperationFirstArticleExportDto
{
    /// <summary>
    /// QualityOperationFirstArticleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationFirstArticleId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 初期检定・定期检定业务费用(元)
    /// </summary>
    public decimal QualificationCost { get; set; }

    /// <summary>
    /// 检定作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 检定其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 检定备注
    /// </summary>
    public string? QualificationNote { get; set; } = string.Empty;

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
