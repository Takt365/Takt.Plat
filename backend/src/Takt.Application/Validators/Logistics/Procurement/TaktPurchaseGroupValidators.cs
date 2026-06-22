// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchaseGroupValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseGroup 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchaseGroup 验证器
// ========================================

/// <summary>
/// 创建PurchaseGroup DTO 验证器
/// </summary>
public class TaktPurchaseGroupCreateValidator : AbstractValidator<TaktPurchaseGroupCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseGroup 校验规则
    /// </summary>
    public TaktPurchaseGroupCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseGroupCode)
            .NotEmpty().WithMessage("采购组编码不能为空")
            .MaximumLength(50).WithMessage("采购组编码长度不能超过50个字符");
        RuleFor(x => x.PurchaseGroupName)
            .NotEmpty().WithMessage("采购组名称不能为空")
            .MaximumLength(100).WithMessage("采购组名称长度不能超过100个字符");
        RuleFor(x => x.PurchaseGroupDescription)
            .MaximumLength(200).WithMessage("采购组描述长度不能超过200个字符");
        RuleFor(x => x.ResponsibleUserId)
            .GreaterThanOrEqualTo(0).WithMessage("采购组负责人用户ID不能为负数");
        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符");
        RuleFor(x => x.ContactEmail)
            .MaximumLength(100).WithMessage("联系邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseGroup 验证器
// ========================================

/// <summary>
/// 更新PurchaseGroup DTO 验证器
/// </summary>
public class TaktPurchaseGroupUpdateValidator : AbstractValidator<TaktPurchaseGroupUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseGroup 校验规则
    /// </summary>
    public TaktPurchaseGroupUpdateValidator()
    {
        RuleFor(x => x.PurchaseGroupId)
            .GreaterThan(0).WithMessage("PurchaseGroupID无效");
    }
}

// ========================================
// 导入PurchaseGroup 验证器
// ========================================

/// <summary>
/// 导入PurchaseGroup DTO 验证器
/// </summary>
public class TaktPurchaseGroupImportValidator : AbstractValidator<TaktPurchaseGroupImportDto>
{
    /// <summary>
    /// 初始化 导入PurchaseGroup 校验规则
    /// </summary>
    public TaktPurchaseGroupImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PurchaseGroupCode)
            .NotEmpty().WithMessage("采购组编码不能为空")
            .MaximumLength(50).WithMessage("采购组编码长度不能超过50个字符");
        RuleFor(x => x.PurchaseGroupName)
            .NotEmpty().WithMessage("采购组名称不能为空")
            .MaximumLength(100).WithMessage("采购组名称长度不能超过100个字符");
        RuleFor(x => x.PurchaseGroupDescription)
            .MaximumLength(200).WithMessage("采购组描述长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.PurchaseGroupDescription));
        RuleFor(x => x.ResponsibleUserId)
            .GreaterThanOrEqualTo(0).WithMessage("采购组负责人用户ID不能为负数");
        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ContactPhone));
        RuleFor(x => x.ContactEmail)
            .MaximumLength(100).WithMessage("联系邮箱长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail))
            .EmailAddress().WithMessage("联系邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
