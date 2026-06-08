// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktAnalysisImprovementValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：AnalysisImprovement 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAnalysisImprovement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建AnalysisImprovement 验证器
// ========================================

/// <summary>
/// 创建AnalysisImprovement DTO 验证器
/// </summary>
public class TaktAnalysisImprovementCreateValidator : AbstractValidator<TaktAnalysisImprovementCreateDto>
{
    /// <summary>
    /// 初始化 创建AnalysisImprovement 校验规则
    /// </summary>
    public TaktAnalysisImprovementCreateValidator()
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
        RuleFor(x => x.AssessmentId)
            .GreaterThanOrEqualTo(0).WithMessage("关联考核评估 ID不能为负数");
        RuleFor(x => x.PlanTitle)
            .NotEmpty().WithMessage("改进计划标题不能为空")
            .MaximumLength(200).WithMessage("改进计划标题长度不能超过200个字符");
        RuleFor(x => x.ImprovementArea)
            .NotEmpty().WithMessage("改进领域不能为空")
            .MaximumLength(50).WithMessage("改进领域长度不能超过50个字符");
        RuleFor(x => x.CurrentSituation)
            .NotEmpty().WithMessage("当前状况描述不能为空")
            .MaximumLength(1000).WithMessage("当前状况描述长度不能超过1000个字符");
        RuleFor(x => x.ImprovementGoal)
            .NotEmpty().WithMessage("改进目标不能为空")
            .MaximumLength(500).WithMessage("改进目标长度不能超过500个字符");
        RuleFor(x => x.ImprovementActions)
            .NotEmpty().WithMessage("改进措施不能为空")
            .MaximumLength(1000).WithMessage("改进措施长度不能超过1000个字符");
        RuleFor(x => x.ResultDescription)
            .NotEmpty().WithMessage("改进结果说明不能为空")
            .MaximumLength(1000).WithMessage("改进结果说明长度不能超过1000个字符");
        RuleFor(x => x.MentorId)
            .GreaterThanOrEqualTo(0).WithMessage("指导老师 ID不能为负数");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新AnalysisImprovement 验证器
// ========================================

/// <summary>
/// 更新AnalysisImprovement DTO 验证器
/// </summary>
public class TaktAnalysisImprovementUpdateValidator : AbstractValidator<TaktAnalysisImprovementUpdateDto>
{
    /// <summary>
    /// 初始化 更新AnalysisImprovement 校验规则
    /// </summary>
    public TaktAnalysisImprovementUpdateValidator()
    {
        RuleFor(x => x.AnalysisImprovementId)
            .GreaterThan(0).WithMessage("AnalysisImprovementID无效");
    }
}

// ========================================
// 导入AnalysisImprovement 验证器
// ========================================

/// <summary>
/// 导入AnalysisImprovement DTO 验证器
/// </summary>
public class TaktAnalysisImprovementImportValidator : AbstractValidator<TaktAnalysisImprovementImportDto>
{
    /// <summary>
    /// 初始化 导入AnalysisImprovement 校验规则
    /// </summary>
    public TaktAnalysisImprovementImportValidator()
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
        RuleFor(x => x.AssessmentId)
            .GreaterThanOrEqualTo(0).WithMessage("关联考核评估 ID不能为负数");
        RuleFor(x => x.PlanTitle)
            .NotEmpty().WithMessage("改进计划标题不能为空")
            .MaximumLength(200).WithMessage("改进计划标题长度不能超过200个字符");
        RuleFor(x => x.ImprovementArea)
            .NotEmpty().WithMessage("改进领域不能为空")
            .MaximumLength(50).WithMessage("改进领域长度不能超过50个字符");
        RuleFor(x => x.CurrentSituation)
            .NotEmpty().WithMessage("当前状况描述不能为空")
            .MaximumLength(1000).WithMessage("当前状况描述长度不能超过1000个字符");
        RuleFor(x => x.ImprovementGoal)
            .NotEmpty().WithMessage("改进目标不能为空")
            .MaximumLength(500).WithMessage("改进目标长度不能超过500个字符");
        RuleFor(x => x.ImprovementActions)
            .NotEmpty().WithMessage("改进措施不能为空")
            .MaximumLength(1000).WithMessage("改进措施长度不能超过1000个字符");
        RuleFor(x => x.ResultDescription)
            .NotEmpty().WithMessage("改进结果说明不能为空")
            .MaximumLength(1000).WithMessage("改进结果说明长度不能超过1000个字符");
        RuleFor(x => x.MentorId)
            .GreaterThanOrEqualTo(0).WithMessage("指导老师 ID不能为负数");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
