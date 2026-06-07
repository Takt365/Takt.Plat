// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktQuartzLogValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：QuartzLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQuartzLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Statistics.Logging;

// ========================================
// 创建QuartzLog 验证器
// ========================================

/// <summary>
/// 创建QuartzLog DTO 验证器
/// </summary>
public class TaktQuartzLogCreateValidator : AbstractValidator<TaktQuartzLogCreateDto>
{
    /// <summary>
    /// 初始化 创建QuartzLog 校验规则
    /// </summary>
    public TaktQuartzLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.QuartzTaskId)
            .GreaterThanOrEqualTo(0).WithMessage("关联定时任务 ID不能为负数");
        RuleFor(x => x.UserName)
            .MaximumLength(50).WithMessage("触发用户长度不能超过50个字符");
        RuleFor(x => x.JobName)
            .NotEmpty().WithMessage("Job 名称不能为空")
            .MaximumLength(100).WithMessage("Job 名称长度不能超过100个字符");
        RuleFor(x => x.JobGroup)
            .NotEmpty().WithMessage("Job 分组不能为空")
            .MaximumLength(50).WithMessage("Job 分组长度不能超过50个字符");
        RuleFor(x => x.TriggerName)
            .MaximumLength(100).WithMessage("Trigger 名称长度不能超过100个字符");
        RuleFor(x => x.ExecuteStatus)
            .IsInEnum().WithMessage("执行状态无效");
        RuleFor(x => x.ErrorMsg)
            .MaximumLength(2000).WithMessage("错误消息长度不能超过2000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QuartzLog 验证器
// ========================================

/// <summary>
/// 更新QuartzLog DTO 验证器
/// </summary>
public class TaktQuartzLogUpdateValidator : AbstractValidator<TaktQuartzLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新QuartzLog 校验规则
    /// </summary>
    public TaktQuartzLogUpdateValidator()
    {
        RuleFor(x => x.QuartzLogId)
            .GreaterThan(0).WithMessage("QuartzLogID无效");
    }
}
