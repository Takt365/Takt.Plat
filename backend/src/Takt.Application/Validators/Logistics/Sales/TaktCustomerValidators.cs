// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktCustomerValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Customer 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCustomer 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建Customer 验证器
// ========================================

/// <summary>
/// 创建Customer DTO 验证器
/// </summary>
public class TaktCustomerCreateValidator : AbstractValidator<TaktCustomerCreateDto>
{
    /// <summary>
    /// 初始化 创建Customer 校验规则
    /// </summary>
    public TaktCustomerCreateValidator()
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
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(20).WithMessage("客户编码长度不能超过20个字符");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(80).WithMessage("客户名称长度不能超过80个字符");
        RuleFor(x => x.CustomerShortName)
            .MaximumLength(40).WithMessage("客户简称长度不能超过40个字符");
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符");
        RuleFor(x => x.CustomerTaxNumber)
            .MaximumLength(50).WithMessage("客户标识长度不能超过50个字符");
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符");
        RuleFor(x => x.CustomerPhone)
            .MaximumLength(50).WithMessage("客户电话长度不能超过50个字符");
        RuleFor(x => x.CustomerFax)
            .MaximumLength(50).WithMessage("客户传真长度不能超过50个字符");
        RuleFor(x => x.CustomerEmail)
            .MaximumLength(100).WithMessage("客户邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("客户邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.CustomerEmail));
        RuleFor(x => x.CustomerWebsite)
            .MaximumLength(200).WithMessage("客户网站长度不能超过200个字符");
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
        RuleFor(x => x.SalesBy)
            .MaximumLength(50).WithMessage("销售员长度不能超过50个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Customer 验证器
// ========================================

/// <summary>
/// 更新Customer DTO 验证器
/// </summary>
public class TaktCustomerUpdateValidator : AbstractValidator<TaktCustomerUpdateDto>
{
    /// <summary>
    /// 初始化 更新Customer 校验规则
    /// </summary>
    public TaktCustomerUpdateValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("CustomerID无效");
    }
}

// ========================================
// 导入Customer 验证器
// ========================================

/// <summary>
/// 导入Customer DTO 验证器
/// </summary>
public class TaktCustomerImportValidator : AbstractValidator<TaktCustomerImportDto>
{
    /// <summary>
    /// 初始化 导入Customer 校验规则
    /// </summary>
    public TaktCustomerImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(20).WithMessage("客户编码长度不能超过20个字符");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(80).WithMessage("客户名称长度不能超过80个字符");
        RuleFor(x => x.CustomerShortName)
            .MaximumLength(40).WithMessage("客户简称长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CustomerShortName));
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.IndustrySector));
        RuleFor(x => x.CustomerTaxNumber)
            .MaximumLength(50).WithMessage("客户标识长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.CustomerTaxNumber));
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationCountry));
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress1));
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress2));
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress3));
        RuleFor(x => x.CustomerPhone)
            .MaximumLength(50).WithMessage("客户电话长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.CustomerPhone));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
