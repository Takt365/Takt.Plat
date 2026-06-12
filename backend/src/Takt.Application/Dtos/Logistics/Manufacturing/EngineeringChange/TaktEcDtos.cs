// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Ec 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEc 生成，请按需审阅）
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
// Ec 响应 DTO
// ========================================

/// <summary>
/// 设变（ECN）主表实体。FlowInstanceId 存流程实例 Id，由业务方在发起流程后写入；流程引擎不识别本表，BusinessKey/BusinessType 与“设变”的对应由调用方（设变业务模块）约定并实现。联络等文档见附件表 Attachments。
/// 对应前端 TaktEcDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（唯一）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    public int ChangeStatus { get; set; } = 0;

    /// <summary>
    /// 设变主题/标题
    /// </summary>
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变详情/详细说明
    /// </summary>
    public string EcDetailText { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal EcLossAmount { get; set; }

    /// <summary>
    /// 区分/类别 1:全仕向，2：部管，3：内部，4：技术
    /// </summary>
    public string EcDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 流程实例名称（填充字段）
    /// </summary>
    public string? FlowInstanceName { get; set; }

    /// <summary>
    /// 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
    /// </summary>
    public int EcStatus { get; set; } = 0;

    /// <summary>
    /// 设变明细列表
    /// （子表：TaktEcDetail）
    /// </summary>
    public List<TaktEcDetailDto>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（一个设变可对应多个附件）
    /// （子表：TaktEcAttachment）
    /// </summary>
    public List<TaktEcAttachmentDto>? Attachments { get; set; }

}

// ========================================
// Ec 查询 DTO
// ========================================

/// <summary>
/// Ec 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcQueryDto : TaktPagedQuery
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
    /// 设变单号（唯一）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期（范围查询-开始）
    /// </summary>
    public DateTime? EcIssueDateStart { get; set; }

    /// <summary>
    /// 发行日期（范围查询-结束）
    /// </summary>
    public DateTime? EcIssueDateEnd { get; set; }

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变主题/标题
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变详情/详细说明
    /// </summary>
    public string? EcDetailText { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal? EcLossAmount { get; set; }

    /// <summary>
    /// 区分/类别 1:全仕向，2：部管，3：内部，4：技术
    /// </summary>
    public string? EcDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 录入日期（范围查询-开始）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期（范围查询-结束）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
    /// </summary>
    public int? EcStatus { get; set; }

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
// 创建Ec DTO
// ========================================

/// <summary>
/// 创建Ec DTO
/// </summary>
public class TaktEcCreateDto
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
    /// 设变单号（唯一）
    /// </summary>
    [Required(ErrorMessage = "设变单号（唯一）不能为空")]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    public int ChangeStatus { get; set; } = 0;

    /// <summary>
    /// 设变主题/标题
    /// </summary>
    [Required(ErrorMessage = "设变主题/标题不能为空")]
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变详情/详细说明
    /// </summary>
    [Required(ErrorMessage = "设变详情/详细说明不能为空")]
    public string EcDetailText { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    [Required(ErrorMessage = "负责人不能为空")]
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal EcLossAmount { get; set; }

    /// <summary>
    /// 区分/类别 1:全仕向，2：部管，3：内部，4：技术
    /// </summary>
    [Required(ErrorMessage = "区分/类别 1:全仕向，2：部管，3：内部，4：技术不能为空")]
    public string EcDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
    /// </summary>
    public int EcStatus { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（子表，级联保存）
    /// </summary>
    public List<TaktEcDetailCreateDto>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（一个设变可对应多个附件）（子表，级联保存）
    /// </summary>
    public List<TaktEcAttachmentCreateDto>? Attachments { get; set; }

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
// 更新Ec DTO
// ========================================

/// <summary>
/// 更新Ec DTO
/// 继承 TaktEcCreateDto，添加 EcId 字段
/// </summary>
public class TaktEcUpdateDto : TaktEcCreateDto
{
    /// <summary>
    /// EcID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

}

// ========================================
// Ec 状态 DTO
// ========================================

/// <summary>
/// Ec 状态更新 DTO
/// </summary>
public class TaktEcStatusDto
{
    /// <summary>
    /// EcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    [Required(ErrorMessage = "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)不能为空")]
    public int ChangeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Ec 导入模板行 DTO
/// </summary>
public class TaktEcTemplateDto
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
    /// 设变单号（唯一）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变主题/标题
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变详情/详细说明
    /// </summary>
    public string? EcDetailText { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 区分/类别 1:全仕向，2：部管，3：内部，4：技术
    /// </summary>
    public string? EcDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例ID（关联工作流）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
    /// </summary>
    public int? EcStatus { get; set; }

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
/// Ec 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcImportDto
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
    /// 设变单号（唯一）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变主题/标题
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变详情/详细说明
    /// </summary>
    public string? EcDetailText { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 区分/类别 1:全仕向，2：部管，3：内部，4：技术
    /// </summary>
    public string? EcDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例ID（关联工作流）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
    /// </summary>
    public int? EcStatus { get; set; }

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
/// Ec 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcExportDto
{
    /// <summary>
    /// EcID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（唯一）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
    /// </summary>
    public int ChangeStatus { get; set; } = 0;

    /// <summary>
    /// 设变主题/标题
    /// </summary>
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变详情/详细说明
    /// </summary>
    public string EcDetailText { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal EcLossAmount { get; set; }

    /// <summary>
    /// 区分/类别 1:全仕向，2：部管，3：内部，4：技术
    /// </summary>
    public string EcDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 流程实例ID（关联工作流）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
    /// </summary>
    public int EcStatus { get; set; } = 0;

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
