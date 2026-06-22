// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityAssurance 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityAssurance 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityAssurance 验证器
// ========================================

/// <summary>
/// 创建QualityAssurance DTO 验证器
/// </summary>
public class TaktQualityAssuranceCreateValidator : AbstractValidator<TaktQualityAssuranceCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityAssurance 校验规则
    /// </summary>
    public TaktQualityAssuranceCreateValidator()
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
        RuleFor(x => x.QualityAssuranceCode)
            .NotEmpty().WithMessage("品质业务编码不能为空")
            .MaximumLength(30).WithMessage("品质业务编码长度不能超过30个字符");
        RuleFor(x => x.AssuranceMonth)
            .NotEmpty().WithMessage("业务年月不能为空")
            .MaximumLength(7).WithMessage("业务年月长度不能超过7个字符");
        RuleFor(x => x.CustomerName)
            .MaximumLength(50).WithMessage("顾客名长度不能超过50个字符");
        RuleFor(x => x.DebitNoteNo)
            .MaximumLength(30).WithMessage("Debit Note No长度不能超过30个字符");
        RuleFor(x => x.Recorder)
            .MaximumLength(30).WithMessage("记录者长度不能超过30个字符");
        RuleFor(x => x.CostCurrency)
            .NotEmpty().WithMessage("成本币种不能为空")
            .MaximumLength(10).WithMessage("成本币种长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityAssurance 验证器
// ========================================

/// <summary>
/// 更新QualityAssurance DTO 验证器
/// </summary>
public class TaktQualityAssuranceUpdateValidator : AbstractValidator<TaktQualityAssuranceUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityAssurance 校验规则
    /// </summary>
    public TaktQualityAssuranceUpdateValidator()
    {
        RuleFor(x => x.QualityAssuranceId)
            .GreaterThan(0).WithMessage("QualityAssuranceID无效");
    }
}

// ========================================
// 导入QualityAssurance 验证器
// ========================================

/// <summary>
/// 导入QualityAssurance DTO 验证器
/// </summary>
public class TaktQualityAssuranceImportValidator : AbstractValidator<TaktQualityAssuranceImportDto>
{
    /// <summary>
    /// 初始化 导入QualityAssurance 校验规则
    /// </summary>
    public TaktQualityAssuranceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityAssuranceCode)
            .NotEmpty().WithMessage("品质业务编码不能为空")
            .MaximumLength(30).WithMessage("品质业务编码长度不能超过30个字符");
        RuleFor(x => x.AssuranceMonth)
            .NotEmpty().WithMessage("业务年月不能为空")
            .MaximumLength(7).WithMessage("业务年月长度不能超过7个字符");
        RuleFor(x => x.CustomerName)
            .MaximumLength(50).WithMessage("顾客名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.CustomerName));
        RuleFor(x => x.DebitNoteNo)
            .MaximumLength(30).WithMessage("Debit Note No长度不能超过30个字符").When(x => !string.IsNullOrWhiteSpace(x.DebitNoteNo));
        RuleFor(x => x.Recorder)
            .MaximumLength(30).WithMessage("记录者长度不能超过30个字符").When(x => !string.IsNullOrWhiteSpace(x.Recorder));
        RuleFor(x => x.CostCurrency)
            .NotEmpty().WithMessage("成本币种不能为空")
            .MaximumLength(10).WithMessage("成本币种长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
