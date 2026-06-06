// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktCompanyValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Company 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCompany 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建Company 验证器
// ========================================

/// <summary>
/// 创建Company DTO 验证器
/// </summary>
public class TaktCompanyCreateValidator : AbstractValidator<TaktCompanyCreateDto>
{
    /// <summary>
    /// 初始化 创建Company 校验规则
    /// </summary>
    public TaktCompanyCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("公司名称不能为空")
            .MaximumLength(200).WithMessage("公司名称长度不能超过200个字符");
        RuleFor(x => x.CompanyShortName)
            .NotEmpty().WithMessage("公司简称不能为空")
            .MaximumLength(50).WithMessage("公司简称长度不能超过50个字符");
        RuleFor(x => x.CompanyType)
            .IsInEnum().WithMessage("公司类型无效");
        RuleFor(x => x.EnterpriseNature)
            .IsInEnum().WithMessage("企业性质无效");
        RuleFor(x => x.IndustryAttribute)
            .IsInEnum().WithMessage("行业属性无效");
        RuleFor(x => x.EnterpriseScale)
            .IsInEnum().WithMessage("企业规模无效");
        RuleFor(x => x.BusinessScope)
            .NotEmpty().WithMessage("经营范围不能为空");
        RuleFor(x => x.RegistrationAddress1)
            .NotEmpty().WithMessage("注册地址1不能为空")
            .MaximumLength(200).WithMessage("注册地址1长度不能超过200个字符");
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(200).WithMessage("注册地址2长度不能超过200个字符");
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(200).WithMessage("注册地址3长度不能超过200个字符");
        RuleFor(x => x.RegistrationRegion)
            .NotEmpty().WithMessage("注册国家不能为空")
            .MaximumLength(50).WithMessage("注册国家长度不能超过50个字符");
        RuleFor(x => x.RegistrationProvince)
            .NotEmpty().WithMessage("注册省不能为空")
            .MaximumLength(50).WithMessage("注册省长度不能超过50个字符");
        RuleFor(x => x.RegistrationCity)
            .NotEmpty().WithMessage("注册市不能为空")
            .MaximumLength(50).WithMessage("注册市长度不能超过50个字符");
        RuleFor(x => x.BusinessRegion)
            .NotEmpty().WithMessage("经营国家不能为空")
            .MaximumLength(50).WithMessage("经营国家长度不能超过50个字符");
        RuleFor(x => x.BusinessProvince)
            .NotEmpty().WithMessage("经营地区-省不能为空")
            .MaximumLength(50).WithMessage("经营地区-省长度不能超过50个字符");
        RuleFor(x => x.BusinessCity)
            .NotEmpty().WithMessage("经营地区-市不能为空")
            .MaximumLength(50).WithMessage("经营地区-市长度不能超过50个字符");
        RuleFor(x => x.BusinessAddress1)
            .NotEmpty().WithMessage("经营地址1不能为空")
            .MaximumLength(200).WithMessage("经营地址1长度不能超过200个字符");
        RuleFor(x => x.BusinessAddress2)
            .MaximumLength(200).WithMessage("经营地址2长度不能超过200个字符");
        RuleFor(x => x.BusinessAddress3)
            .MaximumLength(200).WithMessage("经营地址3长度不能超过200个字符");
        RuleFor(x => x.CompanyPhone)
            .NotEmpty().WithMessage("公司电话不能为空")
            .MaximumLength(50).WithMessage("公司电话长度不能超过50个字符");
        RuleFor(x => x.CompanyEmail)
            .NotEmpty().WithMessage("公司邮箱不能为空")
            .MaximumLength(100).WithMessage("公司邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("公司邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.CompanyEmail));
        RuleFor(x => x.CompanyFax)
            .NotEmpty().WithMessage("公司传真不能为空")
            .MaximumLength(50).WithMessage("公司传真长度不能超过50个字符");
        RuleFor(x => x.CompanyWebsite)
            .NotEmpty().WithMessage("公司网站不能为空")
            .MaximumLength(200).WithMessage("公司网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.CompanyManager)
            .NotEmpty().WithMessage("公司负责人不能为空")
            .MaximumLength(50).WithMessage("公司负责人长度不能超过50个字符");
        RuleFor(x => x.CompanyExistence)
            .IsInEnum().WithMessage("存续状态无效");
        RuleFor(x => x.RelatedPlant)
            .NotEmpty().WithMessage("关联工厂编码不能为空")
            .MaximumLength(4).WithMessage("关联工厂编码长度不能超过4个字符");
        RuleFor(x => x.DefaultCulture)
            .NotEmpty().WithMessage("默认区域文化编码不能为空")
            .MaximumLength(5).WithMessage("默认区域文化编码长度不能超过5个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
        RuleFor(x => x.CompanyStatus)
            .IsInEnum().WithMessage("公司状态无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Company 验证器
// ========================================

/// <summary>
/// 更新Company DTO 验证器
/// </summary>
public class TaktCompanyUpdateValidator : AbstractValidator<TaktCompanyUpdateDto>
{
    /// <summary>
    /// 初始化 更新Company 校验规则
    /// </summary>
    public TaktCompanyUpdateValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0).WithMessage("CompanyID无效");
    }
}

// ========================================
// 导入Company 验证器
// ========================================

/// <summary>
/// 导入Company DTO 验证器
/// </summary>
public class TaktCompanyImportValidator : AbstractValidator<TaktCompanyImportDto>
{
    /// <summary>
    /// 初始化 导入Company 校验规则
    /// </summary>
    public TaktCompanyImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("公司名称不能为空")
            .MaximumLength(200).WithMessage("公司名称长度不能超过200个字符");
        RuleFor(x => x.CompanyShortName)
            .NotEmpty().WithMessage("公司简称不能为空")
            .MaximumLength(50).WithMessage("公司简称长度不能超过50个字符");
        RuleFor(x => x.CompanyType)
            .IsInEnum().WithMessage("公司类型无效");
        RuleFor(x => x.EnterpriseNature)
            .IsInEnum().WithMessage("企业性质无效");
        RuleFor(x => x.IndustryAttribute)
            .IsInEnum().WithMessage("行业属性无效");
        RuleFor(x => x.EnterpriseScale)
            .IsInEnum().WithMessage("企业规模无效");
        RuleFor(x => x.BusinessScope)
            .NotEmpty().WithMessage("经营范围不能为空");
        RuleFor(x => x.RegistrationAddress1)
            .NotEmpty().WithMessage("注册地址1不能为空")
            .MaximumLength(200).WithMessage("注册地址1长度不能超过200个字符");
        RuleFor(x => x.RegistrationAddress2)
            .MaximumLength(200).WithMessage("注册地址2长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress2));
        RuleFor(x => x.RegistrationAddress3)
            .MaximumLength(200).WithMessage("注册地址3长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.RegistrationAddress3));
        RuleFor(x => x.RegistrationRegion)
            .NotEmpty().WithMessage("注册国家不能为空")
            .MaximumLength(50).WithMessage("注册国家长度不能超过50个字符");
        RuleFor(x => x.RegistrationProvince)
            .NotEmpty().WithMessage("注册省不能为空")
            .MaximumLength(50).WithMessage("注册省长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
