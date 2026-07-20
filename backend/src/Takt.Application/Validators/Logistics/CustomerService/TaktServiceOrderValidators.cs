// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.CustomerService
// 文件名称：TaktServiceOrderValidators.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：ServiceOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktServiceOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.CustomerService;

namespace Takt.Application.Validators.Logistics.CustomerService;

// ========================================
// 创建ServiceOrder 验证器
// ========================================

/// <summary>
/// 创建ServiceOrder DTO 验证器
/// </summary>
public class TaktServiceOrderCreateValidator : AbstractValidator<TaktServiceOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建ServiceOrder 校验规则
    /// </summary>
    public TaktServiceOrderCreateValidator()
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
        RuleFor(x => x.ServiceOrderCode)
            .NotEmpty().WithMessage("服务订单编码不能为空")
            .MaximumLength(50).WithMessage("服务订单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(10).WithMessage("结算币种代码长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ServiceOrder 验证器
// ========================================

/// <summary>
/// 更新ServiceOrder DTO 验证器
/// </summary>
public class TaktServiceOrderUpdateValidator : AbstractValidator<TaktServiceOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新ServiceOrder 校验规则
    /// </summary>
    public TaktServiceOrderUpdateValidator()
    {
        RuleFor(x => x.ServiceOrderId)
            .GreaterThan(0).WithMessage("ServiceOrderID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceOrderCode)
            .NotEmpty().WithMessage("服务订单编码不能为空")
            .MaximumLength(50).WithMessage("服务订单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(10).WithMessage("结算币种代码长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ServiceOrder 验证器
// ========================================

/// <summary>
/// 导入ServiceOrder DTO 验证器
/// </summary>
public class TaktServiceOrderImportValidator : AbstractValidator<TaktServiceOrderImportDto>
{
    /// <summary>
    /// 初始化 导入ServiceOrder 校验规则
    /// </summary>
    public TaktServiceOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceOrderCode)
            .NotEmpty().WithMessage("服务订单编码不能为空")
            .MaximumLength(50).WithMessage("服务订单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(10).WithMessage("结算币种代码长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
