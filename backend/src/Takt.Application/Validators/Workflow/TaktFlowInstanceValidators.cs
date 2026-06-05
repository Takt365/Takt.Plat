// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Workflow
// 文件名称：TaktFlowInstanceValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowInstance 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFlowInstance 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Workflow;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Workflow;

// ========================================
// 创建FlowInstance 验证器
// ========================================

/// <summary>
/// 创建FlowInstance DTO 验证器
/// </summary>
public class TaktFlowInstanceCreateValidator : AbstractValidator<TaktFlowInstanceCreateDto>
{
    /// <summary>
    /// 初始化 创建FlowInstance 校验规则
    /// </summary>
    public TaktFlowInstanceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.InstanceCode)
            .NotEmpty().WithMessage("实例编码不能为空")
            .MaximumLength(64).WithMessage("实例编码长度不能超过64个字符");
        RuleFor(x => x.ProcessDefinitionId)
            .GreaterThanOrEqualTo(0).WithMessage("流程定义 ID不能为负数");
        RuleFor(x => x.ProcessKey)
            .NotEmpty().WithMessage("流程键不能为空")
            .MaximumLength(64).WithMessage("流程键长度不能超过64个字符");
        RuleFor(x => x.ProcessName)
            .NotEmpty().WithMessage("流程名称不能为空")
            .MaximumLength(200).WithMessage("流程名称长度不能超过200个字符");
        RuleFor(x => x.ProcessTitle)
            .MaximumLength(500).WithMessage("申请标题长度不能超过500个字符");
        RuleFor(x => x.InstanceStatus)
            .IsInEnum().WithMessage("实例状态无效");
        RuleFor(x => x.CurrentActivityId)
            .MaximumLength(64).WithMessage("当前节点 ID长度不能超过64个字符");
        RuleFor(x => x.CurrentActivityName)
            .MaximumLength(200).WithMessage("当前节点名称长度不能超过200个字符");
        RuleFor(x => x.StartUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发起人 ID不能为负数");
        RuleFor(x => x.StartUserName)
            .MaximumLength(20).WithMessage("发起人姓名长度不能超过20个字符");
        RuleFor(x => x.BusinessKey)
            .MaximumLength(64).WithMessage("业务主键长度不能超过64个字符");
        RuleFor(x => x.BusinessType)
            .MaximumLength(64).WithMessage("业务类型长度不能超过64个字符");
        RuleFor(x => x.SuperInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("父流程实例 ID不能为负数");
        RuleFor(x => x.DeleteReason)
            .MaximumLength(500).WithMessage("终止原因长度不能超过500个字符");
        RuleFor(x => x.FormId)
            .GreaterThanOrEqualTo(0).WithMessage("关联表单 ID不能为负数");
        RuleFor(x => x.FormCode)
            .MaximumLength(64).WithMessage("关联表单编码长度不能超过64个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FlowInstance 验证器
// ========================================

/// <summary>
/// 更新FlowInstance DTO 验证器
/// </summary>
public class TaktFlowInstanceUpdateValidator : AbstractValidator<TaktFlowInstanceUpdateDto>
{
    /// <summary>
    /// 初始化 更新FlowInstance 校验规则
    /// </summary>
    public TaktFlowInstanceUpdateValidator()
    {
        RuleFor(x => x.FlowInstanceId)
            .GreaterThan(0).WithMessage("FlowInstanceID无效");
    }
}

// ========================================
// 导入FlowInstance 验证器
// ========================================

/// <summary>
/// 导入FlowInstance DTO 验证器
/// </summary>
public class TaktFlowInstanceImportValidator : AbstractValidator<TaktFlowInstanceImportDto>
{
    /// <summary>
    /// 初始化 导入FlowInstance 校验规则
    /// </summary>
    public TaktFlowInstanceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.InstanceCode)
            .NotEmpty().WithMessage("实例编码不能为空")
            .MaximumLength(64).WithMessage("实例编码长度不能超过64个字符");
        RuleFor(x => x.ProcessDefinitionId)
            .GreaterThanOrEqualTo(0).WithMessage("流程定义 ID不能为负数");
        RuleFor(x => x.ProcessKey)
            .NotEmpty().WithMessage("流程键不能为空")
            .MaximumLength(64).WithMessage("流程键长度不能超过64个字符");
        RuleFor(x => x.ProcessName)
            .NotEmpty().WithMessage("流程名称不能为空")
            .MaximumLength(200).WithMessage("流程名称长度不能超过200个字符");
        RuleFor(x => x.ProcessTitle)
            .MaximumLength(500).WithMessage("申请标题长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.ProcessTitle));
        RuleFor(x => x.InstanceStatus)
            .IsInEnum().WithMessage("实例状态无效");
        RuleFor(x => x.CurrentActivityId)
            .MaximumLength(64).WithMessage("当前节点 ID长度不能超过64个字符").When(x => !string.IsNullOrWhiteSpace(x.CurrentActivityId));
        RuleFor(x => x.CurrentActivityName)
            .MaximumLength(200).WithMessage("当前节点名称长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.CurrentActivityName));
        RuleFor(x => x.StartUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发起人 ID不能为负数");
        RuleFor(x => x.StartUserName)
            .MaximumLength(20).WithMessage("发起人姓名长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.StartUserName));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
