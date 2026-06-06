// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityFailure 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityFailure 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityFailure 验证器
// ========================================

/// <summary>
/// 创建QualityFailure DTO 验证器
/// </summary>
public class TaktQualityFailureCreateValidator : AbstractValidator<TaktQualityFailureCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityFailure 校验规则
    /// </summary>
    public TaktQualityFailureCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityFailureCode)
            .NotEmpty().WithMessage("品质问题编码不能为空")
            .MaximumLength(30).WithMessage("品质问题编码长度不能超过30个字符");
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("机种/产品型号不能为空")
            .MaximumLength(255).WithMessage("机种/产品型号长度不能超过255个字符");
        RuleFor(x => x.Lot)
            .NotEmpty().WithMessage("批次号/Lot No不能为空")
            .MaximumLength(30).WithMessage("批次号/Lot No长度不能超过30个字符");
        RuleFor(x => x.QualityProblemsResponse)
            .MaximumLength(255).WithMessage("品质问题应对摘要长度不能超过255个字符");
        RuleFor(x => x.ReworkDueToDefects)
            .MaximumLength(255).WithMessage("不良改修应对摘要长度不能超过255个字符");
        RuleFor(x => x.NeedRework)
            .MaximumLength(1).WithMessage("是否需要不良改修应对长度不能超过1个字符");
        RuleFor(x => x.CostCurrency)
            .NotEmpty().WithMessage("成本币种不能为空")
            .MaximumLength(10).WithMessage("成本币种长度不能超过10个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityFailure 验证器
// ========================================

/// <summary>
/// 更新QualityFailure DTO 验证器
/// </summary>
public class TaktQualityFailureUpdateValidator : AbstractValidator<TaktQualityFailureUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityFailure 校验规则
    /// </summary>
    public TaktQualityFailureUpdateValidator()
    {
        RuleFor(x => x.QualityFailureId)
            .GreaterThan(0).WithMessage("QualityFailureID无效");
    }
}

// ========================================
// 导入QualityFailure 验证器
// ========================================

/// <summary>
/// 导入QualityFailure DTO 验证器
/// </summary>
public class TaktQualityFailureImportValidator : AbstractValidator<TaktQualityFailureImportDto>
{
    /// <summary>
    /// 初始化 导入QualityFailure 校验规则
    /// </summary>
    public TaktQualityFailureImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityFailureCode)
            .NotEmpty().WithMessage("品质问题编码不能为空")
            .MaximumLength(30).WithMessage("品质问题编码长度不能超过30个字符");
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("机种/产品型号不能为空")
            .MaximumLength(255).WithMessage("机种/产品型号长度不能超过255个字符");
        RuleFor(x => x.Lot)
            .NotEmpty().WithMessage("批次号/Lot No不能为空")
            .MaximumLength(30).WithMessage("批次号/Lot No长度不能超过30个字符");
        RuleFor(x => x.QualityProblemsResponse)
            .MaximumLength(255).WithMessage("品质问题应对摘要长度不能超过255个字符").When(x => !string.IsNullOrWhiteSpace(x.QualityProblemsResponse));
        RuleFor(x => x.ReworkDueToDefects)
            .MaximumLength(255).WithMessage("不良改修应对摘要长度不能超过255个字符").When(x => !string.IsNullOrWhiteSpace(x.ReworkDueToDefects));
        RuleFor(x => x.NeedRework)
            .MaximumLength(1).WithMessage("是否需要不良改修应对长度不能超过1个字符").When(x => !string.IsNullOrWhiteSpace(x.NeedRework));
        RuleFor(x => x.CostCurrency)
            .NotEmpty().WithMessage("成本币种不能为空")
            .MaximumLength(10).WithMessage("成本币种长度不能超过10个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
