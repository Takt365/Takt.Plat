// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：EcNotification 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcNotification 生成，请按需审阅）
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
// EcNotification 响应 DTO
// ========================================

/// <summary>
/// 工程变更通知单（技术阶段一 ④，隶属 TaktEcGijutsu）。技术完成 ①主表 ②附件 ③明细 保存后由 TaktEcGijutsuService 自动生成并派发； 各部门确认后在 TaktEcExec* 执行，技术通过看板/批次监控。FlowInstanceId 由通知审批流程写入（可选）。
/// 对应前端 TaktEcNotificationDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktEcNotificationDto : TaktApprovalDtoBase
{
    /// <summary>
    /// EcNotificationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 关联的设变主表名称（填充字段）
    /// </summary>
    public string? EcName { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNotificationDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNotificationDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNotificationNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int EcNotificationMethod { get; set; } = 0;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int EcNotificationStatus { get; set; } = 0;

    /// <summary>
    /// 关联的设变主表
    /// （主表：TaktEcGijutsu）
    /// </summary>
    public TaktEcGijutsuDto? EcGijutsu { get; set; }

}

// ========================================
// EcNotification 查询 DTO
// ========================================

/// <summary>
/// EcNotification 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcNotificationQueryDto : TaktPagedQuery
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
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string? EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期（范围查询-开始）
    /// </summary>
    public DateTime? EcNotificationDateStart { get; set; }

    /// <summary>
    /// 通知日期（范围查询-结束）
    /// </summary>
    public DateTime? EcNotificationDateEnd { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNotificationDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNotificationDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNotificationNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int? EcNotificationMethod { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int? EcNotificationStatus { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建EcNotification DTO
// ========================================

/// <summary>
/// 创建EcNotification DTO
/// </summary>
public class TaktEcNotificationCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    [Required(ErrorMessage = "通知单号（唯一，如：EC-2026-0001）不能为空")]
    public string EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余字段，便于查询）不能为空")]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNotificationDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNotificationDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNotificationNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int EcNotificationMethod { get; set; } = 0;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int EcNotificationStatus { get; set; } = 0;

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
// 更新EcNotification DTO
// ========================================

/// <summary>
/// 更新EcNotification DTO
/// 继承 TaktEcNotificationCreateDto，添加 EcNotificationId 字段
/// </summary>
public class TaktEcNotificationUpdateDto : TaktEcNotificationCreateDto
{
    /// <summary>
    /// EcNotificationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

}

// ========================================
// EcNotification 状态 DTO
// ========================================

/// <summary>
/// EcNotification 状态更新 DTO
/// </summary>
public class TaktEcNotificationStatusDto
{
    /// <summary>
    /// EcNotificationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    [Required(ErrorMessage = "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）不能为空")]
    public int EcNotificationStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcNotification 导入模板行 DTO
/// </summary>
public class TaktEcNotificationTemplateDto
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
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string? EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime? EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNotificationDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNotificationDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNotificationNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int? EcNotificationMethod { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int? EcNotificationStatus { get; set; }

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
/// EcNotification 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcNotificationImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string? EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime? EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNotificationDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNotificationDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNotificationNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int? EcNotificationMethod { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int? EcNotificationStatus { get; set; }

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
/// EcNotification 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcNotificationExportDto
{
    /// <summary>
    /// EcNotificationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNotificationDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNotificationDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNotificationNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int EcNotificationMethod { get; set; } = 0;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int EcNotificationStatus { get; set; } = 0;

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
