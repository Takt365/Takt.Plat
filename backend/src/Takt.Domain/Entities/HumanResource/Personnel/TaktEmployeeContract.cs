// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeContract.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工劳动合同实体（人事-合同管理）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工劳动合同
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_contract", "员工劳动合同表")]
[SugarIndex("ix_employee_contract_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_contract_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_contract_no", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ContractNo), OrderByType.Asc, true)]
public class TaktEmployeeContract : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工ID
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 合同编号
    /// </summary>
    [SugarColumn(ColumnName = "contract_no", ColumnDescription = "合同编号", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string ContractNo { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=固定期限，1=无固定期限，2=以完成一定工作任务为期限，3=实习）
    /// </summary>
    [SugarColumn(ColumnName = "contract_type", ColumnDescription = "合同类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ContractType { get; set; }

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=到期，3=终止）
    /// </summary>
    [SugarColumn(ColumnName = "contract_status", ColumnDescription = "合同状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ContractStatus { get; set; }

    /// <summary>
    /// 合同开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "合同开始日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 合同结束日期
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "合同结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    [SugarColumn(ColumnName = "probation_end_date", ColumnDescription = "试用期结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    [SugarColumn(ColumnName = "sign_date", ColumnDescription = "签订日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    [SugarColumn(ColumnName = "sign_company", ColumnDescription = "签约单位", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? SignCompany { get; set; }
}
