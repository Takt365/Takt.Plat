// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesPriceScaleValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceScale 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesPriceScale 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesPriceScale 验证器
// ========================================

/// <summary>
/// 创建SalesPriceScale DTO 验证器
/// </summary>
public class TaktSalesPriceScaleCreateValidator : AbstractValidator<TaktSalesPriceScaleCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesPriceScale 校验规则
    /// </summary>
    public TaktSalesPriceScaleCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ItemId)
            .GreaterThanOrEqualTo(0).WithMessage("价格明细ID不能为负数");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("销售价格编码不能为空")
            .MaximumLength(50).WithMessage("销售价格编码长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesPriceScale 验证器
// ========================================

/// <summary>
/// 更新SalesPriceScale DTO 验证器
/// </summary>
public class TaktSalesPriceScaleUpdateValidator : AbstractValidator<TaktSalesPriceScaleUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesPriceScale 校验规则
    /// </summary>
    public TaktSalesPriceScaleUpdateValidator()
    {
        RuleFor(x => x.SalesPriceScaleId)
            .GreaterThan(0).WithMessage("SalesPriceScaleID无效");
    }
}

// ========================================
// 导入SalesPriceScale 验证器
// ========================================

/// <summary>
/// 导入SalesPriceScale DTO 验证器
/// </summary>
public class TaktSalesPriceScaleImportValidator : AbstractValidator<TaktSalesPriceScaleImportDto>
{
    /// <summary>
    /// 初始化 导入SalesPriceScale 校验规则
    /// </summary>
    public TaktSalesPriceScaleImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ItemId)
            .GreaterThanOrEqualTo(0).WithMessage("价格明细ID不能为负数");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("销售价格编码不能为空")
            .MaximumLength(50).WithMessage("销售价格编码长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
