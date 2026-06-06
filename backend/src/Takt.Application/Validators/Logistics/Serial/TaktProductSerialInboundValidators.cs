// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktProductSerialInboundValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialInbound 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductSerialInbound 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建ProductSerialInbound 验证器
// ========================================

/// <summary>
/// 创建ProductSerialInbound DTO 验证器
/// </summary>
public class TaktProductSerialInboundCreateValidator : AbstractValidator<TaktProductSerialInboundCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductSerialInbound 校验规则
    /// </summary>
    public TaktProductSerialInboundCreateValidator()
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
// 更新ProductSerialInbound 验证器
// ========================================

/// <summary>
/// 更新ProductSerialInbound DTO 验证器
/// </summary>
public class TaktProductSerialInboundUpdateValidator : AbstractValidator<TaktProductSerialInboundUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductSerialInbound 校验规则
    /// </summary>
    public TaktProductSerialInboundUpdateValidator()
    {
        RuleFor(x => x.ProductSerialInboundId)
            .GreaterThan(0).WithMessage("ProductSerialInboundID无效");
    }
}

// ========================================
// 导入ProductSerialInbound 验证器
// ========================================

/// <summary>
/// 导入ProductSerialInbound DTO 验证器
/// </summary>
public class TaktProductSerialInboundImportValidator : AbstractValidator<TaktProductSerialInboundImportDto>
{
    /// <summary>
    /// 初始化 导入ProductSerialInbound 校验规则
    /// </summary>
    public TaktProductSerialInboundImportValidator()
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
