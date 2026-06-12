// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktMessageValidators.cs
// 创建时间：2026-06-09
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
public class TaktMessageCreateValidator : AbstractValidator<TaktMessageCreateDto>
{
    /// <summary>
    /// 初始化 创建Message 校验规则
    /// </summary>
    public TaktMessageCreateValidator()
    {
        RuleFor(x => x.FromUserName)
            .NotEmpty().WithMessage("发送者用户名不能为空")
            .MaximumLength(40).WithMessage("发送者用户名长度不能超过40个字符");
        RuleFor(x => x.FromUserId)
            .GreaterThan(0).WithMessage("发送者用户 ID无效")
            .When(x => string.IsNullOrWhiteSpace(x.FromUserName));
        RuleFor(x => x)
            .Must(x => x.FromUserId > 0 || !string.IsNullOrWhiteSpace(x.FromUserName))
            .WithMessage("发送者用户 ID 与用户名不能同时为空");
        RuleFor(x => x.ToUserName)
            .MaximumLength(40).WithMessage("接收者用户名长度不能超过40个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.ToUserName));
        RuleFor(x => x)
            .Must(x => x.ToUserId > 0 || !string.IsNullOrWhiteSpace(x.ToUserName))
            .WithMessage("接收者用户 ID 与用户名不能同时为空");
        RuleFor(x => x.ToUserId)
            .GreaterThan(0).WithMessage("接收者用户 ID无效")
            .When(x => string.IsNullOrWhiteSpace(x.ToUserName));
        RuleFor(x => x.MessageTitle)
            .MaximumLength(200).WithMessage("消息标题长度不能超过200个字符");
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.MessageContent) || HasAttachments(x.Attachments))
            .WithMessage("消息内容与附件不能同时为空");
        RuleFor(x => x.Attachments)
            .MaximumLength(8000).WithMessage("附件 JSON 长度不能超过8000个字符");
        RuleFor(x => x.MessageType)
            .GreaterThanOrEqualTo(0).WithMessage("消息类型无效");
        RuleFor(x => x.MessageGroup)
            .GreaterThan(0).WithMessage("消息分组不能为空");
        RuleFor(x => x.IsCc)
            .InclusiveBetween(0, 1).WithMessage("是否抄送无效");
        RuleFor(x => x.ReadStatus)
            .InclusiveBetween(0, 1).WithMessage("读取状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }

    private static bool HasAttachments(string? attachments)
    {
        if (string.IsNullOrWhiteSpace(attachments))
        {
            return false;
        }

        var trimmed = attachments.Trim();
        return trimmed != "[]" && trimmed != "{}";
    }
}

// ========================================
// 批量创建并推送 验证器
// ========================================

/// <summary>
/// 批量创建在线消息并推送 DTO 验证器
/// </summary>
public class TaktMessageBatchCreateValidator : AbstractValidator<TaktMessageBatchCreateDto>
{
    /// <summary>
    /// 单批最大接收者数量（全员发送，与 06-overflow-csharp 对齐）
    /// </summary>
    private const int MaxBatchRecipients = 500;

    /// <summary>
    /// 指定用户列表模式最大接收者数量
    /// </summary>
    private const int MaxListRecipients = 5;

    /// <summary>
    /// 初始化批量创建并推送校验规则
    /// </summary>
    public TaktMessageBatchCreateValidator()
    {
        RuleFor(x => x.FromUserName)
            .NotEmpty().WithMessage("发送者用户名不能为空")
            .MaximumLength(40).WithMessage("发送者用户名长度不能超过40个字符");
        RuleFor(x => x.FromUserId)
            .GreaterThan(0).WithMessage("发送者用户 ID无效")
            .When(x => string.IsNullOrWhiteSpace(x.FromUserName));
        RuleFor(x => x)
            .Must(x => x.FromUserId > 0 || !string.IsNullOrWhiteSpace(x.FromUserName))
            .WithMessage("发送者用户 ID 与用户名不能同时为空");
        RuleFor(x => x)
            .Must(x => x.SendToAll || (x.ToUserIds != null && x.ToUserIds.Count > 0))
            .WithMessage("须选择全员发送或至少一位接收者");
        RuleFor(x => x.ToUserIds)
            .Must(ids => ids == null || ids.Count <= MaxListRecipients)
            .When(x => !x.SendToAll)
            .WithMessage($"指定用户最多选择 {MaxListRecipients} 位");
        RuleFor(x => x.ToUserIds)
            .Must(ids => ids == null || ids.Count <= MaxBatchRecipients)
            .When(x => x.SendToAll)
            .WithMessage($"单次最多向 {MaxBatchRecipients} 位用户发送消息");
        RuleFor(x => x.MessageTitle)
            .MaximumLength(200).WithMessage("消息标题长度不能超过200个字符");
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.MessageContent) || HasAttachments(x.Attachments))
            .WithMessage("消息内容与附件不能同时为空");
        RuleFor(x => x.Attachments)
            .MaximumLength(8000).WithMessage("附件 JSON 长度不能超过8000个字符");
        RuleFor(x => x.MessageType)
            .GreaterThanOrEqualTo(0).WithMessage("消息类型无效");
        RuleFor(x => x.MessageGroup)
            .GreaterThan(0).WithMessage("消息分组不能为空");
        RuleFor(x => x.IsCc)
            .InclusiveBetween(0, 1).WithMessage("是否抄送无效");
        RuleFor(x => x.ReadStatus)
            .InclusiveBetween(0, 1).WithMessage("读取状态无效");
    }

    private static bool HasAttachments(string? attachments)
    {
        if (string.IsNullOrWhiteSpace(attachments))
        {
            return false;
        }

        var trimmed = attachments.Trim();
        return trimmed != "[]" && trimmed != "{}";
    }
}

// ========================================
// 已读 / 未读 验证器
// ========================================

/// <summary>
/// 标记在线消息已读 DTO 验证器
/// </summary>
public class TaktMessageReadValidator : AbstractValidator<TaktMessageReadDto>
{
    /// <summary>
    /// 初始化已读校验规则
    /// </summary>
    public TaktMessageReadValidator()
    {
        RuleFor(x => x.MessageId)
            .GreaterThan(0).WithMessage("MessageID无效");
        RuleFor(x => x.ReadStatus)
            .Equal(1).WithMessage("读取状态必须为已读");
    }
}

/// <summary>
/// 标记在线消息未读 DTO 验证器
/// </summary>
public class TaktMessageUnreadValidator : AbstractValidator<TaktMessageUnreadDto>
{
    /// <summary>
    /// 初始化未读校验规则
    /// </summary>
    public TaktMessageUnreadValidator()
    {
        RuleFor(x => x.MessageId)
            .GreaterThan(0).WithMessage("MessageID无效");
        RuleFor(x => x.ReadStatus)
            .Equal(0).WithMessage("读取状态必须为未读");
        RuleFor(x => x.ReadTime)
            .Null().WithMessage("标记未读须清空读取时间");
    }
}
