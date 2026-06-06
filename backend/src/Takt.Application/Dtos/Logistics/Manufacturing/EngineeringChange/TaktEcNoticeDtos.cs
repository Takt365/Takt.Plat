// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNoticeDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EcNotice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcNotice 生成，请按需审阅）
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
// EcNotice 响应 DTO
// ========================================

/// <summary>
/// 工程变更通知单实体（EC Notice），用于将设变（ECN）通知到相关部门和人员，追踪通知状态和反馈
/// 对应前端 TaktEcNoticeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcNoticeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcNoticeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNoticeId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime EcNoticeDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNoticeDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNoticeDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNoticeNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int EcNoticeMethod { get; set; } = 0;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int EcNoticeStatus { get; set; } = 0;

    /// <summary>
    /// 确认人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeConfirmerId { get; set; }

    /// <summary>
    /// 确认人姓名
    /// </summary>
    public string? EcNoticeConfirmerName { get; set; } = string.Empty;

    /// <summary>
    /// 确认日期
    /// </summary>
    public DateTime? EcNoticeConfirmDate { get; set; }

    /// <summary>
    /// 确认意见/反馈
    /// </summary>
    public string? EcNoticeConfirmComment { get; set; } = string.Empty;

    /// <summary>
    /// 要求反馈截止日期
    /// </summary>
    public DateTime? EcNoticeRequireFeedbackDate { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 流程实例名称（填充字段）
    /// </summary>
    public string? FlowInstanceName { get; set; }

    /// <summary>
    /// 关联的设变主表
    /// （主表：TaktEc）
    /// </summary>
    public TaktEcDto? Ec { get; set; }

}

// ========================================
// EcNotice 查询 DTO
// ========================================

/// <summary>
/// EcNotice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcNoticeQueryDto : TaktPagedQuery
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
    public string? EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期（范围查询-开始）
    /// </summary>
    public DateTime? EcNoticeDateStart { get; set; }

    /// <summary>
    /// 通知日期（范围查询-结束）
    /// </summary>
    public DateTime? EcNoticeDateEnd { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNoticeDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNoticeDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNoticeNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int? EcNoticeMethod { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int? EcNoticeStatus { get; set; }

    /// <summary>
    /// 确认人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeConfirmerId { get; set; }

    /// <summary>
    /// 确认人姓名
    /// </summary>
    public string? EcNoticeConfirmerName { get; set; } = string.Empty;

    /// <summary>
    /// 确认日期（范围查询-开始）
    /// </summary>
    public DateTime? EcNoticeConfirmDateStart { get; set; }

    /// <summary>
    /// 确认日期（范围查询-结束）
    /// </summary>
    public DateTime? EcNoticeConfirmDateEnd { get; set; }

    /// <summary>
    /// 确认意见/反馈
    /// </summary>
    public string? EcNoticeConfirmComment { get; set; } = string.Empty;

    /// <summary>
    /// 要求反馈截止日期（范围查询-开始）
    /// </summary>
    public DateTime? EcNoticeRequireFeedbackDateStart { get; set; }

    /// <summary>
    /// 要求反馈截止日期（范围查询-结束）
    /// </summary>
    public DateTime? EcNoticeRequireFeedbackDateEnd { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流，序列化为string以避免Javascript精度问题）
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
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建EcNotice DTO
// ========================================

/// <summary>
/// 创建EcNotice DTO
/// </summary>
public class TaktEcNoticeCreateDto
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
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    [Required(ErrorMessage = "通知单号（唯一，如：EC-2026-0001）不能为空")]
    public string EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime EcNoticeDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNoticeDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNoticeDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNoticeNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int EcNoticeMethod { get; set; } = 0;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int EcNoticeStatus { get; set; } = 0;

    /// <summary>
    /// 确认人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeConfirmerId { get; set; }

    /// <summary>
    /// 确认人姓名
    /// </summary>
    public string? EcNoticeConfirmerName { get; set; } = string.Empty;

    /// <summary>
    /// 确认日期
    /// </summary>
    public DateTime? EcNoticeConfirmDate { get; set; }

    /// <summary>
    /// 确认意见/反馈
    /// </summary>
    public string? EcNoticeConfirmComment { get; set; } = string.Empty;

    /// <summary>
    /// 要求反馈截止日期
    /// </summary>
    public DateTime? EcNoticeRequireFeedbackDate { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

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
// 更新EcNotice DTO
// ========================================

/// <summary>
/// 更新EcNotice DTO
/// 继承 TaktEcNoticeCreateDto，添加 EcNoticeId 字段
/// </summary>
public class TaktEcNoticeUpdateDto : TaktEcNoticeCreateDto
{
    /// <summary>
    /// EcNoticeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNoticeId { get; set; }

}

// ========================================
// EcNotice 状态 DTO
// ========================================

/// <summary>
/// EcNotice 状态更新 DTO
/// </summary>
public class TaktEcNoticeStatusDto
{
    /// <summary>
    /// EcNoticeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNoticeId { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    [Required(ErrorMessage = "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）不能为空")]
    public int EcNoticeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcNotice 导入模板行 DTO
/// </summary>
public class TaktEcNoticeTemplateDto
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
    public string? EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNoticeDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNoticeDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNoticeNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int? EcNoticeMethod { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int? EcNoticeStatus { get; set; }

    /// <summary>
    /// 确认人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeConfirmerId { get; set; }

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
/// EcNotice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcNoticeImportDto
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
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string? EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNoticeDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNoticeDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNoticeNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int? EcNoticeMethod { get; set; }

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int? EcNoticeStatus { get; set; }

    /// <summary>
    /// 确认人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeConfirmerId { get; set; }

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
/// EcNotice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcNoticeExportDto
{
    /// <summary>
    /// EcNoticeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNoticeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    public string EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 通知日期
    /// </summary>
    public DateTime EcNoticeDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    public string? EcNoticeDeptCodes { get; set; } = string.Empty;

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    public string? EcNoticeDeptNames { get; set; } = string.Empty;

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    public string? EcNoticeNotifierName { get; set; } = string.Empty;

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    public int EcNoticeMethod { get; set; } = 0;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    public int EcNoticeStatus { get; set; } = 0;

    /// <summary>
    /// 确认人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeConfirmerId { get; set; }

    /// <summary>
    /// 确认人姓名
    /// </summary>
    public string? EcNoticeConfirmerName { get; set; } = string.Empty;

    /// <summary>
    /// 确认日期
    /// </summary>
    public DateTime? EcNoticeConfirmDate { get; set; }

    /// <summary>
    /// 确认意见/反馈
    /// </summary>
    public string? EcNoticeConfirmComment { get; set; } = string.Empty;

    /// <summary>
    /// 要求反馈截止日期
    /// </summary>
    public DateTime? EcNoticeRequireFeedbackDate { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

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
