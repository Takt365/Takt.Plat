// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Talent
// 文件名称：TaktTalentInterviewValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentInterview 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTalentInterview 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Talent;

namespace Takt.Application.Validators.HumanResource.Talent;

// ========================================
// 创建TalentInterview 验证器
// ========================================

/// <summary>
/// 创建TalentInterview DTO 验证器
/// </summary>
public class TaktTalentInterviewCreateValidator : AbstractValidator<TaktTalentInterviewCreateDto>
{
    /// <summary>
    /// 初始化 创建TalentInterview 校验规则
    /// </summary>
    public TaktTalentInterviewCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.JobPostingId)
            .GreaterThanOrEqualTo(0).WithMessage("职位发布ID不能为负数");
        RuleFor(x => x.InterviewNo)
            .NotEmpty().WithMessage("面试单号不能为空")
            .MaximumLength(20).WithMessage("面试单号长度不能超过20个字符");
        RuleFor(x => x.InterviewerName)
            .MaximumLength(50).WithMessage("面试官姓名长度不能超过50个字符");
        RuleFor(x => x.CandidateName)
            .NotEmpty().WithMessage("候选人姓名不能为空")
            .MaximumLength(50).WithMessage("候选人姓名长度不能超过50个字符");
        RuleFor(x => x.Mobile)
            .MaximumLength(11).WithMessage("候选人手机长度不能超过11个字符");
        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("候选人邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("候选人邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.InterviewLocation)
            .MaximumLength(200).WithMessage("面试地点长度不能超过200个字符");
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("面试说明长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TalentInterview 验证器
// ========================================

/// <summary>
/// 更新TalentInterview DTO 验证器
/// </summary>
public class TaktTalentInterviewUpdateValidator : AbstractValidator<TaktTalentInterviewUpdateDto>
{
    /// <summary>
    /// 初始化 更新TalentInterview 校验规则
    /// </summary>
    public TaktTalentInterviewUpdateValidator()
    {
        RuleFor(x => x.TalentInterviewId)
            .GreaterThan(0).WithMessage("TalentInterviewID无效");
    }
}

// ========================================
// 导入TalentInterview 验证器
// ========================================

/// <summary>
/// 导入TalentInterview DTO 验证器
/// </summary>
public class TaktTalentInterviewImportValidator : AbstractValidator<TaktTalentInterviewImportDto>
{
    /// <summary>
    /// 初始化 导入TalentInterview 校验规则
    /// </summary>
    public TaktTalentInterviewImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.JobPostingId)
            .GreaterThanOrEqualTo(0).WithMessage("职位发布ID不能为负数");
        RuleFor(x => x.InterviewNo)
            .NotEmpty().WithMessage("面试单号不能为空")
            .MaximumLength(20).WithMessage("面试单号长度不能超过20个字符");
        RuleFor(x => x.InterviewerName)
            .MaximumLength(50).WithMessage("面试官姓名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.InterviewerName));
        RuleFor(x => x.CandidateName)
            .NotEmpty().WithMessage("候选人姓名不能为空")
            .MaximumLength(50).WithMessage("候选人姓名长度不能超过50个字符");
        RuleFor(x => x.Mobile)
            .MaximumLength(11).WithMessage("候选人手机长度不能超过11个字符").When(x => !string.IsNullOrWhiteSpace(x.Mobile));
        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("候选人邮箱长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.Email))
            .EmailAddress().WithMessage("候选人邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.InterviewLocation)
            .MaximumLength(200).WithMessage("面试地点长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.InterviewLocation));
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("面试说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Reason));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
