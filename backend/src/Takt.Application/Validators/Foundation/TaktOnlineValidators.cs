// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktOnlineValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Online 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktOnline 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Foundation;

// ========================================
// SignalR 会话注册验证器
// ========================================

/// <summary>
/// SignalR 在线会话注册 DTO 验证器
/// </summary>
public class TaktOnlineCreateValidator : AbstractValidator<TaktOnlineCreateDto>
{
    /// <summary>
    /// 初始化 SignalR 会话注册校验规则
    /// </summary>
    public TaktOnlineCreateValidator()
    {
        RuleFor(x => x.ConnectionId)
            .NotEmpty().WithMessage("SignalR 连接 ID不能为空")
            .MaximumLength(200).WithMessage("SignalR 连接 ID长度不能超过200个字符");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(40).WithMessage("用户名长度不能超过40个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.OnlineStatus)
            .IsInEnum().WithMessage("在线状态无效");
        RuleFor(x => x.ConnectIp)
            .MaximumLength(50).WithMessage("连接 IP 地址长度不能超过50个字符");
        RuleFor(x => x.ConnectLocation)
            .MaximumLength(200).WithMessage("连接地点长度不能超过200个字符");
        RuleFor(x => x.UserAgent)
            .MaximumLength(500).WithMessage("User-Agent长度不能超过500个字符");
        RuleFor(x => x.DeviceType)
            .IsInEnum().WithMessage("设备类型无效");
        RuleFor(x => x.BrowserType)
            .IsInEnum().WithMessage("浏览器类型无效");
        RuleFor(x => x.OperatingSystem)
            .IsInEnum().WithMessage("操作系统无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
