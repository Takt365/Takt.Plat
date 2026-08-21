// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesPriceItemValidators.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesPriceItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesPriceItem 验证器
// ========================================

/// <summary>
/// 创建SalesPriceItem DTO 验证器
/// </summary>
public class TaktSalesPriceItemCreateValidator : AbstractValidator<TaktSalesPriceItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesPriceItem 校验规则
    /// </summary>
    public TaktSalesPriceItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesPriceId)
            .GreaterThanOrEqualTo(0).WithMessage("销售价格 ID不能为负数");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CalculationType)
            .NotEmpty().WithMessage("计算类型不能为空")
            .MaximumLength(1).WithMessage("计算类型长度不能超过1个字符");
        RuleFor(x => x.ConditionCurrencyCode)
            .NotEmpty().WithMessage("条件货币不能为空")
            .MaximumLength(3).WithMessage("条件货币长度不能超过3个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(5).WithMessage("计量单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesPriceItem 验证器
// ========================================

/// <summary>
/// 更新SalesPriceItem DTO 验证器
/// </summary>
public class TaktSalesPriceItemUpdateValidator : AbstractValidator<TaktSalesPriceItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesPriceItem 校验规则
    /// </summary>
    public TaktSalesPriceItemUpdateValidator()
    {
        RuleFor(x => x.SalesPriceItemId)
            .GreaterThan(0).WithMessage("SalesPriceItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesPriceId)
            .GreaterThanOrEqualTo(0).WithMessage("销售价格 ID不能为负数");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CalculationType)
            .NotEmpty().WithMessage("计算类型不能为空")
            .MaximumLength(1).WithMessage("计算类型长度不能超过1个字符");
        RuleFor(x => x.ConditionCurrencyCode)
            .NotEmpty().WithMessage("条件货币不能为空")
            .MaximumLength(3).WithMessage("条件货币长度不能超过3个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(5).WithMessage("计量单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesPriceItem 验证器
// ========================================

/// <summary>
/// 导入SalesPriceItem DTO 验证器
/// </summary>
public class TaktSalesPriceItemImportValidator : AbstractValidator<TaktSalesPriceItemImportDto>
{
    /// <summary>
    /// 初始化 导入SalesPriceItem 校验规则
    /// </summary>
    public TaktSalesPriceItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesPriceId)
            .GreaterThanOrEqualTo(0).WithMessage("销售价格 ID不能为负数");
        RuleFor(x => x.SalesPriceCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CalculationType)
            .NotEmpty().WithMessage("计算类型不能为空")
            .MaximumLength(1).WithMessage("计算类型长度不能超过1个字符");
        RuleFor(x => x.ConditionCurrencyCode)
            .NotEmpty().WithMessage("条件货币不能为空")
            .MaximumLength(3).WithMessage("条件货币长度不能超过3个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(5).WithMessage("计量单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
