// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.CompensationBenefits
// 文件名称：TaktPayslip.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资条发放实体，对应菜单 compensation-benefits/payslip
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.CompensationBenefits;

/// <summary>
/// 员工薪资条
/// </summary>
[SugarTable("takt_human_resource_compensation_benefits_payslip", "薪资条表")]
[SugarIndex("ix_payslip_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_payslip_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_payslip_employee_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(PayPeriod), OrderByType.Asc, false)]
public class TaktPayslip : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工 ID
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工姓名
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    [SugarColumn(ColumnName = "pay_period", ColumnDescription = "发薪期间", ColumnDataType = "nvarchar", Length = 16, IsNullable = false)]
    public string PayPeriod { get; set; } = string.Empty;
    /// <summary>
    /// 基本工资
    /// </summary>
    [SugarColumn(ColumnName = "base_salary", ColumnDescription = "基本工资", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal BaseSalary { get; set; }
    /// <summary>
    /// 岗位津贴
    /// </summary>
    [SugarColumn(ColumnName = "position_allowance", ColumnDescription = "岗位津贴", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PositionAllowance { get; set; }
    /// <summary>
    /// 绩效奖金
    /// </summary>
    [SugarColumn(ColumnName = "performance_bonus", ColumnDescription = "绩效奖金", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PerformanceBonus { get; set; }
    /// <summary>
    /// 加班费
    /// </summary>
    [SugarColumn(ColumnName = "overtime_pay", ColumnDescription = "加班费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OvertimePay { get; set; }
    /// <summary>
    /// 补贴合计
    /// </summary>
    [SugarColumn(ColumnName = "allowance_total", ColumnDescription = "补贴合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AllowanceTotal { get; set; }
    /// <summary>
    /// 应发合计
    /// </summary>
    [SugarColumn(ColumnName = "gross_amount", ColumnDescription = "应发合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal GrossAmount { get; set; }
    /// <summary>
    /// 社保扣款
    /// </summary>
    [SugarColumn(ColumnName = "social_security_deduction", ColumnDescription = "社保扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SocialSecurityDeduction { get; set; }
    /// <summary>
    /// 公积金扣款
    /// </summary>
    [SugarColumn(ColumnName = "housing_fund_deduction", ColumnDescription = "公积金扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal HousingFundDeduction { get; set; }
    /// <summary>
    /// 个税扣款
    /// </summary>
    [SugarColumn(ColumnName = "tax_deduction", ColumnDescription = "个税扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxDeduction { get; set; }
    /// <summary>
    /// 其他扣款
    /// </summary>
    [SugarColumn(ColumnName = "other_deduction", ColumnDescription = "其他扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OtherDeduction { get; set; }
    /// <summary>
    /// 实发金额
    /// </summary>
    [SugarColumn(ColumnName = "net_amount", ColumnDescription = "实发金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal NetAmount { get; set; }
    /// <summary>
    /// 发放状态（0=待发放 1=已发放 2=已确认）
    /// </summary>
    [SugarColumn(ColumnName = "issue_status", ColumnDescription = "发放状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IssueStatus { get; set; }
    /// <summary>
    /// 发放日期
    /// </summary>
    [SugarColumn(ColumnName = "issue_date", ColumnDescription = "发放日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? IssueDate { get; set; }
    /// <summary>
    /// 关联工厂
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? RelatedPlant { get; set; }
}
