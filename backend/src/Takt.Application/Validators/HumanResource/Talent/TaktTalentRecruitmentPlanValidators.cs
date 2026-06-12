// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Talent
// 文件名称：TaktTalentRecruitmentPlanValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentRecruitmentPlan 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTalentRecruitmentPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Talent;

namespace Takt.Application.Validators.HumanResource.Talent;

// ========================================
// 创建TalentRecruitmentPlan 验证器
// ========================================

/// <summary>
/// 创建TalentRecruitmentPlan DTO 验证器
/// </summary>
public class TaktTalentRecruitmentPlanCreateValidator : AbstractValidator<TaktTalentRecruitmentPlanCreateDto>
{
    /// <summary>
    /// 初始化 创建TalentRecruitmentPlan 校验规则
    /// </summary>
    public TaktTalentRecruitmentPlanCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.StaffingRequirementId)
            .GreaterThanOrEqualTo(0).WithMessage("用人需求ID不能为负数");
        RuleFor(x => x.PlanNo)
            .NotEmpty().WithMessage("计划单号不能为空")
            .MaximumLength(20).WithMessage("计划单号长度不能超过20个字符");
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("计划说明长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TalentRecruitmentPlan 验证器
// ========================================

/// <summary>
/// 更新TalentRecruitmentPlan DTO 验证器
/// </summary>
public class TaktTalentRecruitmentPlanUpdateValidator : AbstractValidator<TaktTalentRecruitmentPlanUpdateDto>
{
    /// <summary>
    /// 初始化 更新TalentRecruitmentPlan 校验规则
    /// </summary>
    public TaktTalentRecruitmentPlanUpdateValidator()
    {
        RuleFor(x => x.TalentRecruitmentPlanId)
            .GreaterThan(0).WithMessage("TalentRecruitmentPlanID无效");
    }
}

// ========================================
// 导入TalentRecruitmentPlan 验证器
// ========================================

/// <summary>
/// 导入TalentRecruitmentPlan DTO 验证器
/// </summary>
public class TaktTalentRecruitmentPlanImportValidator : AbstractValidator<TaktTalentRecruitmentPlanImportDto>
{
    /// <summary>
    /// 初始化 导入TalentRecruitmentPlan 校验规则
    /// </summary>
    public TaktTalentRecruitmentPlanImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.StaffingRequirementId)
            .GreaterThanOrEqualTo(0).WithMessage("用人需求ID不能为负数");
        RuleFor(x => x.PlanNo)
            .NotEmpty().WithMessage("计划单号不能为空")
            .MaximumLength(20).WithMessage("计划单号长度不能超过20个字符");
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("计划说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Reason));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
