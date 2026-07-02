// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInvoice 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseInvoice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchaseInvoice 验证器
// ========================================

/// <summary>
/// 创建PurchaseInvoice DTO 验证器
/// </summary>
public class TaktPurchaseInvoiceCreateValidator : AbstractValidator<TaktPurchaseInvoiceCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseInvoice 校验规则
    /// </summary>
    public TaktPurchaseInvoiceCreateValidator()
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
        RuleFor(x => x.PurchaseInvoiceCode)
            .NotEmpty().WithMessage("采购发票编码不能为空")
            .MaximumLength(50).WithMessage("采购发票编码长度不能超过50个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供应商名称不能为空")
            .MaximumLength(200).WithMessage("供应商名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseInvoice 验证器
// ========================================

/// <summary>
/// 更新PurchaseInvoice DTO 验证器
/// </summary>
public class TaktPurchaseInvoiceUpdateValidator : AbstractValidator<TaktPurchaseInvoiceUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseInvoice 校验规则
    /// </summary>
    public TaktPurchaseInvoiceUpdateValidator()
    {
        RuleFor(x => x.PurchaseInvoiceId)
            .GreaterThan(0).WithMessage("PurchaseInvoiceID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseInvoiceCode)
            .NotEmpty().WithMessage("采购发票编码不能为空")
            .MaximumLength(50).WithMessage("采购发票编码长度不能超过50个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供应商名称不能为空")
            .MaximumLength(200).WithMessage("供应商名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PurchaseInvoice 验证器
// ========================================

/// <summary>
/// 导入PurchaseInvoice DTO 验证器
/// </summary>
public class TaktPurchaseInvoiceImportValidator : AbstractValidator<TaktPurchaseInvoiceImportDto>
{
    /// <summary>
    /// 初始化 导入PurchaseInvoice 校验规则
    /// </summary>
    public TaktPurchaseInvoiceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseInvoiceCode)
            .NotEmpty().WithMessage("采购发票编码不能为空")
            .MaximumLength(50).WithMessage("采购发票编码长度不能超过50个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(50).WithMessage("供应商编码长度不能超过50个字符");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供应商名称不能为空")
            .MaximumLength(200).WithMessage("供应商名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
