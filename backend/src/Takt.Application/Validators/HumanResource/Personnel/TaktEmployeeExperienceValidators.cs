// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeExperienceValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeExperience 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeExperience 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建EmployeeExperience 验证器
// ========================================

/// <summary>
/// 创建EmployeeExperience DTO 验证器
/// </summary>
public class TaktEmployeeExperienceCreateValidator : AbstractValidator<TaktEmployeeExperienceCreateDto>
{
    /// <summary>
    /// 初始化 创建EmployeeExperience 校验规则
    /// </summary>
    public TaktEmployeeExperienceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工ID不能为负数");
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("工作单位名称不能为空")
            .MaximumLength(200).WithMessage("工作单位名称长度不能超过200个字符");
        RuleFor(x => x.PositionName)
            .MaximumLength(100).WithMessage("职位名称长度不能超过100个字符");
        RuleFor(x => x.JobContent)
            .MaximumLength(2000).WithMessage("工作内容长度不能超过2000个字符");
        RuleFor(x => x.WitnessName)
            .MaximumLength(50).WithMessage("证明人姓名长度不能超过50个字符");
        RuleFor(x => x.WitnessPhone)
            .MaximumLength(20).WithMessage("证明人电话长度不能超过20个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EmployeeExperience 验证器
// ========================================

/// <summary>
/// 更新EmployeeExperience DTO 验证器
/// </summary>
public class TaktEmployeeExperienceUpdateValidator : AbstractValidator<TaktEmployeeExperienceUpdateDto>
{
    /// <summary>
    /// 初始化 更新EmployeeExperience 校验规则
    /// </summary>
    public TaktEmployeeExperienceUpdateValidator()
    {
        RuleFor(x => x.EmployeeExperienceId)
            .GreaterThan(0).WithMessage("EmployeeExperienceID无效");
    }
}

// ========================================
// 导入EmployeeExperience 验证器
// ========================================

/// <summary>
/// 导入EmployeeExperience DTO 验证器
/// </summary>
public class TaktEmployeeExperienceImportValidator : AbstractValidator<TaktEmployeeExperienceImportDto>
{
    /// <summary>
    /// 初始化 导入EmployeeExperience 校验规则
    /// </summary>
    public TaktEmployeeExperienceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工ID不能为负数");
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("工作单位名称不能为空")
            .MaximumLength(200).WithMessage("工作单位名称长度不能超过200个字符");
        RuleFor(x => x.PositionName)
            .MaximumLength(100).WithMessage("职位名称长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.PositionName));
        RuleFor(x => x.JobContent)
            .MaximumLength(2000).WithMessage("工作内容长度不能超过2000个字符").When(x => !string.IsNullOrWhiteSpace(x.JobContent));
        RuleFor(x => x.WitnessName)
            .MaximumLength(50).WithMessage("证明人姓名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.WitnessName));
        RuleFor(x => x.WitnessPhone)
            .MaximumLength(20).WithMessage("证明人电话长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.WitnessPhone));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
