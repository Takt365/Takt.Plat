// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktProductSerialOutboundValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialOutbound 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductSerialOutbound 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建ProductSerialOutbound 验证器
// ========================================

/// <summary>
/// 创建ProductSerialOutbound DTO 验证器
/// </summary>
public class TaktProductSerialOutboundCreateValidator : AbstractValidator<TaktProductSerialOutboundCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductSerialOutbound 校验规则
    /// </summary>
    public TaktProductSerialOutboundCreateValidator()
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
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.ShippingInvoiceNo)
            .NotEmpty().WithMessage("出货发票号不能为空")
            .MaximumLength(50).WithMessage("出货发票号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(200).WithMessage("仕向地长度不能超过200个字符");
        RuleFor(x => x.ShippingMethod)
            .NotEmpty().WithMessage("运输方式不能为空")
            .MaximumLength(50).WithMessage("运输方式长度不能超过50个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(200).WithMessage("目的地港长度不能超过200个字符");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空")
            .MaximumLength(50).WithMessage("仓库编码长度不能超过50个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(50).WithMessage("库位编码长度不能超过50个字符");
        RuleFor(x => x.RelatedCompany)
            .NotEmpty().WithMessage("关联公司不能为空")
            .MaximumLength(4).WithMessage("关联公司长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductSerialOutbound 验证器
// ========================================

/// <summary>
/// 更新ProductSerialOutbound DTO 验证器
/// </summary>
public class TaktProductSerialOutboundUpdateValidator : AbstractValidator<TaktProductSerialOutboundUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductSerialOutbound 校验规则
    /// </summary>
    public TaktProductSerialOutboundUpdateValidator()
    {
        RuleFor(x => x.ProductSerialOutboundId)
            .GreaterThan(0).WithMessage("ProductSerialOutboundID无效");
    }
}

// ========================================
// 导入ProductSerialOutbound 验证器
// ========================================

/// <summary>
/// 导入ProductSerialOutbound DTO 验证器
/// </summary>
public class TaktProductSerialOutboundImportValidator : AbstractValidator<TaktProductSerialOutboundImportDto>
{
    /// <summary>
    /// 初始化 导入ProductSerialOutbound 校验规则
    /// </summary>
    public TaktProductSerialOutboundImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.OutboundNo)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(50).WithMessage("出库单号长度不能超过50个字符");
        RuleFor(x => x.ShippingInvoiceNo)
            .NotEmpty().WithMessage("出货发票号不能为空")
            .MaximumLength(50).WithMessage("出货发票号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(200).WithMessage("仕向地长度不能超过200个字符");
        RuleFor(x => x.ShippingMethod)
            .NotEmpty().WithMessage("运输方式不能为空")
            .MaximumLength(50).WithMessage("运输方式长度不能超过50个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(200).WithMessage("目的地港长度不能超过200个字符");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空")
            .MaximumLength(50).WithMessage("仓库编码长度不能超过50个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(50).WithMessage("库位编码长度不能超过50个字符");
        RuleFor(x => x.RelatedCompany)
            .NotEmpty().WithMessage("关联公司不能为空")
            .MaximumLength(4).WithMessage("关联公司长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
