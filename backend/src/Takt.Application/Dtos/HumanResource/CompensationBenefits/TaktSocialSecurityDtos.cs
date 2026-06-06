// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.CompensationBenefits
// 文件名称：TaktSocialSecurityDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SocialSecurity 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSocialSecurity 生成，请按需审阅）
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
// SocialSecurity 响应 DTO
// ========================================

/// <summary>
/// 员工社保缴纳记录
/// 对应前端 TaktSocialSecurityDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSocialSecurityDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SocialSecurityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialSecurityId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数
    /// </summary>
    public decimal SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数
    /// </summary>
    public decimal HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int PayStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// SocialSecurity 查询 DTO
// ========================================

/// <summary>
/// SocialSecurity 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSocialSecurityQueryDto : TaktPagedQuery
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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数
    /// </summary>
    public decimal? SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal? PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal? MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal? UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal? InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal? MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数
    /// </summary>
    public decimal? HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal? HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int? PayStatus { get; set; }

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
// 创建SocialSecurity DTO
// ========================================

/// <summary>
/// 创建SocialSecurity DTO
/// </summary>
public class TaktSocialSecurityCreateDto
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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    [Required(ErrorMessage = "缴纳期间（如 2026-06）不能为空")]
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数
    /// </summary>
    public decimal SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数
    /// </summary>
    public decimal HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int PayStatus { get; set; } = 0;

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
// 更新SocialSecurity DTO
// ========================================

/// <summary>
/// 更新SocialSecurity DTO
/// 继承 TaktSocialSecurityCreateDto，添加 SocialSecurityId 字段
/// </summary>
public class TaktSocialSecurityUpdateDto : TaktSocialSecurityCreateDto
{
    /// <summary>
    /// SocialSecurityID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialSecurityId { get; set; }

}

// ========================================
// SocialSecurity 状态 DTO
// ========================================

/// <summary>
/// SocialSecurity 状态更新 DTO
/// </summary>
public class TaktSocialSecurityStatusDto
{
    /// <summary>
    /// SocialSecurityID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialSecurityId { get; set; }

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    [Required(ErrorMessage = "缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）不能为空")]
    public int PayStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SocialSecurity 导入模板行 DTO
/// </summary>
public class TaktSocialSecurityTemplateDto
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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int? PayStatus { get; set; }

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
/// SocialSecurity 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSocialSecurityImportDto
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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int? PayStatus { get; set; }

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
/// SocialSecurity 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSocialSecurityExportDto
{
    /// <summary>
    /// SocialSecurityID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialSecurityId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数
    /// </summary>
    public decimal SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数
    /// </summary>
    public decimal HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int PayStatus { get; set; } = 0;

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
