// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktPurchasePriceItemValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePriceItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchasePriceItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建PurchasePriceItem 验证器
// ========================================

/// <summary>
/// 创建PurchasePriceItem DTO 验证器
/// </summary>
public class TaktPurchasePriceItemCreateValidator : AbstractValidator<TaktPurchasePriceItemCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchasePriceItem 校验规则
    /// </summary>
    public TaktPurchasePriceItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PurchasePriceId)
            .GreaterThanOrEqualTo(0).WithMessage("采购价格ID不能为负数");
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .MaximumLength(200).WithMessage("物料名称长度不能超过200个字符");
        RuleFor(x => x.MaterialSpecification)
            .MaximumLength(80).WithMessage("物料规格长度不能超过80个字符");
        RuleFor(x => x.PurchaseUnit)
            .NotEmpty().WithMessage("采购单位不能为空")
            .MaximumLength(20).WithMessage("采购单位长度不能超过20个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchasePriceItem 验证器
// ========================================

/// <summary>
/// 更新PurchasePriceItem DTO 验证器
/// </summary>
public class TaktPurchasePriceItemUpdateValidator : AbstractValidator<TaktPurchasePriceItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchasePriceItem 校验规则
    /// </summary>
    public TaktPurchasePriceItemUpdateValidator()
    {
        RuleFor(x => x.PurchasePriceItemId)
            .GreaterThan(0).WithMessage("PurchasePriceItemID无效");
    }
}

// ========================================
// 导入PurchasePriceItem 验证器
// ========================================

/// <summary>
/// 导入PurchasePriceItem DTO 验证器
/// </summary>
public class TaktPurchasePriceItemImportValidator : AbstractValidator<TaktPurchasePriceItemImportDto>
{
    /// <summary>
    /// 初始化 导入PurchasePriceItem 校验规则
    /// </summary>
    public TaktPurchasePriceItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PurchasePriceId)
            .GreaterThanOrEqualTo(0).WithMessage("采购价格ID不能为负数");
        RuleFor(x => x.PurchasePriceCode)
            .NotEmpty().WithMessage("采购价格编码不能为空")
            .MaximumLength(10).WithMessage("采购价格编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .MaximumLength(200).WithMessage("物料名称长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialName));
        RuleFor(x => x.MaterialSpecification)
            .MaximumLength(80).WithMessage("物料规格长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialSpecification));
        RuleFor(x => x.PurchaseUnit)
            .NotEmpty().WithMessage("采购单位不能为空")
            .MaximumLength(20).WithMessage("采购单位长度不能超过20个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
