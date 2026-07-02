// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktVendorValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：Vendor 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktVendor 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建Vendor 验证器
// ========================================

/// <summary>
/// 创建Vendor DTO 验证器
/// </summary>
public class TaktVendorCreateValidator : AbstractValidator<TaktVendorCreateDto>
{
    /// <summary>
    /// 初始化 创建Vendor 校验规则
    /// </summary>
    public TaktVendorCreateValidator()
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
        RuleFor(x => x.VendorCode)
            .NotEmpty().WithMessage("经销商编码不能为空")
            .MaximumLength(20).WithMessage("经销商编码长度不能超过20个字符");
        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("经销商名称不能为空")
            .MaximumLength(80).WithMessage("经销商名称长度不能超过80个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(3).WithMessage("结算币种代码长度不能超过3个字符");
        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("付款条件不能为空")
            .MaximumLength(40).WithMessage("付款条件长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Vendor 验证器
// ========================================

/// <summary>
/// 更新Vendor DTO 验证器
/// </summary>
public class TaktVendorUpdateValidator : AbstractValidator<TaktVendorUpdateDto>
{
    /// <summary>
    /// 初始化 更新Vendor 校验规则
    /// </summary>
    public TaktVendorUpdateValidator()
    {
        RuleFor(x => x.VendorId)
            .GreaterThan(0).WithMessage("VendorID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.VendorCode)
            .NotEmpty().WithMessage("经销商编码不能为空")
            .MaximumLength(20).WithMessage("经销商编码长度不能超过20个字符");
        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("经销商名称不能为空")
            .MaximumLength(80).WithMessage("经销商名称长度不能超过80个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(3).WithMessage("结算币种代码长度不能超过3个字符");
        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("付款条件不能为空")
            .MaximumLength(40).WithMessage("付款条件长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Vendor 验证器
// ========================================

/// <summary>
/// 导入Vendor DTO 验证器
/// </summary>
public class TaktVendorImportValidator : AbstractValidator<TaktVendorImportDto>
{
    /// <summary>
    /// 初始化 导入Vendor 校验规则
    /// </summary>
    public TaktVendorImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.VendorCode)
            .NotEmpty().WithMessage("经销商编码不能为空")
            .MaximumLength(20).WithMessage("经销商编码长度不能超过20个字符");
        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("经销商名称不能为空")
            .MaximumLength(80).WithMessage("经销商名称长度不能超过80个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(3).WithMessage("结算币种代码长度不能超过3个字符");
        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("付款条件不能为空")
            .MaximumLength(40).WithMessage("付款条件长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
