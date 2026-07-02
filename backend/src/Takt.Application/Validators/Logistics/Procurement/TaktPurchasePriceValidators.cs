// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchasePriceValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePrice 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchasePrice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchasePrice 验证器
// ========================================

/// <summary>
/// 创建PurchasePrice DTO 验证器
/// </summary>
public class TaktPurchasePriceCreateValidator : AbstractValidator<TaktPurchasePriceCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchasePrice 校验规则
    /// </summary>
    public TaktPurchasePriceCreateValidator()
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
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购询价 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchasePrice 验证器
// ========================================

/// <summary>
/// 更新PurchasePrice DTO 验证器
/// </summary>
public class TaktPurchasePriceUpdateValidator : AbstractValidator<TaktPurchasePriceUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchasePrice 校验规则
    /// </summary>
    public TaktPurchasePriceUpdateValidator()
    {
        RuleFor(x => x.PurchasePriceId)
            .GreaterThan(0).WithMessage("PurchasePriceID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购询价 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PurchasePrice 验证器
// ========================================

/// <summary>
/// 导入PurchasePrice DTO 验证器
/// </summary>
public class TaktPurchasePriceImportValidator : AbstractValidator<TaktPurchasePriceImportDto>
{
    /// <summary>
    /// 初始化 导入PurchasePrice 校验规则
    /// </summary>
    public TaktPurchasePriceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购询价 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
