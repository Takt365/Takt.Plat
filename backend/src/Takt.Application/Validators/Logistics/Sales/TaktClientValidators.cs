// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktClientValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Client 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktClient 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建Client 验证器
// ========================================

/// <summary>
/// 创建Client DTO 验证器
/// </summary>
public class TaktClientCreateValidator : AbstractValidator<TaktClientCreateDto>
{
    /// <summary>
    /// 初始化 创建Client 校验规则
    /// </summary>
    public TaktClientCreateValidator()
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
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ClientShortName)
            .MaximumLength(40).WithMessage("客户端简称长度不能超过40个字符");
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符");
        RuleFor(x => x.ClientTaxNumber)
            .MaximumLength(50).WithMessage("客户端标识长度不能超过50个字符");
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符");
        RuleFor(x => x.ClientPhone)
            .MaximumLength(50).WithMessage("客户端电话长度不能超过50个字符");
        RuleFor(x => x.ClientFax)
            .MaximumLength(50).WithMessage("客户端传真长度不能超过50个字符");
        RuleFor(x => x.ClientEmail)
            .MaximumLength(100).WithMessage("客户端邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("客户端邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ClientEmail));
        RuleFor(x => x.ClientWebsite)
            .MaximumLength(200).WithMessage("客户端网站长度不能超过200个字符");
        RuleFor(x => x.ContactPerson)
            .MaximumLength(50).WithMessage("联系人长度不能超过50个字符");
        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("联系人电话长度不能超过50个字符");
        RuleFor(x => x.ContactEmail)
            .MaximumLength(100).WithMessage("联系人邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系人邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(10).WithMessage("结算币种代码长度不能超过10个字符");
        RuleFor(x => x.PlatformName)
            .MaximumLength(100).WithMessage("平台名称长度不能超过100个字符");
        RuleFor(x => x.StoreName)
            .MaximumLength(100).WithMessage("店铺名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Client 验证器
// ========================================

/// <summary>
/// 更新Client DTO 验证器
/// </summary>
public class TaktClientUpdateValidator : AbstractValidator<TaktClientUpdateDto>
{
    /// <summary>
    /// 初始化 更新Client 校验规则
    /// </summary>
    public TaktClientUpdateValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientID无效");
    }
}

// ========================================
// 导入Client 验证器
// ========================================

/// <summary>
/// 导入Client DTO 验证器
/// </summary>
public class TaktClientImportValidator : AbstractValidator<TaktClientImportDto>
{
    /// <summary>
    /// 初始化 导入Client 校验规则
    /// </summary>
    public TaktClientImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ClientShortName)
            .MaximumLength(40).WithMessage("客户端简称长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.ClientShortName));
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.IndustrySector));
        RuleFor(x => x.ClientTaxNumber)
            .MaximumLength(50).WithMessage("客户端标识长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ClientTaxNumber));
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationCountry));
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress1));
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress2));
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress3));
        RuleFor(x => x.ClientPhone)
            .MaximumLength(50).WithMessage("客户端电话长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ClientPhone));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
