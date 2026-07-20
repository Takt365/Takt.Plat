// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktSerialSummaryValidators.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialSummary 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSerialSummary 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建SerialSummary 验证器
// ========================================

/// <summary>
/// 创建SerialSummary DTO 验证器
/// </summary>
public class TaktSerialSummaryCreateValidator : AbstractValidator<TaktSerialSummaryCreateDto>
{
    /// <summary>
    /// 初始化 创建SerialSummary 校验规则
    /// </summary>
    public TaktSerialSummaryCreateValidator()
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
        RuleFor(x => x.InboundNo)
            .NotEmpty().WithMessage("入库单号不能为空")
            .MaximumLength(50).WithMessage("入库单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品物料不能为空")
            .MaximumLength(20).WithMessage("产品物料长度不能超过20个字符");
        RuleFor(x => x.InboundSerialNo)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ProductInboundSerialNo)
            .NotEmpty().WithMessage("产品入库序列号不能为空")
            .MaximumLength(100).WithMessage("产品入库序列号长度不能超过100个字符");
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.ShippingInvoiceNo)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(50).WithMessage("发货单号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(40).WithMessage("仕向地长度不能超过40个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(40).WithMessage("目的地港长度不能超过40个字符");
        RuleFor(x => x.OutboundSerialNo)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ProductOutboundSerialNo)
            .NotEmpty().WithMessage("产品出库序列号不能为空")
            .MaximumLength(100).WithMessage("产品出库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SerialSummary 验证器
// ========================================

/// <summary>
/// 更新SerialSummary DTO 验证器
/// </summary>
public class TaktSerialSummaryUpdateValidator : AbstractValidator<TaktSerialSummaryUpdateDto>
{
    /// <summary>
    /// 初始化 更新SerialSummary 校验规则
    /// </summary>
    public TaktSerialSummaryUpdateValidator()
    {
        RuleFor(x => x.SerialSummaryId)
            .GreaterThan(0).WithMessage("SerialSummaryID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.InboundNo)
            .NotEmpty().WithMessage("入库单号不能为空")
            .MaximumLength(50).WithMessage("入库单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品物料不能为空")
            .MaximumLength(20).WithMessage("产品物料长度不能超过20个字符");
        RuleFor(x => x.InboundSerialNo)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ProductInboundSerialNo)
            .NotEmpty().WithMessage("产品入库序列号不能为空")
            .MaximumLength(100).WithMessage("产品入库序列号长度不能超过100个字符");
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.ShippingInvoiceNo)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(50).WithMessage("发货单号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(40).WithMessage("仕向地长度不能超过40个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(40).WithMessage("目的地港长度不能超过40个字符");
        RuleFor(x => x.OutboundSerialNo)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ProductOutboundSerialNo)
            .NotEmpty().WithMessage("产品出库序列号不能为空")
            .MaximumLength(100).WithMessage("产品出库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SerialSummary 验证器
// ========================================

/// <summary>
/// 导入SerialSummary DTO 验证器
/// </summary>
public class TaktSerialSummaryImportValidator : AbstractValidator<TaktSerialSummaryImportDto>
{
    /// <summary>
    /// 初始化 导入SerialSummary 校验规则
    /// </summary>
    public TaktSerialSummaryImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.InboundNo)
            .NotEmpty().WithMessage("入库单号不能为空")
            .MaximumLength(50).WithMessage("入库单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品物料不能为空")
            .MaximumLength(20).WithMessage("产品物料长度不能超过20个字符");
        RuleFor(x => x.InboundSerialNo)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ProductInboundSerialNo)
            .NotEmpty().WithMessage("产品入库序列号不能为空")
            .MaximumLength(100).WithMessage("产品入库序列号长度不能超过100个字符");
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.ShippingInvoiceNo)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(50).WithMessage("发货单号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(40).WithMessage("仕向地长度不能超过40个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(40).WithMessage("目的地港长度不能超过40个字符");
        RuleFor(x => x.OutboundSerialNo)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ProductOutboundSerialNo)
            .NotEmpty().WithMessage("产品出库序列号不能为空")
            .MaximumLength(100).WithMessage("产品出库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
