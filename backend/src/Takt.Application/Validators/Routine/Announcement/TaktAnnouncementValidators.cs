// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.Announcement
// 文件名称：TaktAnnouncementValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Announcement 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAnnouncement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.Announcement;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Routine.Announcement;

// ========================================
// 创建Announcement 验证器
// ========================================

/// <summary>
/// 创建Announcement DTO 验证器
/// </summary>
public class TaktAnnouncementCreateValidator : AbstractValidator<TaktAnnouncementCreateDto>
{
    /// <summary>
    /// 初始化 创建Announcement 校验规则
    /// </summary>
    public TaktAnnouncementCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("公告标题不能为空")
            .MaximumLength(200).WithMessage("公告标题长度不能超过200个字符");
        RuleFor(x => x.AnnouncementType)
            .IsInEnum().WithMessage("公告类型无效");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("公告内容不能为空");
        RuleFor(x => x.Summary)
            .MaximumLength(2000).WithMessage("公告摘要长度不能超过2000个字符");
        RuleFor(x => x.Tags)
            .MaximumLength(500).WithMessage("标签长度不能超过500个字符");
        RuleFor(x => x.Attachments)
            .MaximumLength(2000).WithMessage("附件路径长度不能超过2000个字符");
        RuleFor(x => x.IsScheduled)
            .IsInEnum().WithMessage("是否定时发布无效");
        RuleFor(x => x.IsTop)
            .IsInEnum().WithMessage("是否置顶无效");
        RuleFor(x => x.TargetScope)
            .NotEmpty().WithMessage("目标范围不能为空")
            .MaximumLength(20).WithMessage("目标范围长度不能超过20个字符");
        RuleFor(x => x.TargetDepartments)
            .MaximumLength(1000).WithMessage("目标部门编码长度不能超过1000个字符");
        RuleFor(x => x.TargetUsers)
            .MaximumLength(2000).WithMessage("目标用户 ID长度不能超过2000个字符");
        RuleFor(x => x.AnnouncementStatus)
            .IsInEnum().WithMessage("状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Announcement 验证器
// ========================================

/// <summary>
/// 更新Announcement DTO 验证器
/// </summary>
public class TaktAnnouncementUpdateValidator : AbstractValidator<TaktAnnouncementUpdateDto>
{
    /// <summary>
    /// 初始化 更新Announcement 校验规则
    /// </summary>
    public TaktAnnouncementUpdateValidator()
    {
        RuleFor(x => x.AnnouncementId)
            .GreaterThan(0).WithMessage("AnnouncementID无效");
    }
}

// ========================================
// 导入Announcement 验证器
// ========================================

/// <summary>
/// 导入Announcement DTO 验证器
/// </summary>
public class TaktAnnouncementImportValidator : AbstractValidator<TaktAnnouncementImportDto>
{
    /// <summary>
    /// 初始化 导入Announcement 校验规则
    /// </summary>
    public TaktAnnouncementImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("公告标题不能为空")
            .MaximumLength(200).WithMessage("公告标题长度不能超过200个字符");
        RuleFor(x => x.AnnouncementType)
            .IsInEnum().WithMessage("公告类型无效");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("公告内容不能为空");
        RuleFor(x => x.Summary)
            .MaximumLength(2000).WithMessage("公告摘要长度不能超过2000个字符").When(x => !string.IsNullOrWhiteSpace(x.Summary));
        RuleFor(x => x.Tags)
            .MaximumLength(500).WithMessage("标签长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Tags));
        RuleFor(x => x.Attachments)
            .MaximumLength(2000).WithMessage("附件路径长度不能超过2000个字符").When(x => !string.IsNullOrWhiteSpace(x.Attachments));
        RuleFor(x => x.IsScheduled)
            .IsInEnum().WithMessage("是否定时发布无效");
        RuleFor(x => x.IsTop)
            .IsInEnum().WithMessage("是否置顶无效");
        RuleFor(x => x.TargetScope)
            .NotEmpty().WithMessage("目标范围不能为空")
            .MaximumLength(20).WithMessage("目标范围长度不能超过20个字符");
        RuleFor(x => x.TargetDepartments)
            .MaximumLength(1000).WithMessage("目标部门编码长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.TargetDepartments));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
