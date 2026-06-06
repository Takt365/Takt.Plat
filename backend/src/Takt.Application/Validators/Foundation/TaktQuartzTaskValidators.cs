// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktQuartzTaskValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：QuartzTask 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQuartzTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建QuartzTask 验证器
// ========================================

/// <summary>
/// 创建QuartzTask DTO 验证器
/// </summary>
public class TaktQuartzTaskCreateValidator : AbstractValidator<TaktQuartzTaskCreateDto>
{
    /// <summary>
    /// 初始化 创建QuartzTask 校验规则
    /// </summary>
    public TaktQuartzTaskCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.TaskCode)
            .NotEmpty().WithMessage("任务编码不能为空")
            .MaximumLength(50).WithMessage("任务编码长度不能超过50个字符");
        RuleFor(x => x.TaskName)
            .NotEmpty().WithMessage("任务名称不能为空")
            .MaximumLength(100).WithMessage("任务名称长度不能超过100个字符");
        RuleFor(x => x.JobName)
            .NotEmpty().WithMessage("Quartz Job 名称不能为空")
            .MaximumLength(100).WithMessage("Quartz Job 名称长度不能超过100个字符");
        RuleFor(x => x.JobGroup)
            .NotEmpty().WithMessage("Quartz Job 分组不能为空")
            .MaximumLength(50).WithMessage("Quartz Job 分组长度不能超过50个字符");
        RuleFor(x => x.CronExpression)
            .NotEmpty().WithMessage("Cron 表达式不能为空")
            .MaximumLength(100).WithMessage("Cron 表达式长度不能超过100个字符");
        RuleFor(x => x.JobType)
            .NotEmpty().WithMessage("任务处理器类型不能为空")
            .MaximumLength(200).WithMessage("任务处理器类型长度不能超过200个字符");
        RuleFor(x => x.JobParams)
            .MaximumLength(4000).WithMessage("任务参数 JSON长度不能超过4000个字符");
        RuleFor(x => x.TaskStatus)
            .IsInEnum().WithMessage("任务状态无效");
        RuleFor(x => x.Concurrent)
            .IsInEnum().WithMessage("是否允许并发执行无效");
        RuleFor(x => x.MisfirePolicy)
            .IsInEnum().WithMessage("Misfire 策略无效");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("任务描述长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QuartzTask 验证器
// ========================================

/// <summary>
/// 更新QuartzTask DTO 验证器
/// </summary>
public class TaktQuartzTaskUpdateValidator : AbstractValidator<TaktQuartzTaskUpdateDto>
{
    /// <summary>
    /// 初始化 更新QuartzTask 校验规则
    /// </summary>
    public TaktQuartzTaskUpdateValidator()
    {
        RuleFor(x => x.QuartzTaskId)
            .GreaterThan(0).WithMessage("QuartzTaskID无效");
    }
}

// ========================================
// 导入QuartzTask 验证器
// ========================================

/// <summary>
/// 导入QuartzTask DTO 验证器
/// </summary>
public class TaktQuartzTaskImportValidator : AbstractValidator<TaktQuartzTaskImportDto>
{
    /// <summary>
    /// 初始化 导入QuartzTask 校验规则
    /// </summary>
    public TaktQuartzTaskImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.TaskCode)
            .NotEmpty().WithMessage("任务编码不能为空")
            .MaximumLength(50).WithMessage("任务编码长度不能超过50个字符");
        RuleFor(x => x.TaskName)
            .NotEmpty().WithMessage("任务名称不能为空")
            .MaximumLength(100).WithMessage("任务名称长度不能超过100个字符");
        RuleFor(x => x.JobName)
            .NotEmpty().WithMessage("Quartz Job 名称不能为空")
            .MaximumLength(100).WithMessage("Quartz Job 名称长度不能超过100个字符");
        RuleFor(x => x.JobGroup)
            .NotEmpty().WithMessage("Quartz Job 分组不能为空")
            .MaximumLength(50).WithMessage("Quartz Job 分组长度不能超过50个字符");
        RuleFor(x => x.CronExpression)
            .NotEmpty().WithMessage("Cron 表达式不能为空")
            .MaximumLength(100).WithMessage("Cron 表达式长度不能超过100个字符");
        RuleFor(x => x.JobType)
            .NotEmpty().WithMessage("任务处理器类型不能为空")
            .MaximumLength(200).WithMessage("任务处理器类型长度不能超过200个字符");
        RuleFor(x => x.JobParams)
            .MaximumLength(4000).WithMessage("任务参数 JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.JobParams));
        RuleFor(x => x.TaskStatus)
            .IsInEnum().WithMessage("任务状态无效");
        RuleFor(x => x.Concurrent)
            .IsInEnum().WithMessage("是否允许并发执行无效");
        RuleFor(x => x.MisfirePolicy)
            .IsInEnum().WithMessage("Misfire 策略无效");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("任务描述长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
