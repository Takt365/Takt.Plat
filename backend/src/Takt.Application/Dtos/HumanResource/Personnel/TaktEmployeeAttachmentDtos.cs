// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeAttachment 模块 DTO；文件元数据由 TaktFile 统一管理，本模块仅存 EmployeeId、AttachmentName、AccessUrl。
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
// EmployeeAttachment 响应 DTO
// ========================================

/// <summary>
/// 员工档案附件（主档子表，公司级非审批单）
/// 对应前端 TaktEmployeeAttachmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeAttachmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAttachmentId { get; set; }

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
    /// 附件名称（业务称谓，如毕业证、就业证）
    /// </summary>
    public string AttachmentName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（引用 TaktFile.AccessUrl）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;
}

// ========================================
// EmployeeAttachment 查询 DTO
// ========================================

/// <summary>
/// EmployeeAttachment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeAttachmentQueryDto : TaktPagedQuery
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
    /// 附件名称
    /// </summary>
    public string? AttachmentName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
// 创建EmployeeAttachment DTO
// ========================================

/// <summary>
/// 创建EmployeeAttachment DTO
/// </summary>
public class TaktEmployeeAttachmentCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 附件名称（业务称谓，如毕业证、就业证）
    /// </summary>
    [Required(ErrorMessage = "附件名称不能为空")]
    public string AttachmentName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（引用 TaktFile.AccessUrl）
    /// </summary>
    [Required(ErrorMessage = "访问地址不能为空")]
    public string AccessUrl { get; set; } = string.Empty;

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
// 更新EmployeeAttachment DTO
// ========================================

/// <summary>
/// 更新EmployeeAttachment DTO
/// 继承 TaktEmployeeAttachmentCreateDto，添加 EmployeeAttachmentId 字段
/// </summary>
public class TaktEmployeeAttachmentUpdateDto : TaktEmployeeAttachmentCreateDto
{
    /// <summary>
    /// EmployeeAttachmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAttachmentId { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeAttachment 导入模板行 DTO
/// </summary>
public class TaktEmployeeAttachmentTemplateDto
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
    /// 附件名称
    /// </summary>
    public string? AttachmentName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
/// EmployeeAttachment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeAttachmentImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 附件名称
    /// </summary>
    public string? AttachmentName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
/// EmployeeAttachment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeAttachmentExportDto
{
    /// <summary>
    /// EmployeeAttachmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAttachmentId { get; set; }

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
    /// 附件名称
    /// </summary>
    public string AttachmentName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

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
