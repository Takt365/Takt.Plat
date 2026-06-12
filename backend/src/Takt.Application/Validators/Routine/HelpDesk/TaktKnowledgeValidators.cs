// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktKnowledgeValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Knowledge 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktKnowledge 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

// ========================================
// 创建Knowledge 验证器
// ========================================

/// <summary>
/// 创建Knowledge DTO 验证器
/// </summary>
public class TaktKnowledgeCreateValidator : AbstractValidator<TaktKnowledgeCreateDto>
{
    /// <summary>
    /// 初始化 创建Knowledge 校验规则
    /// </summary>
    public TaktKnowledgeCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("知识标题不能为空")
            .MaximumLength(200).WithMessage("知识标题长度不能超过200个字符");
        RuleFor(x => x.Summary)
            .MaximumLength(1000).WithMessage("知识摘要长度不能超过1000个字符");
        RuleFor(x => x.CategoryCode)
            .MaximumLength(40).WithMessage("分类编码长度不能超过40个字符");
        RuleFor(x => x.Tags)
            .MaximumLength(500).WithMessage("标签长度不能超过500个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Knowledge 验证器
// ========================================

/// <summary>
/// 更新Knowledge DTO 验证器
/// </summary>
public class TaktKnowledgeUpdateValidator : AbstractValidator<TaktKnowledgeUpdateDto>
{
    /// <summary>
    /// 初始化 更新Knowledge 校验规则
    /// </summary>
    public TaktKnowledgeUpdateValidator()
    {
        RuleFor(x => x.KnowledgeId)
            .GreaterThan(0).WithMessage("KnowledgeID无效");
    }
}

// ========================================
// 导入Knowledge 验证器
// ========================================

/// <summary>
/// 导入Knowledge DTO 验证器
/// </summary>
public class TaktKnowledgeImportValidator : AbstractValidator<TaktKnowledgeImportDto>
{
    /// <summary>
    /// 初始化 导入Knowledge 校验规则
    /// </summary>
    public TaktKnowledgeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("知识标题不能为空")
            .MaximumLength(200).WithMessage("知识标题长度不能超过200个字符");
        RuleFor(x => x.Summary)
            .MaximumLength(1000).WithMessage("知识摘要长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.Summary));
        RuleFor(x => x.CategoryCode)
            .MaximumLength(40).WithMessage("分类编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CategoryCode));
        RuleFor(x => x.Tags)
            .MaximumLength(500).WithMessage("标签长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Tags));
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
