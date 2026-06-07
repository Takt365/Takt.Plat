// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNoticeValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：EcNotice 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcNotice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcNotice 验证器
// ========================================

/// <summary>
/// 创建EcNotice DTO 验证器
/// </summary>
public class TaktEcNoticeCreateValidator : AbstractValidator<TaktEcNoticeCreateDto>
{
    /// <summary>
    /// 初始化 创建EcNotice 校验规则
    /// </summary>
    public TaktEcNoticeCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcNoticeNo)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(30).WithMessage("通知单号长度不能超过30个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(30).WithMessage("设变单号长度不能超过30个字符");
        RuleFor(x => x.EcTitle)
            .MaximumLength(500).WithMessage("设变主题长度不能超过500个字符");
        RuleFor(x => x.EcNoticeDeptCodes)
            .MaximumLength(200).WithMessage("通知部门编码长度不能超过200个字符");
        RuleFor(x => x.EcNoticeDeptNames)
            .MaximumLength(500).WithMessage("通知部门名称长度不能超过500个字符");
        RuleFor(x => x.EcNoticeNotifierId)
            .GreaterThanOrEqualTo(0).WithMessage("通知人ID不能为负数");
        RuleFor(x => x.EcNoticeNotifierName)
            .MaximumLength(50).WithMessage("通知人姓名长度不能超过50个字符");
        RuleFor(x => x.EcNoticeConfirmerId)
            .GreaterThanOrEqualTo(0).WithMessage("确认人ID不能为负数");
        RuleFor(x => x.EcNoticeConfirmerName)
            .MaximumLength(50).WithMessage("确认人姓名长度不能超过50个字符");
        RuleFor(x => x.EcNoticeConfirmComment)
            .MaximumLength(1000).WithMessage("确认意见/反馈长度不能超过1000个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例ID不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcNotice 验证器
// ========================================

/// <summary>
/// 更新EcNotice DTO 验证器
/// </summary>
public class TaktEcNoticeUpdateValidator : AbstractValidator<TaktEcNoticeUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcNotice 校验规则
    /// </summary>
    public TaktEcNoticeUpdateValidator()
    {
        RuleFor(x => x.EcNoticeId)
            .GreaterThan(0).WithMessage("EcNoticeID无效");
    }
}

// ========================================
// 导入EcNotice 验证器
// ========================================

/// <summary>
/// 导入EcNotice DTO 验证器
/// </summary>
public class TaktEcNoticeImportValidator : AbstractValidator<TaktEcNoticeImportDto>
{
    /// <summary>
    /// 初始化 导入EcNotice 校验规则
    /// </summary>
    public TaktEcNoticeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcNoticeNo)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(30).WithMessage("通知单号长度不能超过30个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(30).WithMessage("设变单号长度不能超过30个字符");
        RuleFor(x => x.EcTitle)
            .MaximumLength(500).WithMessage("设变主题长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.EcTitle));
        RuleFor(x => x.EcNoticeDeptCodes)
            .MaximumLength(200).WithMessage("通知部门编码长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.EcNoticeDeptCodes));
        RuleFor(x => x.EcNoticeDeptNames)
            .MaximumLength(500).WithMessage("通知部门名称长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.EcNoticeDeptNames));
        RuleFor(x => x.EcNoticeNotifierId)
            .GreaterThanOrEqualTo(0).WithMessage("通知人ID不能为负数");
        RuleFor(x => x.EcNoticeNotifierName)
            .MaximumLength(50).WithMessage("通知人姓名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcNoticeNotifierName));
        RuleFor(x => x.EcNoticeConfirmerId)
            .GreaterThanOrEqualTo(0).WithMessage("确认人ID不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
