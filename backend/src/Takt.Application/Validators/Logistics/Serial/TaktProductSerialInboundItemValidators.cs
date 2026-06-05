// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktProductSerialInboundItemValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialInboundItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductSerialInboundItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建ProductSerialInboundItem 验证器
// ========================================

/// <summary>
/// 创建ProductSerialInboundItem DTO 验证器
/// </summary>
public class TaktProductSerialInboundItemCreateValidator : AbstractValidator<TaktProductSerialInboundItemCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductSerialInboundItem 校验规则
    /// </summary>
    public TaktProductSerialInboundItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.InboundId)
            .GreaterThanOrEqualTo(0).WithMessage("入库ID不能为负数");
        RuleFor(x => x.InboundNo)
            .NotEmpty().WithMessage("入库单号不能为空")
            .MaximumLength(50).WithMessage("入库单号长度不能超过50个字符");
        RuleFor(x => x.InboundSerialNo)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductSerialInboundItem 验证器
// ========================================

/// <summary>
/// 更新ProductSerialInboundItem DTO 验证器
/// </summary>
public class TaktProductSerialInboundItemUpdateValidator : AbstractValidator<TaktProductSerialInboundItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductSerialInboundItem 校验规则
    /// </summary>
    public TaktProductSerialInboundItemUpdateValidator()
    {
        RuleFor(x => x.ProductSerialInboundItemId)
            .GreaterThan(0).WithMessage("ProductSerialInboundItemID无效");
    }
}

// ========================================
// 导入ProductSerialInboundItem 验证器
// ========================================

/// <summary>
/// 导入ProductSerialInboundItem DTO 验证器
/// </summary>
public class TaktProductSerialInboundItemImportValidator : AbstractValidator<TaktProductSerialInboundItemImportDto>
{
    /// <summary>
    /// 初始化 导入ProductSerialInboundItem 校验规则
    /// </summary>
    public TaktProductSerialInboundItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.InboundId)
            .GreaterThanOrEqualTo(0).WithMessage("入库ID不能为负数");
        RuleFor(x => x.InboundNo)
            .NotEmpty().WithMessage("入库单号不能为空")
            .MaximumLength(50).WithMessage("入库单号长度不能超过50个字符");
        RuleFor(x => x.InboundSerialNo)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
