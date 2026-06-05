// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktTicketChangeLogValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TicketChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTicketChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

// ========================================
// 创建TicketChangeLog 验证器
// ========================================

/// <summary>
/// 创建TicketChangeLog DTO 验证器
/// </summary>
public class TaktTicketChangeLogCreateValidator : AbstractValidator<TaktTicketChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建TicketChangeLog 校验规则
    /// </summary>
    public TaktTicketChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.TicketId)
            .GreaterThanOrEqualTo(0).WithMessage("工单 ID不能为负数");
        RuleFor(x => x.TicketNo)
            .MaximumLength(50).WithMessage("工单编号长度不能超过50个字符");
        RuleFor(x => x.ChangeSummary)
            .MaximumLength(500).WithMessage("修改工单内容摘要长度不能超过500个字符");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表长度不能超过4000个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(500).WithMessage("变更原因或备注长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TicketChangeLog 验证器
// ========================================

/// <summary>
/// 更新TicketChangeLog DTO 验证器
/// </summary>
public class TaktTicketChangeLogUpdateValidator : AbstractValidator<TaktTicketChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新TicketChangeLog 校验规则
    /// </summary>
    public TaktTicketChangeLogUpdateValidator()
    {
        RuleFor(x => x.TicketChangeLogId)
            .GreaterThan(0).WithMessage("TicketChangeLogID无效");
    }
}
