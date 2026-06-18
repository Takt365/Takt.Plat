// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationOtherDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityOperationOther 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityOperationOther 生成，请按需审阅）
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
// QualityOperationOther 响应 DTO
// ========================================

/// <summary>
/// 品质业务明细 - 其他通常业务费用
/// 对应前端 TaktQualityOperationOtherDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityOperationOtherDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityOperationOtherID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationOtherId { get; set; }

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
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务主表(导航属性)
    /// （主表：TaktQualityOperation）
    /// </summary>
    public TaktQualityOperationDto? Operation { get; set; }

}

// ========================================
// QualityOperationOther 查询 DTO
// ========================================

/// <summary>
/// QualityOperationOther 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityOperationOtherQueryDto : TaktPagedQuery
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
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal? OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
// 创建QualityOperationOther DTO
// ========================================

/// <summary>
/// 创建QualityOperationOther DTO
/// </summary>
public class TaktQualityOperationOtherCreateDto
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
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
// 更新QualityOperationOther DTO
// ========================================

/// <summary>
/// 更新QualityOperationOther DTO
/// 继承 TaktQualityOperationOtherCreateDto，添加 QualityOperationOtherId 字段
/// </summary>
public class TaktQualityOperationOtherUpdateDto : TaktQualityOperationOtherCreateDto
{
    /// <summary>
    /// QualityOperationOtherID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationOtherId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityOperationOther 导入模板行 DTO
/// </summary>
public class TaktQualityOperationOtherTemplateDto
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
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
/// QualityOperationOther 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityOperationOtherImportDto
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
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
/// QualityOperationOther 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityOperationOtherExportDto
{
    /// <summary>
    /// QualityOperationOtherID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationOtherId { get; set; }

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
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
