// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIssue 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityIssue 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityIssue 验证器
// ========================================

/// <summary>
/// 创建QualityIssue DTO 验证器
/// </summary>
public class TaktQualityIssueCreateValidator : AbstractValidator<TaktQualityIssueCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityIssue 校验规则
    /// </summary>
    public TaktQualityIssueCreateValidator()
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
        RuleFor(x => x.QualityIssueCode)
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
// 更新QualityIssue 验证器
// ========================================

/// <summary>
/// 更新QualityIssue DTO 验证器
/// </summary>
public class TaktQualityIssueUpdateValidator : AbstractValidator<TaktQualityIssueUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityIssue 校验规则
    /// </summary>
    public TaktQualityIssueUpdateValidator()
    {
        RuleFor(x => x.QualityIssueId)
            .GreaterThan(0).WithMessage("QualityIssueID无效");
    }
}

// ========================================
// 导入QualityIssue 验证器
// ========================================

/// <summary>
/// 导入QualityIssue DTO 验证器
/// </summary>
public class TaktQualityIssueImportValidator : AbstractValidator<TaktQualityIssueImportDto>
{
    /// <summary>
    /// 初始化 导入QualityIssue 校验规则
    /// </summary>
    public TaktQualityIssueImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityIssueCode)
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
