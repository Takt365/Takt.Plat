// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.ConferenceCenter
// 文件名称：TaktConferenceParticipantValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：ConferenceParticipant 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktConferenceParticipant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.ConferenceCenter;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Routine.ConferenceCenter;

// ========================================
// 创建ConferenceParticipant 验证器
// ========================================

/// <summary>
/// 创建ConferenceParticipant DTO 验证器
/// </summary>
public class TaktConferenceParticipantCreateValidator : AbstractValidator<TaktConferenceParticipantCreateDto>
{
    /// <summary>
    /// 初始化 创建ConferenceParticipant 校验规则
    /// </summary>
    public TaktConferenceParticipantCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ConferenceId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户姓名不能为空")
            .MaximumLength(20).WithMessage("用户姓名长度不能超过20个字符");
        RuleFor(x => x.ParticipantRole)
            .IsInEnum().WithMessage("参与角色无效");
        RuleFor(x => x.AttendanceStatus)
            .IsInEnum().WithMessage("出席状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ConferenceParticipant 验证器
// ========================================

/// <summary>
/// 更新ConferenceParticipant DTO 验证器
/// </summary>
public class TaktConferenceParticipantUpdateValidator : AbstractValidator<TaktConferenceParticipantUpdateDto>
{
    /// <summary>
    /// 初始化 更新ConferenceParticipant 校验规则
    /// </summary>
    public TaktConferenceParticipantUpdateValidator()
    {
        RuleFor(x => x.ConferenceParticipantId)
            .GreaterThan(0).WithMessage("ConferenceParticipantID无效");
    }
}

// ========================================
// 导入ConferenceParticipant 验证器
// ========================================

/// <summary>
/// 导入ConferenceParticipant DTO 验证器
/// </summary>
public class TaktConferenceParticipantImportValidator : AbstractValidator<TaktConferenceParticipantImportDto>
{
    /// <summary>
    /// 初始化 导入ConferenceParticipant 校验规则
    /// </summary>
    public TaktConferenceParticipantImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ConferenceId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户姓名不能为空")
            .MaximumLength(20).WithMessage("用户姓名长度不能超过20个字符");
        RuleFor(x => x.ParticipantRole)
            .IsInEnum().WithMessage("参与角色无效");
        RuleFor(x => x.AttendanceStatus)
            .IsInEnum().WithMessage("出席状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
