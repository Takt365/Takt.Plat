// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeAttachment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeAttachment 生成，请按需审阅）
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
    /// 文件ID（关联文件服务）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    public int AttachmentType { get; set; } = 0;

    /// <summary>
    /// 附件说明
    /// </summary>
    public string? AttachmentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
    /// 文件ID（关联文件服务）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    public int? AttachmentType { get; set; }

    /// <summary>
    /// 附件说明
    /// </summary>
    public string? AttachmentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 文件ID（关联文件服务）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    [Required(ErrorMessage = "文件名称不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    public int AttachmentType { get; set; } = 0;

    /// <summary>
    /// 附件说明
    /// </summary>
    public string? AttachmentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
// EmployeeAttachment 排序 DTO
// ========================================

/// <summary>
/// EmployeeAttachment 排序更新 DTO
/// </summary>
public class TaktEmployeeAttachmentSortDto
{
    /// <summary>
    /// EmployeeAttachmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAttachmentId { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
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
    /// 文件ID（关联文件服务）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    public int? AttachmentType { get; set; }

    /// <summary>
    /// 附件说明
    /// </summary>
    public string? AttachmentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 文件ID（关联文件服务）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    public int? AttachmentType { get; set; }

    /// <summary>
    /// 附件说明
    /// </summary>
    public string? AttachmentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
    /// 文件ID（关联文件服务）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    public int AttachmentType { get; set; } = 0;

    /// <summary>
    /// 附件说明
    /// </summary>
    public string? AttachmentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
