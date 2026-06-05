// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Talent
// 文件名称：TaktTalentRecruitmentPlan.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：招聘计划（人才链路第2步：基于用人需求制定招聘计划）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Talent;

/// <summary>
/// 招聘计划（审批单，状态见 <see cref="TaktApprovalEntityBase.ApprovalStatus"/>）
/// </summary>
[SugarTable("takt_human_resource_talent_recruitment_plan", "招聘计划表")]
[SugarIndex("ix_talent_recruitment_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_talent_recruitment_plan_staffing", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StaffingRequirementId), OrderByType.Asc, false)]
[SugarIndex("ix_talent_recruitment_plan_approval", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
public class TaktTalentRecruitmentPlan : TaktApprovalEntityBase
{
    /// <summary>
    /// 用人需求ID
    /// </summary>
    [SugarColumn(ColumnName = "staffing_requirement_id", ColumnDescription = "用人需求ID", ColumnDataType = "bigint", IsNullable = false)]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 计划单号（租户+公司内业务编号）
    /// </summary>
    [SugarColumn(ColumnName = "plan_no", ColumnDescription = "计划单号", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string PlanNo { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_date", ColumnDescription = "计划制定日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划招聘开始日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_start_date", ColumnDescription = "计划招聘开始日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanStartDate { get; set; }

    /// <summary>
    /// 计划招聘结束日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_end_date", ColumnDescription = "计划招聘结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlanEndDate { get; set; }

    /// <summary>
    /// 计划招聘人数
    /// </summary>
    [SugarColumn(ColumnName = "plan_headcount", ColumnDescription = "计划招聘人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlanHeadcount { get; set; } = 1;

    /// <summary>
    /// 计划说明
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "计划说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 用人需求
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(StaffingRequirementId))]
    public TaktTalentStaffingRequirement? StaffingRequirement { get; set; }

    /// <summary>
    /// 职位发布
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktTalentJobPosting.RecruitmentPlanId))]
    public List<TaktTalentJobPosting>? TalentJobPostings { get; set; }
}
