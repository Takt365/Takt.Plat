// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktAssessmentValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：Assessment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAssessment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建Assessment 验证器
// ========================================

/// <summary>
/// 创建Assessment DTO 验证器
/// </summary>
public class TaktAssessmentCreateValidator : AbstractValidator<TaktAssessmentCreateDto>
{
    /// <summary>
    /// 初始化 创建Assessment 校验规则
    /// </summary>
    public TaktAssessmentCreateValidator()
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
        RuleFor(x => x.AssessmentPeriod)
            .NotEmpty().WithMessage("考核周期不能为空")
            .MaximumLength(50).WithMessage("考核周期长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标 ID不能为负数");
        RuleFor(x => x.SelfEvaluationNotes)
            .NotEmpty().WithMessage("自评说明不能为空")
            .MaximumLength(1000).WithMessage("自评说明长度不能超过1000个字符");
        RuleFor(x => x.SupervisorComments)
            .NotEmpty().WithMessage("主管评语不能为空")
            .MaximumLength(1000).WithMessage("主管评语长度不能超过1000个字符");
        RuleFor(x => x.PerformanceGrade)
            .NotEmpty().WithMessage("绩效等级不能为空")
            .MaximumLength(10).WithMessage("绩效等级长度不能超过10个字符");
        RuleFor(x => x.ReviewerId)
            .GreaterThanOrEqualTo(0).WithMessage("评审人 ID不能为负数");
        RuleFor(x => x.InterviewNotes)
            .NotEmpty().WithMessage("面谈记录不能为空")
            .MaximumLength(1000).WithMessage("面谈记录长度不能超过1000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Assessment 验证器
// ========================================

/// <summary>
/// 更新Assessment DTO 验证器
/// </summary>
public class TaktAssessmentUpdateValidator : AbstractValidator<TaktAssessmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新Assessment 校验规则
    /// </summary>
    public TaktAssessmentUpdateValidator()
    {
        RuleFor(x => x.AssessmentId)
            .GreaterThan(0).WithMessage("AssessmentID无效");
    }
}

// ========================================
// 导入Assessment 验证器
// ========================================

/// <summary>
/// 导入Assessment DTO 验证器
/// </summary>
public class TaktAssessmentImportValidator : AbstractValidator<TaktAssessmentImportDto>
{
    /// <summary>
    /// 初始化 导入Assessment 校验规则
    /// </summary>
    public TaktAssessmentImportValidator()
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
        RuleFor(x => x.AssessmentPeriod)
            .NotEmpty().WithMessage("考核周期不能为空")
            .MaximumLength(50).WithMessage("考核周期长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标 ID不能为负数");
        RuleFor(x => x.SelfEvaluationNotes)
            .NotEmpty().WithMessage("自评说明不能为空")
            .MaximumLength(1000).WithMessage("自评说明长度不能超过1000个字符");
        RuleFor(x => x.SupervisorComments)
            .NotEmpty().WithMessage("主管评语不能为空")
            .MaximumLength(1000).WithMessage("主管评语长度不能超过1000个字符");
        RuleFor(x => x.PerformanceGrade)
            .NotEmpty().WithMessage("绩效等级不能为空")
            .MaximumLength(10).WithMessage("绩效等级长度不能超过10个字符");
        RuleFor(x => x.ReviewerId)
            .GreaterThanOrEqualTo(0).WithMessage("评审人 ID不能为负数");
        RuleFor(x => x.InterviewNotes)
            .NotEmpty().WithMessage("面谈记录不能为空")
            .MaximumLength(1000).WithMessage("面谈记录长度不能超过1000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
