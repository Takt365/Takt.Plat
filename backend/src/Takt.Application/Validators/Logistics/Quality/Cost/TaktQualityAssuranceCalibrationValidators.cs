// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationCalibrationValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityOperationCalibration 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityOperationCalibration 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityOperationCalibration 验证器
// ========================================

/// <summary>
/// 创建QualityOperationCalibration DTO 验证器
/// </summary>
public class TaktQualityOperationCalibrationCreateValidator : AbstractValidator<TaktQualityOperationCalibrationCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityOperationCalibration 校验规则
    /// </summary>
    public TaktQualityOperationCalibrationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.QualityOperationId)
            .GreaterThanOrEqualTo(0).WithMessage("品质业务主表ID不能为负数");
        RuleFor(x => x.QualityOperationCode)
            .NotEmpty().WithMessage("品质业务编码不能为空")
            .MaximumLength(40).WithMessage("品质业务编码长度不能超过40个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityOperationCalibration 验证器
// ========================================

/// <summary>
/// 更新QualityOperationCalibration DTO 验证器
/// </summary>
public class TaktQualityOperationCalibrationUpdateValidator : AbstractValidator<TaktQualityOperationCalibrationUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityOperationCalibration 校验规则
    /// </summary>
    public TaktQualityOperationCalibrationUpdateValidator()
    {
        RuleFor(x => x.QualityOperationCalibrationId)
            .GreaterThan(0).WithMessage("QualityOperationCalibrationID无效");
    }
}

// ========================================
// 导入QualityOperationCalibration 验证器
// ========================================

/// <summary>
/// 导入QualityOperationCalibration DTO 验证器
/// </summary>
public class TaktQualityOperationCalibrationImportValidator : AbstractValidator<TaktQualityOperationCalibrationImportDto>
{
    /// <summary>
    /// 初始化 导入QualityOperationCalibration 校验规则
    /// </summary>
    public TaktQualityOperationCalibrationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.QualityOperationId)
            .GreaterThanOrEqualTo(0).WithMessage("品质业务主表ID不能为负数");
        RuleFor(x => x.QualityOperationCode)
            .NotEmpty().WithMessage("品质业务编码不能为空")
            .MaximumLength(40).WithMessage("品质业务编码长度不能超过40个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
