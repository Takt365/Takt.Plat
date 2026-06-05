// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeResignation.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工离职办理记录实体（人事-离职管理，主子表之子表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工离职办理记录（审批单，状态见 <see cref="TaktApprovalEntityBase.ApprovalStatus"/>）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_resignation", "员工离职表")]
[SugarIndex("ix_employee_resignation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_resignation_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_resignation_approval", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
public class TaktEmployeeResignation : TaktApprovalEntityBase
{
    /// <summary>
    /// 员工ID
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    [SugarColumn(ColumnName = "resignation_type", ColumnDescription = "离职类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ResignationType { get; set; }

    /// <summary>
    /// 申请日期
    /// </summary>
    [SugarColumn(ColumnName = "apply_date", ColumnDescription = "申请日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ApplyDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    [SugarColumn(ColumnName = "last_work_date", ColumnDescription = "最后工作日", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 实际离职日期
    /// </summary>
    [SugarColumn(ColumnName = "termination_date", ColumnDescription = "实际离职日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "离职原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }

    /// <summary>
    /// 工作交接说明
    /// </summary>
    [SugarColumn(ColumnName = "handover_notes", ColumnDescription = "工作交接说明", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? HandoverNotes { get; set; }
}
