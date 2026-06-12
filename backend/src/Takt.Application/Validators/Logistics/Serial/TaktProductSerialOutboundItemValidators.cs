// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktProductSerialOutboundItemValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialOutboundItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductSerialOutboundItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建ProductSerialOutboundItem 验证器
// ========================================

/// <summary>
/// 创建ProductSerialOutboundItem DTO 验证器
/// </summary>
public class TaktProductSerialOutboundItemCreateValidator : AbstractValidator<TaktProductSerialOutboundItemCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductSerialOutboundItem 校验规则
    /// </summary>
    public TaktProductSerialOutboundItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.OutboundId)
            .GreaterThanOrEqualTo(0).WithMessage("出库ID不能为负数");
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.OutboundSerialNo)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ReferenceInboundId)
            .GreaterThanOrEqualTo(0).WithMessage("关联入库ID不能为负数");
        RuleFor(x => x.ReferenceInboundNo)
            .NotEmpty().WithMessage("关联入库单号不能为空")
            .MaximumLength(50).WithMessage("关联入库单号长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductSerialOutboundItem 验证器
// ========================================

/// <summary>
/// 更新ProductSerialOutboundItem DTO 验证器
/// </summary>
public class TaktProductSerialOutboundItemUpdateValidator : AbstractValidator<TaktProductSerialOutboundItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductSerialOutboundItem 校验规则
    /// </summary>
    public TaktProductSerialOutboundItemUpdateValidator()
    {
        RuleFor(x => x.ProductSerialOutboundItemId)
            .GreaterThan(0).WithMessage("ProductSerialOutboundItemID无效");
    }
}

// ========================================
// 导入ProductSerialOutboundItem 验证器
// ========================================

/// <summary>
/// 导入ProductSerialOutboundItem DTO 验证器
/// </summary>
public class TaktProductSerialOutboundItemImportValidator : AbstractValidator<TaktProductSerialOutboundItemImportDto>
{
    /// <summary>
    /// 初始化 导入ProductSerialOutboundItem 校验规则
    /// </summary>
    public TaktProductSerialOutboundItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.OutboundId)
            .GreaterThanOrEqualTo(0).WithMessage("出库ID不能为负数");
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.OutboundSerialNo)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ReferenceInboundId)
            .GreaterThanOrEqualTo(0).WithMessage("关联入库ID不能为负数");
        RuleFor(x => x.ReferenceInboundNo)
            .NotEmpty().WithMessage("关联入库单号不能为空")
            .MaximumLength(50).WithMessage("关联入库单号长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
