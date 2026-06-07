// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.CompensationBenefits
// 文件名称：TaktTaxCalcValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaxCalc 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTaxCalc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.CompensationBenefits;

namespace Takt.Application.Validators.HumanResource.CompensationBenefits;

// ========================================
// 创建TaxCalc 验证器
// ========================================

/// <summary>
/// 创建TaxCalc DTO 验证器
/// </summary>
public class TaktTaxCalcCreateValidator : AbstractValidator<TaktTaxCalcCreateDto>
{
    /// <summary>
    /// 初始化 创建TaxCalc 校验规则
    /// </summary>
    public TaktTaxCalcCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.RuleCode)
            .NotEmpty().WithMessage("规则编码不能为空")
            .MaximumLength(64).WithMessage("规则编码长度不能超过64个字符");
        RuleFor(x => x.RuleName)
            .NotEmpty().WithMessage("规则名称不能为空")
            .MaximumLength(128).WithMessage("规则名称长度不能超过128个字符");
        RuleFor(x => x.CalculationFormula)
            .NotEmpty().WithMessage("计算公式不能为空")
            .MaximumLength(500).WithMessage("计算公式长度不能超过500个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("规则说明不能为空")
            .MaximumLength(1000).WithMessage("规则说明长度不能超过1000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TaxCalc 验证器
// ========================================

/// <summary>
/// 更新TaxCalc DTO 验证器
/// </summary>
public class TaktTaxCalcUpdateValidator : AbstractValidator<TaktTaxCalcUpdateDto>
{
    /// <summary>
    /// 初始化 更新TaxCalc 校验规则
    /// </summary>
    public TaktTaxCalcUpdateValidator()
    {
        RuleFor(x => x.TaxCalcId)
            .GreaterThan(0).WithMessage("TaxCalcID无效");
    }
}

// ========================================
// 导入TaxCalc 验证器
// ========================================

/// <summary>
/// 导入TaxCalc DTO 验证器
/// </summary>
public class TaktTaxCalcImportValidator : AbstractValidator<TaktTaxCalcImportDto>
{
    /// <summary>
    /// 初始化 导入TaxCalc 校验规则
    /// </summary>
    public TaktTaxCalcImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.RuleCode)
            .NotEmpty().WithMessage("规则编码不能为空")
            .MaximumLength(64).WithMessage("规则编码长度不能超过64个字符");
        RuleFor(x => x.RuleName)
            .NotEmpty().WithMessage("规则名称不能为空")
            .MaximumLength(128).WithMessage("规则名称长度不能超过128个字符");
        RuleFor(x => x.CalculationFormula)
            .NotEmpty().WithMessage("计算公式不能为空")
            .MaximumLength(500).WithMessage("计算公式长度不能超过500个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("规则说明不能为空")
            .MaximumLength(1000).WithMessage("规则说明长度不能超过1000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
