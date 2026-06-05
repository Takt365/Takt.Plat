// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktUserValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：User 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktUser 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Identity;

// ========================================
// User 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 User DTO 验证器
/// </summary>
public class TaktUserDtoValidator : AbstractValidator<TaktUserDto>
{
    /// <summary>
    /// 初始化 关联 User 校验规则
    /// </summary>
    public TaktUserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.Nickname)
            .MaximumLength(40).WithMessage("昵称长度不能超过40个字符");
        RuleFor(x => x.UserType)
            .IsInEnum().WithMessage("用户类型无效");
        RuleFor(x => x.PasswordHash)
            .MaximumLength(255).WithMessage("密码哈希值长度不能超过255个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("关联的员工ID无效");
        RuleFor(x => x.UserStatus)
            .IsInEnum().WithMessage("状态无效");
        RuleFor(x => x.LastLoginIp)
            .MaximumLength(50).WithMessage("最后登录IP长度不能超过50个字符");
        RuleFor(x => x.DefaultCulture)
            .NotEmpty().WithMessage("默认区域文化编码不能为空")
            .MaximumLength(5).WithMessage("默认区域文化编码长度不能超过5个字符");
        RuleFor(x => x.EmployeeName)
            .MaximumLength(200).WithMessage("EmployeeName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.EmployeeName));
    }
}
