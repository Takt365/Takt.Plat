// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：EcAttachment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcAttachment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcAttachment 响应 DTO
// ========================================

/// <summary>
/// 设变附件实体。文件类别：Liaison/EPP/FPP/ExternalLiaison/TCJ 等；文件编号为联络编号等。
/// 对应前端 TaktEcAttachmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcAttachmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

    /// <summary>
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变主表名称（填充字段）
    /// </summary>
    public string? EcName { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
    /// </summary>
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编号（如联络编号等）
    /// </summary>
    public string DocNo { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表（多对一）
    /// （主表：TaktEc）
    /// </summary>
    public TaktEcDto? Ec { get; set; }

}

// ========================================
// EcAttachment 查询 DTO
// ========================================

/// <summary>
/// EcAttachment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcAttachmentQueryDto : TaktPagedQuery
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
    /// </summary>
    public string? AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编号（如联络编号等）
    /// </summary>
    public string? DocNo { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
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
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建EcAttachment DTO
// ========================================

/// <summary>
/// 创建EcAttachment DTO
/// </summary>
public class TaktEcAttachmentCreateDto
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余字段,便于查询）不能为空")]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
    /// </summary>
    [Required(ErrorMessage = "文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等不能为空")]
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编号（如联络编号等）
    /// </summary>
    [Required(ErrorMessage = "文件编号（如联络编号等）不能为空")]
    public string DocNo { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    [Required(ErrorMessage = "文件名称不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    [Required(ErrorMessage = "访问地址（URL）不能为空")]
    public string AccessUrl { get; set; } = string.Empty;

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
// 更新EcAttachment DTO
// ========================================

/// <summary>
/// 更新EcAttachment DTO
/// 继承 TaktEcAttachmentCreateDto，添加 EcAttachmentId 字段
/// </summary>
public class TaktEcAttachmentUpdateDto : TaktEcAttachmentCreateDto
{
    /// <summary>
    /// EcAttachmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcAttachment 导入模板行 DTO
/// </summary>
public class TaktEcAttachmentTemplateDto
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
    /// </summary>
    public string? AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编号（如联络编号等）
    /// </summary>
    public string? DocNo { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
/// EcAttachment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcAttachmentImportDto
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
    /// </summary>
    public string? AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编号（如联络编号等）
    /// </summary>
    public string? DocNo { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
/// EcAttachment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcAttachmentExportDto
{
    /// <summary>
    /// EcAttachmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
    /// </summary>
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编号（如联络编号等）
    /// </summary>
    public string DocNo { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

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
