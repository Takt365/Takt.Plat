// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Workflow
// 文件名称：TaktFlowTaskValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowTask 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFlowTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Workflow;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Workflow;

// ========================================
// 创建FlowTask 验证器
// ========================================

/// <summary>
/// 创建FlowTask DTO 验证器
/// </summary>
public class TaktFlowTaskCreateValidator : AbstractValidator<TaktFlowTaskCreateDto>
{
    /// <summary>
    /// 初始化 创建FlowTask 校验规则
    /// </summary>
    public TaktFlowTaskCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.InstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.TaskDefinitionKey)
            .NotEmpty().WithMessage("任务定义键不能为空")
            .MaximumLength(64).WithMessage("任务定义键长度不能超过64个字符");
        RuleFor(x => x.TaskName)
            .MaximumLength(200).WithMessage("任务名称长度不能超过200个字符");
        RuleFor(x => x.AssigneeUserId)
            .GreaterThanOrEqualTo(0).WithMessage("办理人 ID不能为负数");
        RuleFor(x => x.AssigneeUserName)
            .MaximumLength(20).WithMessage("办理人姓名长度不能超过20个字符");
        RuleFor(x => x.OwnerUserId)
            .GreaterThanOrEqualTo(0).WithMessage("任务所有者 ID不能为负数");
        RuleFor(x => x.TaskStatus)
            .IsInEnum().WithMessage("任务状态无效");
        RuleFor(x => x.SignType)
            .IsInEnum().WithMessage("会签类型无效");
        RuleFor(x => x.AddSignId)
            .GreaterThanOrEqualTo(0).WithMessage("加签记录 ID不能为负数");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("多实例序号不能为负数");
        RuleFor(x => x.Comment)
            .MaximumLength(2000).WithMessage("审批意见长度不能超过2000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FlowTask 验证器
// ========================================

/// <summary>
/// 更新FlowTask DTO 验证器
/// </summary>
public class TaktFlowTaskUpdateValidator : AbstractValidator<TaktFlowTaskUpdateDto>
{
    /// <summary>
    /// 初始化 更新FlowTask 校验规则
    /// </summary>
    public TaktFlowTaskUpdateValidator()
    {
        RuleFor(x => x.FlowTaskId)
            .GreaterThan(0).WithMessage("FlowTaskID无效");
    }
}

// ========================================
// 导入FlowTask 验证器
// ========================================

/// <summary>
/// 导入FlowTask DTO 验证器
/// </summary>
public class TaktFlowTaskImportValidator : AbstractValidator<TaktFlowTaskImportDto>
{
    /// <summary>
    /// 初始化 导入FlowTask 校验规则
    /// </summary>
    public TaktFlowTaskImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.InstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.TaskDefinitionKey)
            .NotEmpty().WithMessage("任务定义键不能为空")
            .MaximumLength(64).WithMessage("任务定义键长度不能超过64个字符");
        RuleFor(x => x.TaskName)
            .MaximumLength(200).WithMessage("任务名称长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.TaskName));
        RuleFor(x => x.AssigneeUserId)
            .GreaterThanOrEqualTo(0).WithMessage("办理人 ID不能为负数");
        RuleFor(x => x.AssigneeUserName)
            .MaximumLength(20).WithMessage("办理人姓名长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.AssigneeUserName));
        RuleFor(x => x.OwnerUserId)
            .GreaterThanOrEqualTo(0).WithMessage("任务所有者 ID不能为负数");
        RuleFor(x => x.TaskStatus)
            .IsInEnum().WithMessage("任务状态无效");
        RuleFor(x => x.SignType)
            .IsInEnum().WithMessage("会签类型无效");
        RuleFor(x => x.AddSignId)
            .GreaterThanOrEqualTo(0).WithMessage("加签记录 ID不能为负数");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("多实例序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
