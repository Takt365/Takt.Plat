// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.ConferenceCenter
// 文件名称：TaktConferenceValidators.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：Conference 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktConference 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.ConferenceCenter;

namespace Takt.Application.Validators.Routine.ConferenceCenter;

// ========================================
// 创建Conference 验证器
// ========================================

/// <summary>
/// 创建Conference DTO 验证器
/// </summary>
public class TaktConferenceCreateValidator : AbstractValidator<TaktConferenceCreateDto>
{
    /// <summary>
    /// 初始化 创建Conference 校验规则
    /// </summary>
    public TaktConferenceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ConferenceCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.ConferenceTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.OrganizerId)
            .GreaterThanOrEqualTo(0).WithMessage("组织人 ID不能为负数");
        RuleFor(x => x.OrganizerName)
            .NotEmpty().WithMessage("组织人姓名不能为空")
            .MaximumLength(20).WithMessage("组织人姓名长度不能超过20个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("主办部门 ID不能为负数");
        RuleFor(x => x.ConferenceRoomId)
            .GreaterThanOrEqualTo(0).WithMessage("会议室 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Conference 验证器
// ========================================

/// <summary>
/// 更新Conference DTO 验证器
/// </summary>
public class TaktConferenceUpdateValidator : AbstractValidator<TaktConferenceUpdateDto>
{
    /// <summary>
    /// 初始化 更新Conference 校验规则
    /// </summary>
    public TaktConferenceUpdateValidator()
    {
        RuleFor(x => x.ConferenceId)
            .GreaterThan(0).WithMessage("ConferenceID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ConferenceCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.ConferenceTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.OrganizerId)
            .GreaterThanOrEqualTo(0).WithMessage("组织人 ID不能为负数");
        RuleFor(x => x.OrganizerName)
            .NotEmpty().WithMessage("组织人姓名不能为空")
            .MaximumLength(20).WithMessage("组织人姓名长度不能超过20个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("主办部门 ID不能为负数");
        RuleFor(x => x.ConferenceRoomId)
            .GreaterThanOrEqualTo(0).WithMessage("会议室 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Conference 验证器
// ========================================

/// <summary>
/// 导入Conference DTO 验证器
/// </summary>
public class TaktConferenceImportValidator : AbstractValidator<TaktConferenceImportDto>
{
    /// <summary>
    /// 初始化 导入Conference 校验规则
    /// </summary>
    public TaktConferenceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ConferenceCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.ConferenceTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.OrganizerId)
            .GreaterThanOrEqualTo(0).WithMessage("组织人 ID不能为负数");
        RuleFor(x => x.OrganizerName)
            .NotEmpty().WithMessage("组织人姓名不能为空")
            .MaximumLength(20).WithMessage("组织人姓名长度不能超过20个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("主办部门 ID不能为负数");
        RuleFor(x => x.ConferenceRoomId)
            .GreaterThanOrEqualTo(0).WithMessage("会议室 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
