// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesPriceValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPrice 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesPrice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesPrice 验证器
// ========================================

/// <summary>
/// 创建SalesPrice DTO 验证器
/// </summary>
public class TaktSalesPriceCreateValidator : AbstractValidator<TaktSalesPriceCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesPrice 校验规则
    /// </summary>
    public TaktSalesPriceCreateValidator()
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
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("销售价格编码不能为空")
            .MaximumLength(50).WithMessage("销售价格编码长度不能超过50个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("价格类型不能为空")
            .MaximumLength(4).WithMessage("价格类型长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesPrice 验证器
// ========================================

/// <summary>
/// 更新SalesPrice DTO 验证器
/// </summary>
public class TaktSalesPriceUpdateValidator : AbstractValidator<TaktSalesPriceUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesPrice 校验规则
    /// </summary>
    public TaktSalesPriceUpdateValidator()
    {
        RuleFor(x => x.SalesPriceId)
            .GreaterThan(0).WithMessage("SalesPriceID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("销售价格编码不能为空")
            .MaximumLength(50).WithMessage("销售价格编码长度不能超过50个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("价格类型不能为空")
            .MaximumLength(4).WithMessage("价格类型长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesPrice 验证器
// ========================================

/// <summary>
/// 导入SalesPrice DTO 验证器
/// </summary>
public class TaktSalesPriceImportValidator : AbstractValidator<TaktSalesPriceImportDto>
{
    /// <summary>
    /// 初始化 导入SalesPrice 校验规则
    /// </summary>
    public TaktSalesPriceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("销售价格编码不能为空")
            .MaximumLength(50).WithMessage("销售价格编码长度不能超过50个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("价格类型不能为空")
            .MaximumLength(4).WithMessage("价格类型长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
