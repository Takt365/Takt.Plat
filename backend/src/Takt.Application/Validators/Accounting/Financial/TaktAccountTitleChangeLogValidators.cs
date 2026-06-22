// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktAccountTitleChangeLogValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：AccountTitleChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAccountTitleChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建AccountTitleChangeLog 验证器
// ========================================

/// <summary>
/// 创建AccountTitleChangeLog DTO 验证器
/// </summary>
public class TaktAccountTitleChangeLogCreateValidator : AbstractValidator<TaktAccountTitleChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建AccountTitleChangeLog 校验规则
    /// </summary>
    public TaktAccountTitleChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.AccountTitleId)
            .GreaterThanOrEqualTo(0).WithMessage("会计科目 ID不能为负数");
        RuleFor(x => x.TitleCode)
            .NotEmpty().WithMessage("科目编码不能为空")
            .MaximumLength(50).WithMessage("科目编码长度不能超过50个字符");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表 JSON长度不能超过4000个字符");
        RuleFor(x => x.ChangeBy)
            .MaximumLength(50).WithMessage("变更人长度不能超过50个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(500).WithMessage("变更原因长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新AccountTitleChangeLog 验证器
// ========================================

/// <summary>
/// 更新AccountTitleChangeLog DTO 验证器
/// </summary>
public class TaktAccountTitleChangeLogUpdateValidator : AbstractValidator<TaktAccountTitleChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新AccountTitleChangeLog 校验规则
    /// </summary>
    public TaktAccountTitleChangeLogUpdateValidator()
    {
        RuleFor(x => x.AccountTitleChangeLogId)
            .GreaterThan(0).WithMessage("AccountTitleChangeLogID无效");
    }
}
