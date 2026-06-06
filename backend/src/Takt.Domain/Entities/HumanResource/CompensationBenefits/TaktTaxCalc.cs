// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.CompensationBenefits
// 文件名称：TaktTaxCalc.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：个税计算规则实体，对应菜单 compensation-benefits/tax-calc
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.CompensationBenefits;

/// <summary>
/// 个税计算规则（税率档、扣除标准等）
/// </summary>
[SugarTable("takt_human_resource_compensation_benefits_tax_calc", "个税计算规则表")]
[SugarIndex("ix_tax_calc_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_tax_calc_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_tax_calc_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RuleCode), OrderByType.Asc, true)]
[SugarIndex("ix_tax_calc_tax_year", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TaxYear), OrderByType.Desc, false)]
public class TaktTaxCalc : TaktCompanyEntityBase
{
    /// <summary>
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "rule_code", ColumnDescription = "规则编码", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string RuleCode { get; set; } = string.Empty;
    /// <summary>
    /// 规则名称
    /// </summary>
    [SugarColumn(ColumnName = "rule_name", ColumnDescription = "规则名称", ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
    public string RuleName { get; set; } = string.Empty;
    /// <summary>
    /// 税务年度
    /// </summary>
    [SugarColumn(ColumnName = "tax_year", ColumnDescription = "税务年度", ColumnDataType = "int", IsNullable = false)]
    public int TaxYear { get; set; }
    /// <summary>
    /// 税收起征点
    /// </summary>
    [SugarColumn(ColumnName = "tax_threshold", ColumnDescription = "税收起征点", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxThreshold { get; set; }
    /// <summary>
    /// 应纳税所得额下限
    /// </summary>
    [SugarColumn(ColumnName = "taxable_income_min", ColumnDescription = "应纳税所得额下限", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxableIncomeMin { get; set; }
    /// <summary>
    /// 应纳税所得额上限
    /// </summary>
    [SugarColumn(ColumnName = "taxable_income_max", ColumnDescription = "应纳税所得额上限", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxableIncomeMax { get; set; }
    /// <summary>
    /// 税率（%）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxRate { get; set; }
    /// <summary>
    /// 速算扣除数
    /// </summary>
    [SugarColumn(ColumnName = "quick_deduction", ColumnDescription = "速算扣除数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal QuickDeduction { get; set; }
    /// <summary>
    /// 专项扣除标准
    /// </summary>
    [SugarColumn(ColumnName = "special_deduction_standard", ColumnDescription = "专项扣除标准", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SpecialDeductionStandard { get; set; }
    /// <summary>
    /// 社保扣除比例（%）
    /// </summary>
    [SugarColumn(ColumnName = "social_security_deduction_rate", ColumnDescription = "社保扣除比例", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SocialSecurityDeductionRate { get; set; }
    /// <summary>
    /// 公积金扣除比例（%）
    /// </summary>
    [SugarColumn(ColumnName = "housing_fund_deduction_rate", ColumnDescription = "公积金扣除比例", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal HousingFundDeductionRate { get; set; }
    /// <summary>
    /// 计算公式
    /// </summary>
    [SugarColumn(ColumnName = "calculation_formula", ColumnDescription = "计算公式", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string CalculationFormula { get; set; } = string.Empty;
    /// <summary>
    /// 规则说明
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "规则说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    [SugarColumn(ColumnName = "tax_calc_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TaxCalcStatus { get; set; }
    /// <summary>
    /// 关联工厂
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? RelatedPlant { get; set; }
}
