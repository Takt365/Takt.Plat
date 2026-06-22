// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktSettingValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Setting 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSetting 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Setting 验证器
// ========================================

/// <summary>
/// 创建Setting DTO 验证器
/// </summary>
public class TaktSettingCreateValidator : AbstractValidator<TaktSettingCreateDto>
{
    /// <summary>
    /// 初始化 创建Setting 校验规则
    /// </summary>
    public TaktSettingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SettingKey)
            .NotEmpty().WithMessage("设置键不能为空")
            .MaximumLength(100).WithMessage("设置键长度不能超过100个字符");
        RuleFor(x => x.SettingValue)
            .MaximumLength(4000).WithMessage("设置值长度不能超过4000个字符");
        RuleFor(x => x.SettingName)
            .NotEmpty().WithMessage("设置名称不能为空")
            .MaximumLength(100).WithMessage("设置名称长度不能超过100个字符");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("设置描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Setting 验证器
// ========================================

/// <summary>
/// 更新Setting DTO 验证器
/// </summary>
public class TaktSettingUpdateValidator : AbstractValidator<TaktSettingUpdateDto>
{
    /// <summary>
    /// 初始化 更新Setting 校验规则
    /// </summary>
    public TaktSettingUpdateValidator()
    {
        RuleFor(x => x.SettingId)
            .GreaterThan(0).WithMessage("SettingID无效");
    }
}

// ========================================
// 导入Setting 验证器
// ========================================

/// <summary>
/// 导入Setting DTO 验证器
/// </summary>
public class TaktSettingImportValidator : AbstractValidator<TaktSettingImportDto>
{
    /// <summary>
    /// 初始化 导入Setting 校验规则
    /// </summary>
    public TaktSettingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.SettingKey)
            .NotEmpty().WithMessage("设置键不能为空")
            .MaximumLength(100).WithMessage("设置键长度不能超过100个字符");
        RuleFor(x => x.SettingValue)
            .MaximumLength(4000).WithMessage("设置值长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.SettingValue));
        RuleFor(x => x.SettingName)
            .NotEmpty().WithMessage("设置名称不能为空")
            .MaximumLength(100).WithMessage("设置名称长度不能超过100个字符");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("设置描述长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
