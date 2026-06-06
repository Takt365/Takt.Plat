// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.CompensationBenefits
// 文件名称：TaktSalaryCalcDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SalaryCalc 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalaryCalc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.CompensationBenefits;

// ========================================
// SalaryCalc 响应 DTO
// ========================================

/// <summary>
/// 薪资核算批次
/// 对应前端 TaktSalaryCalcDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalaryCalcDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalaryCalcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryCalcId { get; set; }

    /// <summary>
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    public string CalcCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次名称
    /// </summary>
    public string CalcName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CalcDate { get; set; }

    /// <summary>
    /// 参与核算人数
    /// </summary>
    public int EmployeeCount { get; set; } = 0;

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 实发合计（元）
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    public int CalcStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// SalaryCalc 查询 DTO
// ========================================

/// <summary>
/// SalaryCalc 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalaryCalcQueryDto : TaktPagedQuery
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
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    public string? CalcCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次名称
    /// </summary>
    public string? CalcName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（范围查询-开始）
    /// </summary>
    public DateTime? CalcDateStart { get; set; }

    /// <summary>
    /// 核算日期（范围查询-结束）
    /// </summary>
    public DateTime? CalcDateEnd { get; set; }

    /// <summary>
    /// 参与核算人数
    /// </summary>
    public int? EmployeeCount { get; set; }

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 实发合计（元）
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    public int? CalcStatus { get; set; }

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
// 创建SalaryCalc DTO
// ========================================

/// <summary>
/// 创建SalaryCalc DTO
/// </summary>
public class TaktSalaryCalcCreateDto
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
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "核算批次编码（租户+公司内唯一）不能为空")]
    public string CalcCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次名称
    /// </summary>
    [Required(ErrorMessage = "核算批次名称不能为空")]
    public string CalcName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    [Required(ErrorMessage = "发薪期间（如 2026-06）不能为空")]
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CalcDate { get; set; }

    /// <summary>
    /// 参与核算人数
    /// </summary>
    public int EmployeeCount { get; set; } = 0;

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 实发合计（元）
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    public int CalcStatus { get; set; } = 0;

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
// 更新SalaryCalc DTO
// ========================================

/// <summary>
/// 更新SalaryCalc DTO
/// 继承 TaktSalaryCalcCreateDto，添加 SalaryCalcId 字段
/// </summary>
public class TaktSalaryCalcUpdateDto : TaktSalaryCalcCreateDto
{
    /// <summary>
    /// SalaryCalcID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryCalcId { get; set; }

}

// ========================================
// SalaryCalc 状态 DTO
// ========================================

/// <summary>
/// SalaryCalc 状态更新 DTO
/// </summary>
public class TaktSalaryCalcStatusDto
{
    /// <summary>
    /// SalaryCalcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryCalcId { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    [Required(ErrorMessage = "核算状态（0=草稿 1=核算中 2=已完成 3=已归档）不能为空")]
    public int CalcStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalaryCalc 导入模板行 DTO
/// </summary>
public class TaktSalaryCalcTemplateDto
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
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    public string? CalcCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次名称
    /// </summary>
    public string? CalcName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 参与核算人数
    /// </summary>
    public int? EmployeeCount { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    public int? CalcStatus { get; set; }

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
/// SalaryCalc 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalaryCalcImportDto
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
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    public string? CalcCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次名称
    /// </summary>
    public string? CalcName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 参与核算人数
    /// </summary>
    public int? EmployeeCount { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    public int? CalcStatus { get; set; }

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
/// SalaryCalc 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalaryCalcExportDto
{
    /// <summary>
    /// SalaryCalcID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryCalcId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    public string CalcCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算批次名称
    /// </summary>
    public string CalcName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CalcDate { get; set; }

    /// <summary>
    /// 参与核算人数
    /// </summary>
    public int EmployeeCount { get; set; } = 0;

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 实发合计（元）
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    public int CalcStatus { get; set; } = 0;

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
