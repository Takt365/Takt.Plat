// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgendaValidators.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：ConferenceAgenda 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktConferenceAgenda 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.ConferenceCenter;

namespace Takt.Application.Validators.Routine.ConferenceCenter;

// ========================================
// 创建ConferenceAgenda 验证器
// ========================================

/// <summary>
/// 创建ConferenceAgenda DTO 验证器
/// </summary>
public class TaktConferenceAgendaCreateValidator : AbstractValidator<TaktConferenceAgendaCreateDto>
{
    /// <summary>
    /// 初始化 创建ConferenceAgenda 校验规则
    /// </summary>
    public TaktConferenceAgendaCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ConferenceId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.ConferenceAgendaTitle)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(200).WithMessage("标题长度不能超过200个字符");
        RuleFor(x => x.PresenterId)
            .GreaterThanOrEqualTo(0).WithMessage("主讲人/汇报人 ID不能为负数");
        RuleFor(x => x.RecorderId)
            .GreaterThanOrEqualTo(0).WithMessage("记录人 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ConferenceAgenda 验证器
// ========================================

/// <summary>
/// 更新ConferenceAgenda DTO 验证器
/// </summary>
public class TaktConferenceAgendaUpdateValidator : AbstractValidator<TaktConferenceAgendaUpdateDto>
{
    /// <summary>
    /// 初始化 更新ConferenceAgenda 校验规则
    /// </summary>
    public TaktConferenceAgendaUpdateValidator()
    {
        RuleFor(x => x.ConferenceAgendaId)
            .GreaterThan(0).WithMessage("ConferenceAgendaID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ConferenceId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.ConferenceAgendaTitle)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(200).WithMessage("标题长度不能超过200个字符");
        RuleFor(x => x.PresenterId)
            .GreaterThanOrEqualTo(0).WithMessage("主讲人/汇报人 ID不能为负数");
        RuleFor(x => x.RecorderId)
            .GreaterThanOrEqualTo(0).WithMessage("记录人 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ConferenceAgenda 验证器
// ========================================

/// <summary>
/// 导入ConferenceAgenda DTO 验证器
/// </summary>
public class TaktConferenceAgendaImportValidator : AbstractValidator<TaktConferenceAgendaImportDto>
{
    /// <summary>
    /// 初始化 导入ConferenceAgenda 校验规则
    /// </summary>
    public TaktConferenceAgendaImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ConferenceId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.ConferenceAgendaTitle)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(200).WithMessage("标题长度不能超过200个字符");
        RuleFor(x => x.PresenterId)
            .GreaterThanOrEqualTo(0).WithMessage("主讲人/汇报人 ID不能为负数");
        RuleFor(x => x.RecorderId)
            .GreaterThanOrEqualTo(0).WithMessage("记录人 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
