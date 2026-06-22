// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceIncomingDtos.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityAssuranceIncoming 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityAssuranceIncoming 生成，请按需审阅）
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
// QualityAssuranceIncoming 响应 DTO
// ========================================

/// <summary>
/// 品质业务明细 - 来料检验费用
/// 对应前端 TaktQualityAssuranceIncomingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityAssuranceIncomingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityAssuranceIncomingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceIncomingId { get; set; }

    /// <summary>
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务主表名称（填充字段）
    /// </summary>
    public string? QualityAssuranceName { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率(元/分钟)
    /// </summary>
    public decimal DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 来料检验业务费用(元)
    /// </summary>
    public decimal IncomingInspectionCost { get; set; }

    /// <summary>
    /// 检查时间(分钟)
    /// </summary>
    public int InspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费(元)
    /// </summary>
    public decimal TravelCost { get; set; }

    /// <summary>
    /// 检查其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 来料检验备注
    /// </summary>
    public string? IncomingNote { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务主表(导航属性)
    /// （主表：TaktQualityAssurance）
    /// </summary>
    public TaktQualityAssuranceDto? Operation { get; set; }

}

// ========================================
// QualityAssuranceIncoming 查询 DTO
// ========================================

/// <summary>
/// QualityAssuranceIncoming 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityAssuranceIncomingQueryDto : TaktPagedQuery
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
    public long? QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 直接人员费率(元/分钟)
    /// </summary>
    public decimal? DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 来料检验业务费用(元)
    /// </summary>
    public decimal? IncomingInspectionCost { get; set; }

    /// <summary>
    /// 检查时间(分钟)
    /// </summary>
    public int? InspectionTimeMinutes { get; set; }

    /// <summary>
    /// 交通费、旅费(元)
    /// </summary>
    public decimal? TravelCost { get; set; }

    /// <summary>
    /// 检查其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 来料检验备注
    /// </summary>
    public string? IncomingNote { get; set; } = string.Empty;

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
// 创建QualityAssuranceIncoming DTO
// ========================================

/// <summary>
/// 创建QualityAssuranceIncoming DTO
/// </summary>
public class TaktQualityAssuranceIncomingCreateDto
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
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "品质业务编码（冗余字段,便于查询）不能为空")]
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率(元/分钟)
    /// </summary>
    public decimal DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 来料检验业务费用(元)
    /// </summary>
    public decimal IncomingInspectionCost { get; set; }

    /// <summary>
    /// 检查时间(分钟)
    /// </summary>
    public int InspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费(元)
    /// </summary>
    public decimal TravelCost { get; set; }

    /// <summary>
    /// 检查其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 来料检验备注
    /// </summary>
    public string? IncomingNote { get; set; } = string.Empty;

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
// 更新QualityAssuranceIncoming DTO
// ========================================

/// <summary>
/// 更新QualityAssuranceIncoming DTO
/// 继承 TaktQualityAssuranceIncomingCreateDto，添加 QualityAssuranceIncomingId 字段
/// </summary>
public class TaktQualityAssuranceIncomingUpdateDto : TaktQualityAssuranceIncomingCreateDto
{
    /// <summary>
    /// QualityAssuranceIncomingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceIncomingId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityAssuranceIncoming 导入模板行 DTO
/// </summary>
public class TaktQualityAssuranceIncomingTemplateDto
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
    public long? QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检查时间(分钟)
    /// </summary>
    public int? InspectionTimeMinutes { get; set; }

    /// <summary>
    /// 来料检验备注
    /// </summary>
    public string? IncomingNote { get; set; } = string.Empty;

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
/// QualityAssuranceIncoming 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityAssuranceIncomingImportDto
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
    public long? QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检查时间(分钟)
    /// </summary>
    public int? InspectionTimeMinutes { get; set; }

    /// <summary>
    /// 来料检验备注
    /// </summary>
    public string? IncomingNote { get; set; } = string.Empty;

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
/// QualityAssuranceIncoming 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityAssuranceIncomingExportDto
{
    /// <summary>
    /// QualityAssuranceIncomingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceIncomingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率(元/分钟)
    /// </summary>
    public decimal DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 来料检验业务费用(元)
    /// </summary>
    public decimal IncomingInspectionCost { get; set; }

    /// <summary>
    /// 检查时间(分钟)
    /// </summary>
    public int InspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费(元)
    /// </summary>
    public decimal TravelCost { get; set; }

    /// <summary>
    /// 检查其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 来料检验备注
    /// </summary>
    public string? IncomingNote { get; set; } = string.Empty;

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
