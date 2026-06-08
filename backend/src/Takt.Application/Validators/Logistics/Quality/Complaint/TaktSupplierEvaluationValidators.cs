// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：SupplierEvaluation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSupplierEvaluation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建SupplierEvaluation 验证器
// ========================================

/// <summary>
/// 创建SupplierEvaluation DTO 验证器
/// </summary>
public class TaktSupplierEvaluationCreateValidator : AbstractValidator<TaktSupplierEvaluationCreateDto>
{
    /// <summary>
    /// 初始化 创建SupplierEvaluation 校验规则
    /// </summary>
    public TaktSupplierEvaluationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SupplierEvaluationCode)
            .NotEmpty().WithMessage("评价表编号不能为空")
            .MaximumLength(50).WithMessage("评价表编号长度不能超过50个字符");
        RuleFor(x => x.SupplierId)
            .GreaterThanOrEqualTo(0).WithMessage("供应商ID不能为负数");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供应商名称不能为空")
            .MaximumLength(200).WithMessage("供应商名称长度不能超过200个字符");
        RuleFor(x => x.SupplierCode)
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.EvaluatorBy)
            .MaximumLength(50).WithMessage("评价人长度不能超过50个字符");
        RuleFor(x => x.EvaluationDept)
            .MaximumLength(100).WithMessage("评价部门长度不能超过100个字符");
        RuleFor(x => x.MainStrengths)
            .MaximumLength(2000).WithMessage("主要优点长度不能超过2000个字符");
        RuleFor(x => x.MainIssues)
            .MaximumLength(2000).WithMessage("主要问题/不足长度不能超过2000个字符");
        RuleFor(x => x.ImprovementRequirements)
            .MaximumLength(2000).WithMessage("改进要求/建议长度不能超过2000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SupplierEvaluation 验证器
// ========================================

/// <summary>
/// 更新SupplierEvaluation DTO 验证器
/// </summary>
public class TaktSupplierEvaluationUpdateValidator : AbstractValidator<TaktSupplierEvaluationUpdateDto>
{
    /// <summary>
    /// 初始化 更新SupplierEvaluation 校验规则
    /// </summary>
    public TaktSupplierEvaluationUpdateValidator()
    {
        RuleFor(x => x.SupplierEvaluationId)
            .GreaterThan(0).WithMessage("SupplierEvaluationID无效");
    }
}

// ========================================
// 导入SupplierEvaluation 验证器
// ========================================

/// <summary>
/// 导入SupplierEvaluation DTO 验证器
/// </summary>
public class TaktSupplierEvaluationImportValidator : AbstractValidator<TaktSupplierEvaluationImportDto>
{
    /// <summary>
    /// 初始化 导入SupplierEvaluation 校验规则
    /// </summary>
    public TaktSupplierEvaluationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.SupplierEvaluationCode)
            .NotEmpty().WithMessage("评价表编号不能为空")
            .MaximumLength(50).WithMessage("评价表编号长度不能超过50个字符");
        RuleFor(x => x.SupplierId)
            .GreaterThanOrEqualTo(0).WithMessage("供应商ID不能为负数");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供应商名称不能为空")
            .MaximumLength(200).WithMessage("供应商名称长度不能超过200个字符");
        RuleFor(x => x.SupplierCode)
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SupplierCode));
        RuleFor(x => x.EvaluatorBy)
            .MaximumLength(50).WithMessage("评价人长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EvaluatorBy));
        RuleFor(x => x.EvaluationDept)
            .MaximumLength(100).WithMessage("评价部门长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.EvaluationDept));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
