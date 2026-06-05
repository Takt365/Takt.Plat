// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktCountersign.cs
// 创建时间：2025-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单实体（财务审批业务单，可关联工作流实例）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 会签单实体
/// </summary>
[SugarTable("takt_accounting_financial_countersign", "会签单表")]
[SugarIndex("ix_countersign_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_countersign_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_countersign_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CountersignCode), OrderByType.Asc, true)]
[SugarIndex("ix_countersign_flow_instance", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktCountersign : TaktApprovalEntityBase
{
    /// <summary>
    /// 会签编号
    /// </summary>
    [SugarColumn(ColumnName = "countersign_code", ColumnDescription = "会签编号", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string CountersignCode { get; set; } = string.Empty;
    /// <summary>
    /// 会签部门 JSON
    /// </summary>
    [SugarColumn(ColumnName = "countersign_depts", ColumnDescription = "会签部门", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? CountersignDepts { get; set; }
    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    [SugarColumn(ColumnName = "finance_dept", ColumnDescription = "财务部门", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? FinanceDept { get; set; }
    /// <summary>
    /// 预算审核意见
    /// </summary>
    [SugarColumn(ColumnName = "budget_review_comment", ColumnDescription = "预算审核意见", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? BudgetReviewComment { get; set; }
    /// <summary>
    /// 总经室 JSON
    /// </summary>
    [SugarColumn(ColumnName = "executive_office", ColumnDescription = "总经室", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ExecutiveOffice { get; set; }
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [SugarColumn(ColumnName = "flow_instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? FlowInstanceId { get; set; }
    /// <summary>
    /// 申请人（员工 ID）
    /// </summary>
    [SugarColumn(ColumnName = "applicant_by", ColumnDescription = "申请人", ColumnDataType = "bigint", IsNullable = false)]
    public long ApplicantBy { get; set; }
    /// <summary>
    /// 申请部门
    /// </summary>
    [SugarColumn(ColumnName = "application_dept", ColumnDescription = "申请部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ApplicationDept { get; set; }
    /// <summary>
    /// 经费负担部门
    /// </summary>
    [SugarColumn(ColumnName = "cost_bearer_dept", ColumnDescription = "经费负担部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? CostBearerDept { get; set; }
    /// <summary>
    /// 是否有预算
    /// </summary>
    [SugarColumn(ColumnName = "is_budget", ColumnDescription = "是否有预算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBudget { get; set; }
    /// <summary>
    /// 预算项目
    /// </summary>
    [SugarColumn(ColumnName = "budget_item", ColumnDescription = "预算项目", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BudgetItem { get; set; }
    /// <summary>
    /// 预算金额
    /// </summary>
    [SugarColumn(ColumnName = "budget_amount", ColumnDescription = "预算金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal BudgetAmount { get; set; }
    /// <summary>
    /// 申请金额
    /// </summary>
    [SugarColumn(ColumnName = "application_amount", ColumnDescription = "申请金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ApplicationAmount { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnName = "countersign_title", ColumnDescription = "标题", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? CountersignTitle { get; set; }
    /// <summary>
    /// 申请原因
    /// </summary>
    [SugarColumn(ColumnName = "application_reason", ColumnDescription = "申请原因", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ApplicationReason { get; set; }
    /// <summary>
    /// 预算使用说明
    /// </summary>
    [SugarColumn(ColumnName = "budget_usage_description", ColumnDescription = "预算使用说明", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? BudgetUsageDescription { get; set; }
    /// <summary>
    /// 目标与预期效益
    /// </summary>
    [SugarColumn(ColumnName = "target_and_expected_benefit", ColumnDescription = "目标与预期效益", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? TargetAndExpectedBenefit { get; set; }
    /// <summary>
    /// 附件 JSON
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_status", ColumnDescription = "会签单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CountersignStatus { get; set; }
}
