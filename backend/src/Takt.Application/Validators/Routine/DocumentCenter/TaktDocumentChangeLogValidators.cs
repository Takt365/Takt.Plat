// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.DocumentCenter
// 文件名称：TaktDocumentChangeLogValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：DocumentChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDocumentChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Routine.DocumentCenter;

// ========================================
// 创建DocumentChangeLog 验证器
// ========================================

/// <summary>
/// 创建DocumentChangeLog DTO 验证器
/// </summary>
public class TaktDocumentChangeLogCreateValidator : AbstractValidator<TaktDocumentChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建DocumentChangeLog 校验规则
    /// </summary>
    public TaktDocumentChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.DocumentId)
            .GreaterThanOrEqualTo(0).WithMessage("文档 ID不能为负数");
        RuleFor(x => x.DocumentCode)
            .MaximumLength(50).WithMessage("文档编码长度不能超过50个字符");
        RuleFor(x => x.DocumentTitle)
            .MaximumLength(200).WithMessage("文档标题长度不能超过200个字符");
        RuleFor(x => x.ChangeType)
            .IsInEnum().WithMessage("变更类型无效");
        RuleFor(x => x.ChangeSummary)
            .MaximumLength(500).WithMessage("变更内容摘要长度不能超过500个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(500).WithMessage("变更原因或备注长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新DocumentChangeLog 验证器
// ========================================

/// <summary>
/// 更新DocumentChangeLog DTO 验证器
/// </summary>
public class TaktDocumentChangeLogUpdateValidator : AbstractValidator<TaktDocumentChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新DocumentChangeLog 校验规则
    /// </summary>
    public TaktDocumentChangeLogUpdateValidator()
    {
        RuleFor(x => x.DocumentChangeLogId)
            .GreaterThan(0).WithMessage("DocumentChangeLogID无效");
    }
}
