// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureAssyReworkValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityFailureAssyRework 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityFailureAssyRework 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityFailureAssyRework 验证器
// ========================================

/// <summary>
/// 创建QualityFailureAssyRework DTO 验证器
/// </summary>
public class TaktQualityFailureAssyReworkCreateValidator : AbstractValidator<TaktQualityFailureAssyReworkCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityFailureAssyRework 校验规则
    /// </summary>
    public TaktQualityFailureAssyReworkCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.QualityFailureId)
            .GreaterThanOrEqualTo(0).WithMessage("品质问题主表ID不能为负数");
        RuleFor(x => x.QualityFailureCode)
            .NotEmpty().WithMessage("品质问题编码不能为空")
            .MaximumLength(30).WithMessage("品质问题编码长度不能超过30个字符");
        RuleFor(x => x.AssyCustomerName)
            .MaximumLength(50).WithMessage("组装顾客名长度不能超过50个字符");
        RuleFor(x => x.AssyDebitNoteNo)
            .MaximumLength(30).WithMessage("组装 Debit Note No长度不能超过30个字符");
        RuleFor(x => x.AssyRecorder)
            .MaximumLength(30).WithMessage("组装不良改修应对记录者长度不能超过30个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityFailureAssyRework 验证器
// ========================================

/// <summary>
/// 更新QualityFailureAssyRework DTO 验证器
/// </summary>
public class TaktQualityFailureAssyReworkUpdateValidator : AbstractValidator<TaktQualityFailureAssyReworkUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityFailureAssyRework 校验规则
    /// </summary>
    public TaktQualityFailureAssyReworkUpdateValidator()
    {
        RuleFor(x => x.QualityFailureAssyReworkId)
            .GreaterThan(0).WithMessage("QualityFailureAssyReworkID无效");
    }
}

// ========================================
// 导入QualityFailureAssyRework 验证器
// ========================================

/// <summary>
/// 导入QualityFailureAssyRework DTO 验证器
/// </summary>
public class TaktQualityFailureAssyReworkImportValidator : AbstractValidator<TaktQualityFailureAssyReworkImportDto>
{
    /// <summary>
    /// 初始化 导入QualityFailureAssyRework 校验规则
    /// </summary>
    public TaktQualityFailureAssyReworkImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.QualityFailureId)
            .GreaterThanOrEqualTo(0).WithMessage("品质问题主表ID不能为负数");
        RuleFor(x => x.QualityFailureCode)
            .NotEmpty().WithMessage("品质问题编码不能为空")
            .MaximumLength(30).WithMessage("品质问题编码长度不能超过30个字符");
        RuleFor(x => x.AssyCustomerName)
            .MaximumLength(50).WithMessage("组装顾客名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.AssyCustomerName));
        RuleFor(x => x.AssyDebitNoteNo)
            .MaximumLength(30).WithMessage("组装 Debit Note No长度不能超过30个字符").When(x => !string.IsNullOrWhiteSpace(x.AssyDebitNoteNo));
        RuleFor(x => x.AssyRecorder)
            .MaximumLength(30).WithMessage("组装不良改修应对记录者长度不能超过30个字符").When(x => !string.IsNullOrWhiteSpace(x.AssyRecorder));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
