// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktManufacturerValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Manufacturer 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktManufacturer 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建Manufacturer 验证器
// ========================================

/// <summary>
/// 创建Manufacturer DTO 验证器
/// </summary>
public class TaktManufacturerCreateValidator : AbstractValidator<TaktManufacturerCreateDto>
{
    /// <summary>
    /// 初始化 创建Manufacturer 校验规则
    /// </summary>
    public TaktManufacturerCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ManufacturerCode)
            .NotEmpty().WithMessage("制造商编码不能为空")
            .MaximumLength(20).WithMessage("制造商编码长度不能超过20个字符");
        RuleFor(x => x.ManufacturerName)
            .NotEmpty().WithMessage("制造商名称不能为空")
            .MaximumLength(80).WithMessage("制造商名称长度不能超过80个字符");
        RuleFor(x => x.ManufacturerShortName)
            .MaximumLength(40).WithMessage("制造商简称长度不能超过40个字符");
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符");
        RuleFor(x => x.ManufacturerTaxNumber)
            .MaximumLength(50).WithMessage("制造商标识长度不能超过50个字符");
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符");
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符");
        RuleFor(x => x.ManufacturerPhone)
            .MaximumLength(50).WithMessage("制造商电话长度不能超过50个字符");
        RuleFor(x => x.ManufacturerFax)
            .MaximumLength(50).WithMessage("制造商传真长度不能超过50个字符");
        RuleFor(x => x.ManufacturerEmail)
            .MaximumLength(100).WithMessage("制造商邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("制造商邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ManufacturerEmail));
        RuleFor(x => x.ManufacturerWebsite)
            .MaximumLength(200).WithMessage("制造商网站长度不能超过200个字符");
        RuleFor(x => x.ContactPerson)
            .MaximumLength(50).WithMessage("联系人长度不能超过50个字符");
        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("联系人电话长度不能超过50个字符");
        RuleFor(x => x.ContactEmail)
            .MaximumLength(100).WithMessage("联系人邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系人邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Manufacturer 验证器
// ========================================

/// <summary>
/// 更新Manufacturer DTO 验证器
/// </summary>
public class TaktManufacturerUpdateValidator : AbstractValidator<TaktManufacturerUpdateDto>
{
    /// <summary>
    /// 初始化 更新Manufacturer 校验规则
    /// </summary>
    public TaktManufacturerUpdateValidator()
    {
        RuleFor(x => x.ManufacturerId)
            .GreaterThan(0).WithMessage("ManufacturerID无效");
    }
}

// ========================================
// 导入Manufacturer 验证器
// ========================================

/// <summary>
/// 导入Manufacturer DTO 验证器
/// </summary>
public class TaktManufacturerImportValidator : AbstractValidator<TaktManufacturerImportDto>
{
    /// <summary>
    /// 初始化 导入Manufacturer 校验规则
    /// </summary>
    public TaktManufacturerImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ManufacturerCode)
            .NotEmpty().WithMessage("制造商编码不能为空")
            .MaximumLength(20).WithMessage("制造商编码长度不能超过20个字符");
        RuleFor(x => x.ManufacturerName)
            .NotEmpty().WithMessage("制造商名称不能为空")
            .MaximumLength(80).WithMessage("制造商名称长度不能超过80个字符");
        RuleFor(x => x.ManufacturerShortName)
            .MaximumLength(40).WithMessage("制造商简称长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.ManufacturerShortName));
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.IndustrySector));
        RuleFor(x => x.ManufacturerTaxNumber)
            .MaximumLength(50).WithMessage("制造商标识长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ManufacturerTaxNumber));
        RuleFor(x => x.RegistrationCountry)
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationCountry));
        RuleFor(x => x.RegistrationAddress1)
            .MaximumLength(80).WithMessage("注册地址1长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress1));
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(80).WithMessage("注册地址2长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress2));
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(80).WithMessage("注册地址3长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress3));
        RuleFor(x => x.ManufacturerPhone)
            .MaximumLength(50).WithMessage("制造商电话长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ManufacturerPhone));
        RuleFor(x => x.ManufacturerFax)
            .MaximumLength(50).WithMessage("制造商传真长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ManufacturerFax));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
