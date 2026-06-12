// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityOperation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityOperation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityOperation 验证器
// ========================================

/// <summary>
/// 创建QualityOperation DTO 验证器
/// </summary>
public class TaktQualityOperationCreateValidator : AbstractValidator<TaktQualityOperationCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityOperation 校验规则
    /// </summary>
    public TaktQualityOperationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.QualityOperationCode)
            .NotEmpty().WithMessage("品质业务编码不能为空")
            .MaximumLength(40).WithMessage("品质业务编码长度不能超过40个字符");
        RuleFor(x => x.OperationMonth)
            .NotEmpty().WithMessage("业务年月不能为空")
            .MaximumLength(7).WithMessage("业务年月长度不能超过7个字符");
        RuleFor(x => x.CustomerName)
            .MaximumLength(40).WithMessage("顾客名长度不能超过40个字符");
        RuleFor(x => x.DebitNoteNo)
            .MaximumLength(30).WithMessage("Debit Note No长度不能超过30个字符");
        RuleFor(x => x.Recorder)
            .MaximumLength(30).WithMessage("记录者长度不能超过30个字符");
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
// 更新QualityOperation 验证器
// ========================================

/// <summary>
/// 更新QualityOperation DTO 验证器
/// </summary>
public class TaktQualityOperationUpdateValidator : AbstractValidator<TaktQualityOperationUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityOperation 校验规则
    /// </summary>
    public TaktQualityOperationUpdateValidator()
    {
        RuleFor(x => x.QualityOperationId)
            .GreaterThan(0).WithMessage("QualityOperationID无效");
    }
}

// ========================================
// 导入QualityOperation 验证器
// ========================================

/// <summary>
/// 导入QualityOperation DTO 验证器
/// </summary>
public class TaktQualityOperationImportValidator : AbstractValidator<TaktQualityOperationImportDto>
{
    /// <summary>
    /// 初始化 导入QualityOperation 校验规则
    /// </summary>
    public TaktQualityOperationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.QualityOperationCode)
            .NotEmpty().WithMessage("品质业务编码不能为空")
            .MaximumLength(40).WithMessage("品质业务编码长度不能超过40个字符");
        RuleFor(x => x.OperationMonth)
            .NotEmpty().WithMessage("业务年月不能为空")
            .MaximumLength(7).WithMessage("业务年月长度不能超过7个字符");
        RuleFor(x => x.CustomerName)
            .MaximumLength(40).WithMessage("顾客名长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CustomerName));
        RuleFor(x => x.DebitNoteNo)
            .MaximumLength(30).WithMessage("Debit Note No长度不能超过30个字符").When(x => !string.IsNullOrWhiteSpace(x.DebitNoteNo));
        RuleFor(x => x.Recorder)
            .MaximumLength(30).WithMessage("记录者长度不能超过30个字符").When(x => !string.IsNullOrWhiteSpace(x.Recorder));
        RuleFor(x => x.CostCurrency)
            .NotEmpty().WithMessage("成本币种不能为空")
            .MaximumLength(10).WithMessage("成本币种长度不能超过10个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
