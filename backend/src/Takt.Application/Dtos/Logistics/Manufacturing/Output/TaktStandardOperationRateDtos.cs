// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktStandardOperationRateDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：StandardOperationRate 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktStandardOperationRate 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// StandardOperationRate 响应 DTO
// ========================================

/// <summary>
/// 标准生产稼动率实体
/// 对应前端 TaktStandardOperationRateDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktStandardOperationRateDto : TaktCompanyDtoBase
{
    /// <summary>
    /// StandardOperationRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationRateId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度
    /// </summary>
    public string FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    public int OperationType { get; set; } = 0;

    /// <summary>
    /// 稼动率（%）
    /// </summary>
    public decimal OperationRate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; } = 0;

}

// ========================================
// StandardOperationRate 查询 DTO
// ========================================

/// <summary>
/// StandardOperationRate 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktStandardOperationRateQueryDto : TaktPagedQuery
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度
    /// </summary>
    public string? FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    public int? OperationType { get; set; }

    /// <summary>
    /// 稼动率（%）
    /// </summary>
    public decimal? OperationRate { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }

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
// 创建StandardOperationRate DTO
// ========================================

/// <summary>
/// 创建StandardOperationRate DTO
/// </summary>
public class TaktStandardOperationRateCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度
    /// </summary>
    [Required(ErrorMessage = "财务年度不能为空")]
    public string FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    public int OperationType { get; set; } = 0;

    /// <summary>
    /// 稼动率（%）
    /// </summary>
    public decimal OperationRate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; } = 0;

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
// 更新StandardOperationRate DTO
// ========================================

/// <summary>
/// 更新StandardOperationRate DTO
/// 继承 TaktStandardOperationRateCreateDto，添加 StandardOperationRateId 字段
/// </summary>
public class TaktStandardOperationRateUpdateDto : TaktStandardOperationRateCreateDto
{
    /// <summary>
    /// StandardOperationRateID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationRateId { get; set; }

}

// ========================================
// StandardOperationRate 状态 DTO
// ========================================

/// <summary>
/// StandardOperationRate 状态更新 DTO
/// </summary>
public class TaktStandardOperationRateStatusDto
{
    /// <summary>
    /// StandardOperationRateID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationRateId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public int Status { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// StandardOperationRate 导入模板行 DTO
/// </summary>
public class TaktStandardOperationRateTemplateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度
    /// </summary>
    public string? FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    public int? OperationType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }

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
/// StandardOperationRate 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktStandardOperationRateImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度
    /// </summary>
    public string? FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    public int? OperationType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }

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
/// StandardOperationRate 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktStandardOperationRateExportDto
{
    /// <summary>
    /// StandardOperationRateID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationRateId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度
    /// </summary>
    public string FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    public int OperationType { get; set; } = 0;

    /// <summary>
    /// 稼动率（%）
    /// </summary>
    public decimal OperationRate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; } = 0;

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
