// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.NewsCenter
// 文件名称：TaktNewsCommentValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：NewsComment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktNewsComment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Routine.NewsCenter;

// ========================================
// 创建NewsComment 验证器
// ========================================

/// <summary>
/// 创建NewsComment DTO 验证器
/// </summary>
public class TaktNewsCommentCreateValidator : AbstractValidator<TaktNewsCommentCreateDto>
{
    /// <summary>
    /// 初始化 创建NewsComment 校验规则
    /// </summary>
    public TaktNewsCommentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.NewsId)
            .GreaterThanOrEqualTo(0).WithMessage("新闻 ID不能为负数");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父评论 ID不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("评论人 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("评论人姓名不能为空")
            .MaximumLength(20).WithMessage("评论人姓名长度不能超过20个字符");
        RuleFor(x => x.UserAvatar)
            .MaximumLength(500).WithMessage("评论人头像 URL长度不能超过500个字符");
        RuleFor(x => x.ReplyToUserId)
            .GreaterThanOrEqualTo(0).WithMessage("被回复人 ID不能为负数");
        RuleFor(x => x.ReplyToUserName)
            .MaximumLength(20).WithMessage("被回复人姓名长度不能超过20个字符");
        RuleFor(x => x.CommentContent)
            .NotEmpty().WithMessage("评论内容不能为空")
            .MaximumLength(2000).WithMessage("评论内容长度不能超过2000个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.CommentStatus)
            .IsInEnum().WithMessage("评论状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新NewsComment 验证器
// ========================================

/// <summary>
/// 更新NewsComment DTO 验证器
/// </summary>
public class TaktNewsCommentUpdateValidator : AbstractValidator<TaktNewsCommentUpdateDto>
{
    /// <summary>
    /// 初始化 更新NewsComment 校验规则
    /// </summary>
    public TaktNewsCommentUpdateValidator()
    {
        RuleFor(x => x.NewsCommentId)
            .GreaterThan(0).WithMessage("NewsCommentID无效");
    }
}

// ========================================
// 导入NewsComment 验证器
// ========================================

/// <summary>
/// 导入NewsComment DTO 验证器
/// </summary>
public class TaktNewsCommentImportValidator : AbstractValidator<TaktNewsCommentImportDto>
{
    /// <summary>
    /// 初始化 导入NewsComment 校验规则
    /// </summary>
    public TaktNewsCommentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.NewsId)
            .GreaterThanOrEqualTo(0).WithMessage("新闻 ID不能为负数");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父评论 ID不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("评论人 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("评论人姓名不能为空")
            .MaximumLength(20).WithMessage("评论人姓名长度不能超过20个字符");
        RuleFor(x => x.UserAvatar)
            .MaximumLength(500).WithMessage("评论人头像 URL长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.UserAvatar));
        RuleFor(x => x.ReplyToUserId)
            .GreaterThanOrEqualTo(0).WithMessage("被回复人 ID不能为负数");
        RuleFor(x => x.ReplyToUserName)
            .MaximumLength(20).WithMessage("被回复人姓名长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ReplyToUserName));
        RuleFor(x => x.CommentContent)
            .NotEmpty().WithMessage("评论内容不能为空")
            .MaximumLength(2000).WithMessage("评论内容长度不能超过2000个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
