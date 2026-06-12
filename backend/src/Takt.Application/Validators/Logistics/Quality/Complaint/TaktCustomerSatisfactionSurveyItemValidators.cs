// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItemValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerSatisfactionSurveyItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCustomerSatisfactionSurveyItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建CustomerSatisfactionSurveyItem 验证器
// ========================================

/// <summary>
/// 创建CustomerSatisfactionSurveyItem DTO 验证器
/// </summary>
public class TaktCustomerSatisfactionSurveyItemCreateValidator : AbstractValidator<TaktCustomerSatisfactionSurveyItemCreateDto>
{
    /// <summary>
    /// 初始化 创建CustomerSatisfactionSurveyItem 校验规则
    /// </summary>
    public TaktCustomerSatisfactionSurveyItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.SurveyId)
            .GreaterThanOrEqualTo(0).WithMessage("调查表ID不能为负数");
        RuleFor(x => x.CustomerSatisfactionSurveyCode)
            .NotEmpty().WithMessage("调查表编号不能为空")
            .MaximumLength(40).WithMessage("调查表编号长度不能超过40个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("调查项目名称不能为空")
            .MaximumLength(40).WithMessage("调查项目名称长度不能超过40个字符");
        RuleFor(x => x.ItemDescription)
            .MaximumLength(500).WithMessage("调查项目说明长度不能超过500个字符");
        RuleFor(x => x.CustomerFeedback)
            .MaximumLength(1000).WithMessage("客户反馈/意见长度不能超过1000个字符");
        RuleFor(x => x.ImprovementSuggestion)
            .MaximumLength(1000).WithMessage("改进建议长度不能超过1000个字符");
        RuleFor(x => x.FollowUpAction)
            .MaximumLength(1000).WithMessage("跟进措施长度不能超过1000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CustomerSatisfactionSurveyItem 验证器
// ========================================

/// <summary>
/// 更新CustomerSatisfactionSurveyItem DTO 验证器
/// </summary>
public class TaktCustomerSatisfactionSurveyItemUpdateValidator : AbstractValidator<TaktCustomerSatisfactionSurveyItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新CustomerSatisfactionSurveyItem 校验规则
    /// </summary>
    public TaktCustomerSatisfactionSurveyItemUpdateValidator()
    {
        RuleFor(x => x.CustomerSatisfactionSurveyItemId)
            .GreaterThan(0).WithMessage("CustomerSatisfactionSurveyItemID无效");
    }
}

// ========================================
// 导入CustomerSatisfactionSurveyItem 验证器
// ========================================

/// <summary>
/// 导入CustomerSatisfactionSurveyItem DTO 验证器
/// </summary>
public class TaktCustomerSatisfactionSurveyItemImportValidator : AbstractValidator<TaktCustomerSatisfactionSurveyItemImportDto>
{
    /// <summary>
    /// 初始化 导入CustomerSatisfactionSurveyItem 校验规则
    /// </summary>
    public TaktCustomerSatisfactionSurveyItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.SurveyId)
            .GreaterThanOrEqualTo(0).WithMessage("调查表ID不能为负数");
        RuleFor(x => x.CustomerSatisfactionSurveyCode)
            .NotEmpty().WithMessage("调查表编号不能为空")
            .MaximumLength(40).WithMessage("调查表编号长度不能超过40个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("调查项目名称不能为空")
            .MaximumLength(40).WithMessage("调查项目名称长度不能超过40个字符");
        RuleFor(x => x.ItemDescription)
            .MaximumLength(500).WithMessage("调查项目说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.ItemDescription));
        RuleFor(x => x.CustomerFeedback)
            .MaximumLength(1000).WithMessage("客户反馈/意见长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.CustomerFeedback));
        RuleFor(x => x.ImprovementSuggestion)
            .MaximumLength(1000).WithMessage("改进建议长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.ImprovementSuggestion));
        RuleFor(x => x.FollowUpAction)
            .MaximumLength(1000).WithMessage("跟进措施长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.FollowUpAction));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
