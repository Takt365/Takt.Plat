// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirement.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：用人需求（人才链路第1步；字段对齐业务 ReqNo/DeptID/PositionID 等清单）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Domain.Entities.HumanResource.Talent;

/// <summary>
/// 用人需求（审批单；状态见 <see cref="TaktApprovalEntityBase.ApprovalStatus"/>）
/// </summary>
[SugarTable("takt_human_resource_talent_staffing_requirement", "用人需求表")]
[SugarIndex("ix_talent_staffing_requirement_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_talent_staffing_requirement_req_no_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ReqNo), OrderByType.Asc, true)]
[SugarIndex("ix_talent_staffing_requirement_approval", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
[SugarIndex("ix_talent_staffing_requirement_dept", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, false)]
public class TaktTalentStaffingRequirement : TaktApprovalEntityBase
{
    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    [SugarColumn(ColumnName = "req_no", ColumnDescription = "需求单号", ColumnDataType = "varchar", Length = 30, IsNullable = false)]
    public string ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "申请部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long DeptId { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [SugarColumn(ColumnName = "post_id", ColumnDescription = "申请岗位ID", ColumnDataType = "bigint", IsNullable = false)]
    public long PostId { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    [SugarColumn(ColumnName = "job_grade", ColumnDescription = "职级", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? JobGrade { get; set; }

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "request_qty", ColumnDescription = "需求人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RequestQty { get; set; } = 1;

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    [SugarColumn(ColumnName = "headcount_type", ColumnDescription = "编制类型", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "formal")]
    public string HeadcountType { get; set; } = "formal";

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    [SugarColumn(ColumnName = "reason_code", ColumnDescription = "需求原因", ColumnDataType = "varchar", Length = 30, IsNullable = false)]
    public string ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [SugarColumn(ColumnName = "replace_employee_id", ColumnDescription = "替补员工ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）
    /// </summary>
    [SugarColumn(ColumnName = "expected_onboard_date", ColumnDescription = "期望入职日", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpectedOnboardDate { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    [SugarColumn(ColumnName = "contract_type", ColumnDescription = "合同类型", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? ContractType { get; set; }

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    [SugarColumn(ColumnName = "work_location", ColumnDescription = "工作地点", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? WorkLocation { get; set; }

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    [SugarColumn(ColumnName = "job_desc", ColumnDescription = "岗位职责", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? JobDesc { get; set; }

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    [SugarColumn(ColumnName = "qualification", ColumnDescription = "任职要求", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? Qualification { get; set; }

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    [SugarColumn(ColumnName = "budget_year", ColumnDescription = "预算年度", ColumnDataType = "char", Length = 4, IsNullable = true)]
    public string? BudgetYear { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 申请部门
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DeptId))]
    public TaktDept? Dept { get; set; }

    /// <summary>
    /// 申请岗位
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PostId))]
    public TaktPost? Post { get; set; }

    /// <summary>
    /// 替补员工
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ReplaceEmployeeId))]
    public TaktEmployee? ReplaceEmployee { get; set; }

    /// <summary>
    /// 职位发布
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktTalentJobPosting.StaffingRequirementId))]
    public List<TaktTalentJobPosting>? TalentJobPostings { get; set; }
}
