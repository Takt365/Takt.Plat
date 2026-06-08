// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeFamilyValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeFamily 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeFamily 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建EmployeeFamily 验证器
// ========================================

/// <summary>
/// 创建EmployeeFamily DTO 验证器
/// </summary>
public class TaktEmployeeFamilyCreateValidator : AbstractValidator<TaktEmployeeFamilyCreateDto>
{
    /// <summary>
    /// 初始化 创建EmployeeFamily 校验规则
    /// </summary>
    public TaktEmployeeFamilyCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工ID不能为负数");
        RuleFor(x => x.MemberName)
            .NotEmpty().WithMessage("成员姓名不能为空")
            .MaximumLength(50).WithMessage("成员姓名长度不能超过50个字符");
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符");
        RuleFor(x => x.WorkUnit)
            .MaximumLength(200).WithMessage("工作单位长度不能超过200个字符");
        RuleFor(x => x.JobTitle)
            .MaximumLength(100).WithMessage("职务长度不能超过100个字符");
        RuleFor(x => x.IsEmergencyContact)
            .IsInEnum().WithMessage("是否紧急联系人无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EmployeeFamily 验证器
// ========================================

/// <summary>
/// 更新EmployeeFamily DTO 验证器
/// </summary>
public class TaktEmployeeFamilyUpdateValidator : AbstractValidator<TaktEmployeeFamilyUpdateDto>
{
    /// <summary>
    /// 初始化 更新EmployeeFamily 校验规则
    /// </summary>
    public TaktEmployeeFamilyUpdateValidator()
    {
        RuleFor(x => x.EmployeeFamilyId)
            .GreaterThan(0).WithMessage("EmployeeFamilyID无效");
    }
}

// ========================================
// 导入EmployeeFamily 验证器
// ========================================

/// <summary>
/// 导入EmployeeFamily DTO 验证器
/// </summary>
public class TaktEmployeeFamilyImportValidator : AbstractValidator<TaktEmployeeFamilyImportDto>
{
    /// <summary>
    /// 初始化 导入EmployeeFamily 校验规则
    /// </summary>
    public TaktEmployeeFamilyImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工ID不能为负数");
        RuleFor(x => x.MemberName)
            .NotEmpty().WithMessage("成员姓名不能为空")
            .MaximumLength(50).WithMessage("成员姓名长度不能超过50个字符");
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        RuleFor(x => x.WorkUnit)
            .MaximumLength(200).WithMessage("工作单位长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkUnit));
        RuleFor(x => x.JobTitle)
            .MaximumLength(100).WithMessage("职务长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.JobTitle));
        RuleFor(x => x.IsEmergencyContact)
            .IsInEnum().WithMessage("是否紧急联系人无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
