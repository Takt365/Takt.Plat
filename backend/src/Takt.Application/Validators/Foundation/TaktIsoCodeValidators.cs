// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktIsoCodeValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：IsoCode 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIsoCode 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建IsoCode 验证器
// ========================================

/// <summary>
/// 创建IsoCode DTO 验证器
/// </summary>
public class TaktIsoCodeCreateValidator : AbstractValidator<TaktIsoCodeCreateDto>
{
    /// <summary>
    /// 初始化 创建IsoCode 校验规则
    /// </summary>
    public TaktIsoCodeCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.IsoCode)
            .NotEmpty().WithMessage("ISO 编码不能为空")
            .MaximumLength(20).WithMessage("ISO 编码长度不能超过20个字符");
        RuleFor(x => x.IsoName)
            .NotEmpty().WithMessage("ISO 名称不能为空")
            .MaximumLength(100).WithMessage("ISO 名称长度不能超过100个字符");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述说明长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新IsoCode 验证器
// ========================================

/// <summary>
/// 更新IsoCode DTO 验证器
/// </summary>
public class TaktIsoCodeUpdateValidator : AbstractValidator<TaktIsoCodeUpdateDto>
{
    /// <summary>
    /// 初始化 更新IsoCode 校验规则
    /// </summary>
    public TaktIsoCodeUpdateValidator()
    {
        RuleFor(x => x.IsoCodeId)
            .GreaterThan(0).WithMessage("IsoCodeID无效");
    }
}

// ========================================
// 导入IsoCode 验证器
// ========================================

/// <summary>
/// 导入IsoCode DTO 验证器
/// </summary>
public class TaktIsoCodeImportValidator : AbstractValidator<TaktIsoCodeImportDto>
{
    /// <summary>
    /// 初始化 导入IsoCode 校验规则
    /// </summary>
    public TaktIsoCodeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.IsoCode)
            .NotEmpty().WithMessage("ISO 编码不能为空")
            .MaximumLength(20).WithMessage("ISO 编码长度不能超过20个字符");
        RuleFor(x => x.IsoName)
            .NotEmpty().WithMessage("ISO 名称不能为空")
            .MaximumLength(100).WithMessage("ISO 名称长度不能超过100个字符");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
