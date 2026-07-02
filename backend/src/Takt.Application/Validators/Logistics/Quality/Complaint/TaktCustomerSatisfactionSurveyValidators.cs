// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerSatisfactionSurvey 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCustomerSatisfactionSurvey 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建CustomerSatisfactionSurvey 验证器
// ========================================

/// <summary>
/// 创建CustomerSatisfactionSurvey DTO 验证器
/// </summary>
public class TaktCustomerSatisfactionSurveyCreateValidator : AbstractValidator<TaktCustomerSatisfactionSurveyCreateDto>
{
    /// <summary>
    /// 初始化 创建CustomerSatisfactionSurvey 校验规则
    /// </summary>
    public TaktCustomerSatisfactionSurveyCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CustomerSatisfactionSurveyCode)
            .NotEmpty().WithMessage("调查表编号不能为空")
            .MaximumLength(50).WithMessage("调查表编号长度不能超过50个字符");
        RuleFor(x => x.CustomerId)
            .GreaterThanOrEqualTo(0).WithMessage("客户 ID不能为负数");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(200).WithMessage("客户名称长度不能超过200个字符");
        RuleFor(x => x.RelatedComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("关联客诉 ID不能为负数");
        RuleFor(x => x.RelatedPlant)
            .NotEmpty().WithMessage("关联工厂不能为空")
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CustomerSatisfactionSurvey 验证器
// ========================================

/// <summary>
/// 更新CustomerSatisfactionSurvey DTO 验证器
/// </summary>
public class TaktCustomerSatisfactionSurveyUpdateValidator : AbstractValidator<TaktCustomerSatisfactionSurveyUpdateDto>
{
    /// <summary>
    /// 初始化 更新CustomerSatisfactionSurvey 校验规则
    /// </summary>
    public TaktCustomerSatisfactionSurveyUpdateValidator()
    {
        RuleFor(x => x.CustomerSatisfactionSurveyId)
            .GreaterThan(0).WithMessage("CustomerSatisfactionSurveyID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CustomerSatisfactionSurveyCode)
            .NotEmpty().WithMessage("调查表编号不能为空")
            .MaximumLength(50).WithMessage("调查表编号长度不能超过50个字符");
        RuleFor(x => x.CustomerId)
            .GreaterThanOrEqualTo(0).WithMessage("客户 ID不能为负数");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(200).WithMessage("客户名称长度不能超过200个字符");
        RuleFor(x => x.RelatedComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("关联客诉 ID不能为负数");
        RuleFor(x => x.RelatedPlant)
            .NotEmpty().WithMessage("关联工厂不能为空")
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入CustomerSatisfactionSurvey 验证器
// ========================================

/// <summary>
/// 导入CustomerSatisfactionSurvey DTO 验证器
/// </summary>
public class TaktCustomerSatisfactionSurveyImportValidator : AbstractValidator<TaktCustomerSatisfactionSurveyImportDto>
{
    /// <summary>
    /// 初始化 导入CustomerSatisfactionSurvey 校验规则
    /// </summary>
    public TaktCustomerSatisfactionSurveyImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CustomerSatisfactionSurveyCode)
            .NotEmpty().WithMessage("调查表编号不能为空")
            .MaximumLength(50).WithMessage("调查表编号长度不能超过50个字符");
        RuleFor(x => x.CustomerId)
            .GreaterThanOrEqualTo(0).WithMessage("客户 ID不能为负数");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(200).WithMessage("客户名称长度不能超过200个字符");
        RuleFor(x => x.RelatedComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("关联客诉 ID不能为负数");
        RuleFor(x => x.RelatedPlant)
            .NotEmpty().WithMessage("关联工厂不能为空")
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
