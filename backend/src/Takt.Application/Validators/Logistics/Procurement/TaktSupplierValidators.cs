// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktSupplierValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Supplier 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSupplier 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建Supplier 验证器
// ========================================

/// <summary>
/// 创建Supplier DTO 验证器
/// </summary>
public class TaktSupplierCreateValidator : AbstractValidator<TaktSupplierCreateDto>
{
    /// <summary>
    /// 初始化 创建Supplier 校验规则
    /// </summary>
    public TaktSupplierCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供货商编码不能为空")
            .MaximumLength(40).WithMessage("供货商编码长度不能超过40个字符");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供货商名称不能为空")
            .MaximumLength(40).WithMessage("供货商名称长度不能超过40个字符");
        RuleFor(x => x.SupplierShortName)
            .MaximumLength(40).WithMessage("供货商简称长度不能超过40个字符");
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符");
        RuleFor(x => x.SupplierTaxNumber)
            .MaximumLength(50).WithMessage("供货商标识长度不能超过50个字符");
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符");
        RuleFor(x => x.SupplierPhone)
            .MaximumLength(50).WithMessage("供货商电话长度不能超过50个字符");
        RuleFor(x => x.SupplierFax)
            .MaximumLength(50).WithMessage("供货商传真长度不能超过50个字符");
        RuleFor(x => x.SupplierEmail)
            .MaximumLength(100).WithMessage("供货商邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("供货商邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.SupplierEmail));
        RuleFor(x => x.SupplierWebsite)
            .MaximumLength(200).WithMessage("供货商网站长度不能超过200个字符");
        RuleFor(x => x.ContactPerson)
            .MaximumLength(50).WithMessage("联系人长度不能超过50个字符");
        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("联系人电话长度不能超过50个字符");
        RuleFor(x => x.ContactEmail)
            .MaximumLength(100).WithMessage("联系人邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系人邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种代码不能为空")
            .MaximumLength(40).WithMessage("结算币种代码长度不能超过40个字符");
        RuleFor(x => x.SupplierStatus)
            .IsInEnum().WithMessage("供货商状态无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Supplier 验证器
// ========================================

/// <summary>
/// 更新Supplier DTO 验证器
/// </summary>
public class TaktSupplierUpdateValidator : AbstractValidator<TaktSupplierUpdateDto>
{
    /// <summary>
    /// 初始化 更新Supplier 校验规则
    /// </summary>
    public TaktSupplierUpdateValidator()
    {
        RuleFor(x => x.SupplierId)
            .GreaterThan(0).WithMessage("SupplierID无效");
    }
}

// ========================================
// 导入Supplier 验证器
// ========================================

/// <summary>
/// 导入Supplier DTO 验证器
/// </summary>
public class TaktSupplierImportValidator : AbstractValidator<TaktSupplierImportDto>
{
    /// <summary>
    /// 初始化 导入Supplier 校验规则
    /// </summary>
    public TaktSupplierImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供货商编码不能为空")
            .MaximumLength(40).WithMessage("供货商编码长度不能超过40个字符");
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("供货商名称不能为空")
            .MaximumLength(40).WithMessage("供货商名称长度不能超过40个字符");
        RuleFor(x => x.SupplierShortName)
            .MaximumLength(40).WithMessage("供货商简称长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.SupplierShortName));
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.IndustrySector));
        RuleFor(x => x.SupplierTaxNumber)
            .MaximumLength(50).WithMessage("供货商标识长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SupplierTaxNumber));
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationCountry));
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress1));
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress2));
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress3));
        RuleFor(x => x.SupplierPhone)
            .MaximumLength(50).WithMessage("供货商电话长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SupplierPhone));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
