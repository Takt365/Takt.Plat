// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeSkillDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeSkill 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeSkill 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Personnel;

// ========================================
// EmployeeSkill 响应 DTO
// ========================================

/// <summary>
/// 员工技能与证书
/// 对应前端 TaktEmployeeSkillDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeSkillDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeSkillID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeSkillId { get; set; }

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级（0=入门，1=熟练，2=精通，3=专家）
    /// </summary>
    public int SkillLevel { get; set; } = 0;

    /// <summary>
    /// 证书名称
    /// </summary>
    public string? CertificateName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 取得日期
    /// </summary>
    public DateTime? ObtainedDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

}

// ========================================
// EmployeeSkill 查询 DTO
// ========================================

/// <summary>
/// EmployeeSkill 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeSkillQueryDto : TaktPagedQuery
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
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 技能名称
    /// </summary>
    public string? SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级（0=入门，1=熟练，2=精通，3=专家）
    /// </summary>
    public int? SkillLevel { get; set; }

    /// <summary>
    /// 证书名称
    /// </summary>
    public string? CertificateName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 取得日期（范围查询-开始）
    /// </summary>
    public DateTime? ObtainedDateStart { get; set; }

    /// <summary>
    /// 取得日期（范围查询-结束）
    /// </summary>
    public DateTime? ObtainedDateEnd { get; set; }

    /// <summary>
    /// 到期日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 到期日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

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
// 创建EmployeeSkill DTO
// ========================================

/// <summary>
/// 创建EmployeeSkill DTO
/// </summary>
public class TaktEmployeeSkillCreateDto
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
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 技能名称
    /// </summary>
    [Required(ErrorMessage = "技能名称不能为空")]
    public string SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级（0=入门，1=熟练，2=精通，3=专家）
    /// </summary>
    public int SkillLevel { get; set; } = 0;

    /// <summary>
    /// 证书名称
    /// </summary>
    public string? CertificateName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 取得日期
    /// </summary>
    public DateTime? ObtainedDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
// 更新EmployeeSkill DTO
// ========================================

/// <summary>
/// 更新EmployeeSkill DTO
/// 继承 TaktEmployeeSkillCreateDto，添加 EmployeeSkillId 字段
/// </summary>
public class TaktEmployeeSkillUpdateDto : TaktEmployeeSkillCreateDto
{
    /// <summary>
    /// EmployeeSkillID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeSkillId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeSkill 导入模板行 DTO
/// </summary>
public class TaktEmployeeSkillTemplateDto
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
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 技能名称
    /// </summary>
    public string? SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级（0=入门，1=熟练，2=精通，3=专家）
    /// </summary>
    public int? SkillLevel { get; set; }

    /// <summary>
    /// 证书名称
    /// </summary>
    public string? CertificateName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

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
/// EmployeeSkill 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeSkillImportDto
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
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 技能名称
    /// </summary>
    public string? SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级（0=入门，1=熟练，2=精通，3=专家）
    /// </summary>
    public int? SkillLevel { get; set; }

    /// <summary>
    /// 证书名称
    /// </summary>
    public string? CertificateName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

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
/// EmployeeSkill 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeSkillExportDto
{
    /// <summary>
    /// EmployeeSkillID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeSkillId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName { get; set; } = string.Empty;

    /// <summary>
    /// 技能等级（0=入门，1=熟练，2=精通，3=专家）
    /// </summary>
    public int SkillLevel { get; set; } = 0;

    /// <summary>
    /// 证书名称
    /// </summary>
    public string? CertificateName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编号
    /// </summary>
    public string? CertificateNo { get; set; } = string.Empty;

    /// <summary>
    /// 取得日期
    /// </summary>
    public DateTime? ObtainedDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
