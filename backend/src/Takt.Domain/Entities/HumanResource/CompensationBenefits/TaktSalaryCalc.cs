// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.CompensationBenefits
// 文件名称：TaktSalaryCalc.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资核算批次实体，对应菜单 compensation-benefits/salary-calc
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.CompensationBenefits;

/// <summary>
/// 薪资核算批次
/// </summary>
[SugarTable("takt_human_resource_compensation_benefits_salary_calc", "薪资核算表")]
[SugarIndex("ix_salary_calc_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_salary_calc_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_salary_calc_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CalcCode), OrderByType.Asc, true)]
[SugarIndex("ix_salary_calc_pay_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PayPeriod), OrderByType.Asc, false)]
public class TaktSalaryCalc : TaktCompanyEntityBase
{
    /// <summary>
    /// 核算批次编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "calc_code", ColumnDescription = "核算批次编码", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string CalcCode { get; set; } = string.Empty;
    /// <summary>
    /// 核算批次名称
    /// </summary>
    [SugarColumn(ColumnName = "calc_name", ColumnDescription = "核算批次名称", ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
    public string CalcName { get; set; } = string.Empty;
    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    [SugarColumn(ColumnName = "pay_period", ColumnDescription = "发薪期间", ColumnDataType = "nvarchar", Length = 16, IsNullable = false)]
    public string PayPeriod { get; set; } = string.Empty;
    /// <summary>
    /// 核算日期
    /// </summary>
    [SugarColumn(ColumnName = "calc_date", ColumnDescription = "核算日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime CalcDate { get; set; }
    /// <summary>
    /// 参与核算人数
    /// </summary>
    [SugarColumn(ColumnName = "employee_count", ColumnDescription = "参与核算人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EmployeeCount { get; set; }
    /// <summary>
    /// 应发合计（元）
    /// </summary>
    [SugarColumn(ColumnName = "gross_amount", ColumnDescription = "应发合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal GrossAmount { get; set; }
    /// <summary>
    /// 实发合计（元）
    /// </summary>
    [SugarColumn(ColumnName = "net_amount", ColumnDescription = "实发合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal NetAmount { get; set; }
    /// <summary>
    /// 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
    /// </summary>
    [SugarColumn(ColumnName = "calc_status", ColumnDescription = "核算状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CalcStatus { get; set; }
    /// <summary>
    /// 关联工厂
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? RelatedPlant { get; set; }
}
