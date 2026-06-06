// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.CompensationBenefits
// 文件名称：TaktTaxCalcDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaxCalc 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTaxCalc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.CompensationBenefits;

// ========================================
// TaxCalc 响应 DTO
// ========================================

/// <summary>
/// 个税计算规则（税率档、扣除标准等）
/// 对应前端 TaktTaxCalcDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTaxCalcDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TaxCalcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCalcId { get; set; }

    /// <summary>
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 税务年度
    /// </summary>
    public int TaxYear { get; set; } = 0;

    /// <summary>
    /// 税收起征点
    /// </summary>
    public decimal TaxThreshold { get; set; }

    /// <summary>
    /// 应纳税所得额下限
    /// </summary>
    public decimal TaxableIncomeMin { get; set; }

    /// <summary>
    /// 应纳税所得额上限
    /// </summary>
    public decimal TaxableIncomeMax { get; set; }

    /// <summary>
    /// 税率（%）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 速算扣除数
    /// </summary>
    public decimal QuickDeduction { get; set; }

    /// <summary>
    /// 专项扣除标准
    /// </summary>
    public decimal SpecialDeductionStandard { get; set; }

    /// <summary>
    /// 社保扣除比例（%）
    /// </summary>
    public decimal SocialSecurityDeductionRate { get; set; }

    /// <summary>
    /// 公积金扣除比例（%）
    /// </summary>
    public decimal HousingFundDeductionRate { get; set; }

    /// <summary>
    /// 计算公式
    /// </summary>
    public string CalculationFormula { get; set; } = string.Empty;

    /// <summary>
    /// 规则说明
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int TaxCalcStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// TaxCalc 查询 DTO
// ========================================

/// <summary>
/// TaxCalc 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTaxCalcQueryDto : TaktPagedQuery
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
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 税务年度
    /// </summary>
    public int? TaxYear { get; set; }

    /// <summary>
    /// 税收起征点
    /// </summary>
    public decimal? TaxThreshold { get; set; }

    /// <summary>
    /// 应纳税所得额下限
    /// </summary>
    public decimal? TaxableIncomeMin { get; set; }

    /// <summary>
    /// 应纳税所得额上限
    /// </summary>
    public decimal? TaxableIncomeMax { get; set; }

    /// <summary>
    /// 税率（%）
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// 速算扣除数
    /// </summary>
    public decimal? QuickDeduction { get; set; }

    /// <summary>
    /// 专项扣除标准
    /// </summary>
    public decimal? SpecialDeductionStandard { get; set; }

    /// <summary>
    /// 社保扣除比例（%）
    /// </summary>
    public decimal? SocialSecurityDeductionRate { get; set; }

    /// <summary>
    /// 公积金扣除比例（%）
    /// </summary>
    public decimal? HousingFundDeductionRate { get; set; }

    /// <summary>
    /// 计算公式
    /// </summary>
    public string? CalculationFormula { get; set; } = string.Empty;

    /// <summary>
    /// 规则说明
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? TaxCalcStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 创建TaxCalc DTO
// ========================================

/// <summary>
/// 创建TaxCalc DTO
/// </summary>
public class TaktTaxCalcCreateDto
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
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "规则编码（租户+公司内唯一）不能为空")]
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称
    /// </summary>
    [Required(ErrorMessage = "规则名称不能为空")]
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 税务年度
    /// </summary>
    public int TaxYear { get; set; } = 0;

    /// <summary>
    /// 税收起征点
    /// </summary>
    public decimal TaxThreshold { get; set; }

    /// <summary>
    /// 应纳税所得额下限
    /// </summary>
    public decimal TaxableIncomeMin { get; set; }

    /// <summary>
    /// 应纳税所得额上限
    /// </summary>
    public decimal TaxableIncomeMax { get; set; }

    /// <summary>
    /// 税率（%）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 速算扣除数
    /// </summary>
    public decimal QuickDeduction { get; set; }

    /// <summary>
    /// 专项扣除标准
    /// </summary>
    public decimal SpecialDeductionStandard { get; set; }

    /// <summary>
    /// 社保扣除比例（%）
    /// </summary>
    public decimal SocialSecurityDeductionRate { get; set; }

    /// <summary>
    /// 公积金扣除比例（%）
    /// </summary>
    public decimal HousingFundDeductionRate { get; set; }

    /// <summary>
    /// 计算公式
    /// </summary>
    [Required(ErrorMessage = "计算公式不能为空")]
    public string CalculationFormula { get; set; } = string.Empty;

    /// <summary>
    /// 规则说明
    /// </summary>
    [Required(ErrorMessage = "规则说明不能为空")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int TaxCalcStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 更新TaxCalc DTO
// ========================================

/// <summary>
/// 更新TaxCalc DTO
/// 继承 TaktTaxCalcCreateDto，添加 TaxCalcId 字段
/// </summary>
public class TaktTaxCalcUpdateDto : TaktTaxCalcCreateDto
{
    /// <summary>
    /// TaxCalcID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCalcId { get; set; }

}

// ========================================
// TaxCalc 状态 DTO
// ========================================

/// <summary>
/// TaxCalc 状态更新 DTO
/// </summary>
public class TaktTaxCalcStatusDto
{
    /// <summary>
    /// TaxCalcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCalcId { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（0=启用 1=停用）不能为空")]
    public int TaxCalcStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TaxCalc 导入模板行 DTO
/// </summary>
public class TaktTaxCalcTemplateDto
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
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 税务年度
    /// </summary>
    public int? TaxYear { get; set; }

    /// <summary>
    /// 计算公式
    /// </summary>
    public string? CalculationFormula { get; set; } = string.Empty;

    /// <summary>
    /// 规则说明
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? TaxCalcStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
/// TaxCalc 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTaxCalcImportDto
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
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 税务年度
    /// </summary>
    public int? TaxYear { get; set; }

    /// <summary>
    /// 计算公式
    /// </summary>
    public string? CalculationFormula { get; set; } = string.Empty;

    /// <summary>
    /// 规则说明
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? TaxCalcStatus { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
/// TaxCalc 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTaxCalcExportDto
{
    /// <summary>
    /// TaxCalcID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCalcId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则编码（租户+公司内唯一）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 税务年度
    /// </summary>
    public int TaxYear { get; set; } = 0;

    /// <summary>
    /// 税收起征点
    /// </summary>
    public decimal TaxThreshold { get; set; }

    /// <summary>
    /// 应纳税所得额下限
    /// </summary>
    public decimal TaxableIncomeMin { get; set; }

    /// <summary>
    /// 应纳税所得额上限
    /// </summary>
    public decimal TaxableIncomeMax { get; set; }

    /// <summary>
    /// 税率（%）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 速算扣除数
    /// </summary>
    public decimal QuickDeduction { get; set; }

    /// <summary>
    /// 专项扣除标准
    /// </summary>
    public decimal SpecialDeductionStandard { get; set; }

    /// <summary>
    /// 社保扣除比例（%）
    /// </summary>
    public decimal SocialSecurityDeductionRate { get; set; }

    /// <summary>
    /// 公积金扣除比例（%）
    /// </summary>
    public decimal HousingFundDeductionRate { get; set; }

    /// <summary>
    /// 计算公式
    /// </summary>
    public string CalculationFormula { get; set; } = string.Empty;

    /// <summary>
    /// 规则说明
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int TaxCalcStatus { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
