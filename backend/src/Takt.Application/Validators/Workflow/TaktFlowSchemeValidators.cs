// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Workflow
// 文件名称：TaktFlowSchemeValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowScheme 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFlowScheme 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Workflow;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Workflow;

// ========================================
// 创建FlowScheme 验证器
// ========================================

/// <summary>
/// 创建FlowScheme DTO 验证器
/// </summary>
public class TaktFlowSchemeCreateValidator : AbstractValidator<TaktFlowSchemeCreateDto>
{
    /// <summary>
    /// 初始化 创建FlowScheme 校验规则
    /// </summary>
    public TaktFlowSchemeCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ProcessKey)
            .NotEmpty().WithMessage("流程键不能为空")
            .MaximumLength(64).WithMessage("流程键长度不能超过64个字符");
        RuleFor(x => x.ProcessName)
            .NotEmpty().WithMessage("流程名称不能为空")
            .MaximumLength(200).WithMessage("流程名称长度不能超过200个字符");
        RuleFor(x => x.ProcessVersion)
            .NotEmpty().WithMessage("版本标签不能为空")
            .MaximumLength(32).WithMessage("版本标签长度不能超过32个字符");
        RuleFor(x => x.ProcessDescription)
            .MaximumLength(500).WithMessage("流程说明长度不能超过500个字符");
        RuleFor(x => x.ProcessStatus)
            .IsInEnum().WithMessage("发布状态无效");
        RuleFor(x => x.SuspensionState)
            .IsInEnum().WithMessage("挂起状态无效");
        RuleFor(x => x.DeploymentId)
            .MaximumLength(64).WithMessage("部署批次号长度不能超过64个字符");
        RuleFor(x => x.FormId)
            .GreaterThanOrEqualTo(0).WithMessage("关联表单 ID不能为负数");
        RuleFor(x => x.FormCode)
            .MaximumLength(64).WithMessage("关联表单编码长度不能超过64个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FlowScheme 验证器
// ========================================

/// <summary>
/// 更新FlowScheme DTO 验证器
/// </summary>
public class TaktFlowSchemeUpdateValidator : AbstractValidator<TaktFlowSchemeUpdateDto>
{
    /// <summary>
    /// 初始化 更新FlowScheme 校验规则
    /// </summary>
    public TaktFlowSchemeUpdateValidator()
    {
        RuleFor(x => x.FlowSchemeId)
            .GreaterThan(0).WithMessage("FlowSchemeID无效");
    }
}

// ========================================
// 导入FlowScheme 验证器
// ========================================

/// <summary>
/// 导入FlowScheme DTO 验证器
/// </summary>
public class TaktFlowSchemeImportValidator : AbstractValidator<TaktFlowSchemeImportDto>
{
    /// <summary>
    /// 初始化 导入FlowScheme 校验规则
    /// </summary>
    public TaktFlowSchemeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ProcessKey)
            .NotEmpty().WithMessage("流程键不能为空")
            .MaximumLength(64).WithMessage("流程键长度不能超过64个字符");
        RuleFor(x => x.ProcessName)
            .NotEmpty().WithMessage("流程名称不能为空")
            .MaximumLength(200).WithMessage("流程名称长度不能超过200个字符");
        RuleFor(x => x.ProcessVersion)
            .NotEmpty().WithMessage("版本标签不能为空")
            .MaximumLength(32).WithMessage("版本标签长度不能超过32个字符");
        RuleFor(x => x.ProcessDescription)
            .MaximumLength(500).WithMessage("流程说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.ProcessDescription));
        RuleFor(x => x.ProcessStatus)
            .IsInEnum().WithMessage("发布状态无效");
        RuleFor(x => x.SuspensionState)
            .IsInEnum().WithMessage("挂起状态无效");
        RuleFor(x => x.DeploymentId)
            .MaximumLength(64).WithMessage("部署批次号长度不能超过64个字符").When(x => !string.IsNullOrWhiteSpace(x.DeploymentId));
        RuleFor(x => x.FormId)
            .GreaterThanOrEqualTo(0).WithMessage("关联表单 ID不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
