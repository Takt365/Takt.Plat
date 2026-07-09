// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationValidators.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：EcNotification 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcNotification 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcNotification 验证器
// ========================================

/// <summary>
/// 创建EcNotification DTO 验证器
/// </summary>
public class TaktEcNotificationCreateValidator : AbstractValidator<TaktEcNotificationCreateDto>
{
    /// <summary>
    /// 初始化 创建EcNotification 校验规则
    /// </summary>
    public TaktEcNotificationCreateValidator()
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
        RuleFor(x => x.EcNotificationNo)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(30).WithMessage("通知单号长度不能超过30个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(30).WithMessage("设变单号长度不能超过30个字符");
        RuleFor(x => x.EcNotificationNotifierId)
            .GreaterThanOrEqualTo(0).WithMessage("通知人ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcNotification 验证器
// ========================================

/// <summary>
/// 更新EcNotification DTO 验证器
/// </summary>
public class TaktEcNotificationUpdateValidator : AbstractValidator<TaktEcNotificationUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcNotification 校验规则
    /// </summary>
    public TaktEcNotificationUpdateValidator()
    {
        RuleFor(x => x.EcNotificationId)
            .GreaterThan(0).WithMessage("EcNotificationID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcNotificationNo)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(30).WithMessage("通知单号长度不能超过30个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(30).WithMessage("设变单号长度不能超过30个字符");
        RuleFor(x => x.EcNotificationNotifierId)
            .GreaterThanOrEqualTo(0).WithMessage("通知人ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EcNotification 验证器
// ========================================

/// <summary>
/// 导入EcNotification DTO 验证器
/// </summary>
public class TaktEcNotificationImportValidator : AbstractValidator<TaktEcNotificationImportDto>
{
    /// <summary>
    /// 初始化 导入EcNotification 校验规则
    /// </summary>
    public TaktEcNotificationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcNotificationNo)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(30).WithMessage("通知单号长度不能超过30个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(30).WithMessage("设变单号长度不能超过30个字符");
        RuleFor(x => x.EcNotificationNotifierId)
            .GreaterThanOrEqualTo(0).WithMessage("通知人ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
