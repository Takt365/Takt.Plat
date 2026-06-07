// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktTicketEvaluationValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TicketEvaluation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTicketEvaluation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

// ========================================
// 创建TicketEvaluation 验证器
// ========================================

/// <summary>
/// 创建TicketEvaluation DTO 验证器
/// </summary>
public class TaktTicketEvaluationCreateValidator : AbstractValidator<TaktTicketEvaluationCreateDto>
{
    /// <summary>
    /// 初始化 创建TicketEvaluation 校验规则
    /// </summary>
    public TaktTicketEvaluationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.TicketId)
            .GreaterThanOrEqualTo(0).WithMessage("工单 ID不能为负数");
        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("评价内容长度不能超过1000个字符");
        RuleFor(x => x.EvaluatorId)
            .GreaterThanOrEqualTo(0).WithMessage("评价人 ID不能为负数");
        RuleFor(x => x.EvaluatorName)
            .MaximumLength(20).WithMessage("评价人姓名长度不能超过20个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TicketEvaluation 验证器
// ========================================

/// <summary>
/// 更新TicketEvaluation DTO 验证器
/// </summary>
public class TaktTicketEvaluationUpdateValidator : AbstractValidator<TaktTicketEvaluationUpdateDto>
{
    /// <summary>
    /// 初始化 更新TicketEvaluation 校验规则
    /// </summary>
    public TaktTicketEvaluationUpdateValidator()
    {
        RuleFor(x => x.TicketEvaluationId)
            .GreaterThan(0).WithMessage("TicketEvaluationID无效");
    }
}

// ========================================
// 导入TicketEvaluation 验证器
// ========================================

/// <summary>
/// 导入TicketEvaluation DTO 验证器
/// </summary>
public class TaktTicketEvaluationImportValidator : AbstractValidator<TaktTicketEvaluationImportDto>
{
    /// <summary>
    /// 初始化 导入TicketEvaluation 校验规则
    /// </summary>
    public TaktTicketEvaluationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.TicketId)
            .GreaterThanOrEqualTo(0).WithMessage("工单 ID不能为负数");
        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("评价内容长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.Comment));
        RuleFor(x => x.EvaluatorId)
            .GreaterThanOrEqualTo(0).WithMessage("评价人 ID不能为负数");
        RuleFor(x => x.EvaluatorName)
            .MaximumLength(20).WithMessage("评价人姓名长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.EvaluatorName));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
