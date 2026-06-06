// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeTransferDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeTransfer 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeTransfer 生成，请按需审阅）
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
// EmployeeTransfer 响应 DTO
// ========================================

/// <summary>
/// 员工调动记录（审批单，状态见 <see cref="TaktApprovalEntityBase.ApprovalStatus"/>）
/// 对应前端 TaktEmployeeTransferDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktEmployeeTransferDto : TaktApprovalDtoBase
{
    /// <summary>
    /// EmployeeTransferID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeTransferId { get; set; }

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
    /// 调动类型（0=转岗，1=调岗）
    /// </summary>
    public int TransferType { get; set; } = 0;

    /// <summary>
    /// 调出部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromDeptId { get; set; }

    /// <summary>
    /// 调出部门名称
    /// </summary>
    public string FromDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调出岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromPostId { get; set; }

    /// <summary>
    /// 调出岗位名称
    /// </summary>
    public string? FromPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调入部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToDeptId { get; set; }

    /// <summary>
    /// 调入部门名称
    /// </summary>
    public string ToDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调入岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToPostId { get; set; }

    /// <summary>
    /// 调入岗位名称
    /// </summary>
    public string? ToPostName { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 调动原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

}

// ========================================
// EmployeeTransfer 查询 DTO
// ========================================

/// <summary>
/// EmployeeTransfer 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeTransferQueryDto : TaktPagedQuery
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
    /// 调动类型（0=转岗，1=调岗）
    /// </summary>
    public int? TransferType { get; set; }

    /// <summary>
    /// 调出部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromDeptId { get; set; }

    /// <summary>
    /// 调出部门名称
    /// </summary>
    public string? FromDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调出岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromPostId { get; set; }

    /// <summary>
    /// 调出岗位名称
    /// </summary>
    public string? FromPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调入部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToDeptId { get; set; }

    /// <summary>
    /// 调入部门名称
    /// </summary>
    public string? ToDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调入岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToPostId { get; set; }

    /// <summary>
    /// 调入岗位名称
    /// </summary>
    public string? ToPostName { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 调动原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（TaktApprovalStatus）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

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
// 创建EmployeeTransfer DTO
// ========================================

/// <summary>
/// 创建EmployeeTransfer DTO
/// </summary>
public class TaktEmployeeTransferCreateDto
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
    /// 调动类型（0=转岗，1=调岗）
    /// </summary>
    public int TransferType { get; set; } = 0;

    /// <summary>
    /// 调出部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromDeptId { get; set; }

    /// <summary>
    /// 调出部门名称
    /// </summary>
    [Required(ErrorMessage = "调出部门名称不能为空")]
    public string FromDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调出岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromPostId { get; set; }

    /// <summary>
    /// 调出岗位名称
    /// </summary>
    public string? FromPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调入部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToDeptId { get; set; }

    /// <summary>
    /// 调入部门名称
    /// </summary>
    [Required(ErrorMessage = "调入部门名称不能为空")]
    public string ToDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调入岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToPostId { get; set; }

    /// <summary>
    /// 调入岗位名称
    /// </summary>
    public string? ToPostName { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 调动原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
// 更新EmployeeTransfer DTO
// ========================================

/// <summary>
/// 更新EmployeeTransfer DTO
/// 继承 TaktEmployeeTransferCreateDto，添加 EmployeeTransferId 字段
/// </summary>
public class TaktEmployeeTransferUpdateDto : TaktEmployeeTransferCreateDto
{
    /// <summary>
    /// EmployeeTransferID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeTransferId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeTransfer 导入模板行 DTO
/// </summary>
public class TaktEmployeeTransferTemplateDto
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
    /// 调动类型（0=转岗，1=调岗）
    /// </summary>
    public int? TransferType { get; set; }

    /// <summary>
    /// 调出部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromDeptId { get; set; }

    /// <summary>
    /// 调出部门名称
    /// </summary>
    public string? FromDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调出岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromPostId { get; set; }

    /// <summary>
    /// 调出岗位名称
    /// </summary>
    public string? FromPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调入部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToDeptId { get; set; }

    /// <summary>
    /// 调入部门名称
    /// </summary>
    public string? ToDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调入岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToPostId { get; set; }

    /// <summary>
    /// 调入岗位名称
    /// </summary>
    public string? ToPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调动原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
/// EmployeeTransfer 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeTransferImportDto
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
    /// 调动类型（0=转岗，1=调岗）
    /// </summary>
    public int? TransferType { get; set; }

    /// <summary>
    /// 调出部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromDeptId { get; set; }

    /// <summary>
    /// 调出部门名称
    /// </summary>
    public string? FromDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调出岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromPostId { get; set; }

    /// <summary>
    /// 调出岗位名称
    /// </summary>
    public string? FromPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调入部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToDeptId { get; set; }

    /// <summary>
    /// 调入部门名称
    /// </summary>
    public string? ToDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调入岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToPostId { get; set; }

    /// <summary>
    /// 调入岗位名称
    /// </summary>
    public string? ToPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调动原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
/// EmployeeTransfer 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeTransferExportDto
{
    /// <summary>
    /// EmployeeTransferID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeTransferId { get; set; }

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 调动类型（0=转岗，1=调岗）
    /// </summary>
    public int TransferType { get; set; } = 0;

    /// <summary>
    /// 调出部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromDeptId { get; set; }

    /// <summary>
    /// 调出部门名称
    /// </summary>
    public string FromDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调出岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromPostId { get; set; }

    /// <summary>
    /// 调出岗位名称
    /// </summary>
    public string? FromPostName { get; set; } = string.Empty;

    /// <summary>
    /// 调入部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToDeptId { get; set; }

    /// <summary>
    /// 调入部门名称
    /// </summary>
    public string ToDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 调入岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToPostId { get; set; }

    /// <summary>
    /// 调入岗位名称
    /// </summary>
    public string? ToPostName { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 调动原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
