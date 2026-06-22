// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePriceScale 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchasePriceScale 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchasePriceScale 验证器
// ========================================

/// <summary>
/// 创建PurchasePriceScale DTO 验证器
/// </summary>
public class TaktPurchasePriceScaleCreateValidator : AbstractValidator<TaktPurchasePriceScaleCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchasePriceScale 校验规则
    /// </summary>
    public TaktPurchasePriceScaleCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PurchasePriceItemId)
            .GreaterThanOrEqualTo(0).WithMessage("采购价格明细ID不能为负数");
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchasePriceScale 验证器
// ========================================

/// <summary>
/// 更新PurchasePriceScale DTO 验证器
/// </summary>
public class TaktPurchasePriceScaleUpdateValidator : AbstractValidator<TaktPurchasePriceScaleUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchasePriceScale 校验规则
    /// </summary>
    public TaktPurchasePriceScaleUpdateValidator()
    {
        RuleFor(x => x.PurchasePriceScaleId)
            .GreaterThan(0).WithMessage("PurchasePriceScaleID无效");
    }
}

// ========================================
// 导入PurchasePriceScale 验证器
// ========================================

/// <summary>
/// 导入PurchasePriceScale DTO 验证器
/// </summary>
public class TaktPurchasePriceScaleImportValidator : AbstractValidator<TaktPurchasePriceScaleImportDto>
{
    /// <summary>
    /// 初始化 导入PurchasePriceScale 校验规则
    /// </summary>
    public TaktPurchasePriceScaleImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PurchasePriceItemId)
            .GreaterThanOrEqualTo(0).WithMessage("采购价格明细ID不能为负数");
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
