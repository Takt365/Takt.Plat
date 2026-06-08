// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.TrainingDevelopment
// 文件名称：TaktCareerDevelopmentValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：CareerDevelopment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCareerDevelopment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.TrainingDevelopment;

namespace Takt.Application.Validators.HumanResource.TrainingDevelopment;

// ========================================
// 创建CareerDevelopment 验证器
// ========================================

/// <summary>
/// 创建CareerDevelopment DTO 验证器
/// </summary>
public class TaktCareerDevelopmentCreateValidator : AbstractValidator<TaktCareerDevelopmentCreateDto>
{
    /// <summary>
    /// 初始化 创建CareerDevelopment 校验规则
    /// </summary>
    public TaktCareerDevelopmentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(50).WithMessage("员工姓名长度不能超过50个字符");
        RuleFor(x => x.SkillCategory)
            .NotEmpty().WithMessage("技能类别不能为空")
            .MaximumLength(50).WithMessage("技能类别长度不能超过50个字符");
        RuleFor(x => x.SkillName)
            .NotEmpty().WithMessage("技能名称不能为空")
            .MaximumLength(100).WithMessage("技能名称长度不能超过100个字符");
        RuleFor(x => x.AssessmentMethod)
            .NotEmpty().WithMessage("评估方式不能为空")
            .MaximumLength(50).WithMessage("评估方式长度不能超过50个字符");
        RuleFor(x => x.SkillLevel)
            .NotEmpty().WithMessage("技能等级不能为空")
            .MaximumLength(50).WithMessage("技能等级长度不能超过50个字符");
        RuleFor(x => x.TargetPosition)
            .NotEmpty().WithMessage("目标岗位不能为空")
            .MaximumLength(100).WithMessage("目标岗位长度不能超过100个字符");
        RuleFor(x => x.DevelopmentPlan)
            .NotEmpty().WithMessage("发展计划不能为空")
            .MaximumLength(1000).WithMessage("发展计划长度不能超过1000个字符");
        RuleFor(x => x.ImprovementSuggestions)
            .NotEmpty().WithMessage("改进建议不能为空")
            .MaximumLength(500).WithMessage("改进建议长度不能超过500个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CareerDevelopment 验证器
// ========================================

/// <summary>
/// 更新CareerDevelopment DTO 验证器
/// </summary>
public class TaktCareerDevelopmentUpdateValidator : AbstractValidator<TaktCareerDevelopmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新CareerDevelopment 校验规则
    /// </summary>
    public TaktCareerDevelopmentUpdateValidator()
    {
        RuleFor(x => x.CareerDevelopmentId)
            .GreaterThan(0).WithMessage("CareerDevelopmentID无效");
    }
}

// ========================================
// 导入CareerDevelopment 验证器
// ========================================

/// <summary>
/// 导入CareerDevelopment DTO 验证器
/// </summary>
public class TaktCareerDevelopmentImportValidator : AbstractValidator<TaktCareerDevelopmentImportDto>
{
    /// <summary>
    /// 初始化 导入CareerDevelopment 校验规则
    /// </summary>
    public TaktCareerDevelopmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(50).WithMessage("员工姓名长度不能超过50个字符");
        RuleFor(x => x.SkillCategory)
            .NotEmpty().WithMessage("技能类别不能为空")
            .MaximumLength(50).WithMessage("技能类别长度不能超过50个字符");
        RuleFor(x => x.SkillName)
            .NotEmpty().WithMessage("技能名称不能为空")
            .MaximumLength(100).WithMessage("技能名称长度不能超过100个字符");
        RuleFor(x => x.AssessmentMethod)
            .NotEmpty().WithMessage("评估方式不能为空")
            .MaximumLength(50).WithMessage("评估方式长度不能超过50个字符");
        RuleFor(x => x.SkillLevel)
            .NotEmpty().WithMessage("技能等级不能为空")
            .MaximumLength(50).WithMessage("技能等级长度不能超过50个字符");
        RuleFor(x => x.TargetPosition)
            .NotEmpty().WithMessage("目标岗位不能为空")
            .MaximumLength(100).WithMessage("目标岗位长度不能超过100个字符");
        RuleFor(x => x.DevelopmentPlan)
            .NotEmpty().WithMessage("发展计划不能为空")
            .MaximumLength(1000).WithMessage("发展计划长度不能超过1000个字符");
        RuleFor(x => x.ImprovementSuggestions)
            .NotEmpty().WithMessage("改进建议不能为空")
            .MaximumLength(500).WithMessage("改进建议长度不能超过500个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
