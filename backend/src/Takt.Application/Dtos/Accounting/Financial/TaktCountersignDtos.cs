// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktCountersignDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Countersign 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCountersign 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// Countersign 响应 DTO
// ========================================

/// <summary>
/// 会签单实体
/// 对应前端 TaktCountersignDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktCountersignDto : TaktApprovalDtoBase
{
    /// <summary>
    /// CountersignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

    /// <summary>
    /// 会签编号
    /// </summary>
    public string CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 流程实例 名称（填充字段）
    /// </summary>
    public string? FlowInstanceName { get; set; }

    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请部门
    /// </summary>
    public string? ApplicationDept { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门
    /// </summary>
    public string? CostBearerDept { get; set; } = string.Empty;

    /// <summary>
    /// 是否有预算
    /// </summary>
    public int IsBudget { get; set; } = 0;

    /// <summary>
    /// 预算项目
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
    /// </summary>
    public int CountersignStatus { get; set; } = 0;

}

// ========================================
// Countersign 查询 DTO
// ========================================

/// <summary>
/// Countersign 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCountersignQueryDto : TaktPagedQuery
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
    /// 会签编号
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请部门
    /// </summary>
    public string? ApplicationDept { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门
    /// </summary>
    public string? CostBearerDept { get; set; } = string.Empty;

    /// <summary>
    /// 是否有预算
    /// </summary>
    public int? IsBudget { get; set; }

    /// <summary>
    /// 预算项目
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal? ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
    /// </summary>
    public int? CountersignStatus { get; set; }

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
// 创建Countersign DTO
// ========================================

/// <summary>
/// 创建Countersign DTO
/// </summary>
public class TaktCountersignCreateDto
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
    /// 会签编号
    /// </summary>
    [Required(ErrorMessage = "会签编号不能为空")]
    public string CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请部门
    /// </summary>
    public string? ApplicationDept { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门
    /// </summary>
    public string? CostBearerDept { get; set; } = string.Empty;

    /// <summary>
    /// 是否有预算
    /// </summary>
    public int IsBudget { get; set; } = 0;

    /// <summary>
    /// 预算项目
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
    /// </summary>
    public int CountersignStatus { get; set; } = 0;

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
// 更新Countersign DTO
// ========================================

/// <summary>
/// 更新Countersign DTO
/// 继承 TaktCountersignCreateDto，添加 CountersignId 字段
/// </summary>
public class TaktCountersignUpdateDto : TaktCountersignCreateDto
{
    /// <summary>
    /// CountersignID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

}

// ========================================
// Countersign 状态 DTO
// ========================================

/// <summary>
/// Countersign 状态更新 DTO
/// </summary>
public class TaktCountersignStatusDto
{
    /// <summary>
    /// CountersignID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

    /// <summary>
    /// 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
    /// </summary>
    [Required(ErrorMessage = "会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）不能为空")]
    public int CountersignStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Countersign 导入模板行 DTO
/// </summary>
public class TaktCountersignTemplateDto
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
    /// 会签编号
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请部门
    /// </summary>
    public string? ApplicationDept { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门
    /// </summary>
    public string? CostBearerDept { get; set; } = string.Empty;

    /// <summary>
    /// 是否有预算
    /// </summary>
    public int? IsBudget { get; set; }

    /// <summary>
    /// 预算项目
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

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
/// Countersign 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCountersignImportDto
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
    /// 会签编号
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请部门
    /// </summary>
    public string? ApplicationDept { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门
    /// </summary>
    public string? CostBearerDept { get; set; } = string.Empty;

    /// <summary>
    /// 是否有预算
    /// </summary>
    public int? IsBudget { get; set; }

    /// <summary>
    /// 预算项目
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

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
/// Countersign 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCountersignExportDto
{
    /// <summary>
    /// CountersignID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

    /// <summary>
    /// 会签编号
    /// </summary>
    public string CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请部门
    /// </summary>
    public string? ApplicationDept { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门
    /// </summary>
    public string? CostBearerDept { get; set; } = string.Empty;

    /// <summary>
    /// 是否有预算
    /// </summary>
    public int IsBudget { get; set; } = 0;

    /// <summary>
    /// 预算项目
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
    /// </summary>
    public int CountersignStatus { get; set; } = 0;

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
