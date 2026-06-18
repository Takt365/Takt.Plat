// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationReliabilityDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityOperationReliability 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityOperationReliability 生成，请按需审阅）
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
// QualityOperationReliability 响应 DTO
// ========================================

/// <summary>
/// 品质业务明细 - 信赖性评价/ORT费用
/// 对应前端 TaktQualityOperationReliabilityDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityOperationReliabilityDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityOperationReliabilityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationReliabilityId { get; set; }

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
    /// 信赖性评价・ORT业务费用(元)
    /// </summary>
    public decimal TestCost { get; set; }

    /// <summary>
    /// 评价作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 评价其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 信赖性评价备注
    /// </summary>
    public string? ReliabilityNote { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务主表(导航属性)
    /// （主表：TaktQualityOperation）
    /// </summary>
    public TaktQualityOperationDto? Operation { get; set; }

}

// ========================================
// QualityOperationReliability 查询 DTO
// ========================================

/// <summary>
/// QualityOperationReliability 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityOperationReliabilityQueryDto : TaktPagedQuery
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
    /// 信赖性评价・ORT业务费用(元)
    /// </summary>
    public decimal? TestCost { get; set; }

    /// <summary>
    /// 评价作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 评价其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 信赖性评价备注
    /// </summary>
    public string? ReliabilityNote { get; set; } = string.Empty;

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
// 创建QualityOperationReliability DTO
// ========================================

/// <summary>
/// 创建QualityOperationReliability DTO
/// </summary>
public class TaktQualityOperationReliabilityCreateDto
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
    /// 信赖性评价・ORT业务费用(元)
    /// </summary>
    public decimal TestCost { get; set; }

    /// <summary>
    /// 评价作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 评价其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 信赖性评价备注
    /// </summary>
    public string? ReliabilityNote { get; set; } = string.Empty;

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
// 更新QualityOperationReliability DTO
// ========================================

/// <summary>
/// 更新QualityOperationReliability DTO
/// 继承 TaktQualityOperationReliabilityCreateDto，添加 QualityOperationReliabilityId 字段
/// </summary>
public class TaktQualityOperationReliabilityUpdateDto : TaktQualityOperationReliabilityCreateDto
{
    /// <summary>
    /// QualityOperationReliabilityID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationReliabilityId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityOperationReliability 导入模板行 DTO
/// </summary>
public class TaktQualityOperationReliabilityTemplateDto
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
    /// 评价作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 信赖性评价备注
    /// </summary>
    public string? ReliabilityNote { get; set; } = string.Empty;

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
/// QualityOperationReliability 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityOperationReliabilityImportDto
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
    /// 评价作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 信赖性评价备注
    /// </summary>
    public string? ReliabilityNote { get; set; } = string.Empty;

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
/// QualityOperationReliability 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityOperationReliabilityExportDto
{
    /// <summary>
    /// QualityOperationReliabilityID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationReliabilityId { get; set; }

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
    /// 信赖性评价・ORT业务费用(元)
    /// </summary>
    public decimal TestCost { get; set; }

    /// <summary>
    /// 评价作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 评价其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 信赖性评价备注
    /// </summary>
    public string? ReliabilityNote { get; set; } = string.Empty;

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
