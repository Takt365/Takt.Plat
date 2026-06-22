// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktMessageValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Message 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMessage 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Message 验证器
// ========================================

/// <summary>
/// 创建Message DTO 验证器
/// </summary>
public class TaktMessageCreateValidator : AbstractValidator<TaktMessageBatchCreateDto>
{
    /// <summary>
    /// 初始化 创建Message 校验规则
    /// </summary>
    public TaktMessageCreateValidator()
    {
        RuleFor(x => x.FromUserName)
            .NotEmpty().WithMessage("发送者用户名不能为空")
            .MaximumLength(20).WithMessage("发送者用户名长度不能超过20个字符");
        RuleFor(x => x.FromUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发送者用户 ID不能为负数");
        RuleFor(x => x.MessageTitle)
            .MaximumLength(200).WithMessage("消息标题长度不能超过200个字符");
        RuleFor(x => x.MessageContent)
            .NotEmpty().WithMessage("消息内容不能为空");
        RuleFor(x => x.Attachments)
            .MaximumLength(2000).WithMessage("附件路径长度不能超过2000个字符");
    }
}
