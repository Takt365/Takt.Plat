// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktOperLogValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：OperLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktOperLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Validators.Statistics.Logging;

// ========================================
// 创建OperLog 验证器
// ========================================

/// <summary>
/// 创建OperLog DTO 验证器
/// </summary>
public class TaktOperLogCreateValidator : AbstractValidator<TaktOperLogCreateDto>
{
    /// <summary>
    /// 初始化 创建OperLog 校验规则
    /// </summary>
    public TaktOperLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.OperModule)
            .MaximumLength(100).WithMessage("操作模块长度不能超过100个字符");
        RuleFor(x => x.OperType)
            .MaximumLength(50).WithMessage("操作类型长度不能超过50个字符");
        RuleFor(x => x.OperMethod)
            .MaximumLength(200).WithMessage("操作方法长度不能超过200个字符");
        RuleFor(x => x.RequestMethod)
            .MaximumLength(10).WithMessage("请求方式长度不能超过10个字符");
        RuleFor(x => x.OperUrl)
            .MaximumLength(500).WithMessage("操作 URL长度不能超过500个字符");
        RuleFor(x => x.OperIp)
            .MaximumLength(50).WithMessage("操作 IP长度不能超过50个字符");
        RuleFor(x => x.OperLocation)
            .MaximumLength(200).WithMessage("操作地点长度不能超过200个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新OperLog 验证器
// ========================================

/// <summary>
/// 更新OperLog DTO 验证器
/// </summary>
public class TaktOperLogUpdateValidator : AbstractValidator<TaktOperLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新OperLog 校验规则
    /// </summary>
    public TaktOperLogUpdateValidator()
    {
        RuleFor(x => x.OperLogId)
            .GreaterThan(0).WithMessage("OperLogID无效");
    }
}
