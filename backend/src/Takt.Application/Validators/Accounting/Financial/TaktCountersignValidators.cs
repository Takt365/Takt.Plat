// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktCountersignValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Countersign 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCountersign 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建Countersign 验证器
// ========================================

/// <summary>
/// 创建Countersign DTO 验证器
/// </summary>
public class TaktCountersignCreateValidator : AbstractValidator<TaktCountersignCreateDto>
{
    /// <summary>
    /// 初始化 创建Countersign 校验规则
    /// </summary>
    public TaktCountersignCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CountersignCode)
            .NotEmpty().WithMessage("会签编号不能为空")
            .MaximumLength(50).WithMessage("会签编号长度不能超过50个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.ApplicationDept)
            .MaximumLength(100).WithMessage("申请部门长度不能超过100个字符");
        RuleFor(x => x.CostBearerDept)
            .MaximumLength(100).WithMessage("经费负担部门长度不能超过100个字符");
        RuleFor(x => x.BudgetItem)
            .MaximumLength(200).WithMessage("预算项目长度不能超过200个字符");
        RuleFor(x => x.CountersignTitle)
            .MaximumLength(500).WithMessage("标题长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Countersign 验证器
// ========================================

/// <summary>
/// 更新Countersign DTO 验证器
/// </summary>
public class TaktCountersignUpdateValidator : AbstractValidator<TaktCountersignUpdateDto>
{
    /// <summary>
    /// 初始化 更新Countersign 校验规则
    /// </summary>
    public TaktCountersignUpdateValidator()
    {
        RuleFor(x => x.CountersignId)
            .GreaterThan(0).WithMessage("CountersignID无效");
    }
}

// ========================================
// 导入Countersign 验证器
// ========================================

/// <summary>
/// 导入Countersign DTO 验证器
/// </summary>
public class TaktCountersignImportValidator : AbstractValidator<TaktCountersignImportDto>
{
    /// <summary>
    /// 初始化 导入Countersign 校验规则
    /// </summary>
    public TaktCountersignImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CountersignCode)
            .NotEmpty().WithMessage("会签编号不能为空")
            .MaximumLength(50).WithMessage("会签编号长度不能超过50个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.ApplicationDept)
            .MaximumLength(100).WithMessage("申请部门长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.ApplicationDept));
        RuleFor(x => x.CostBearerDept)
            .MaximumLength(100).WithMessage("经费负担部门长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.CostBearerDept));
        RuleFor(x => x.BudgetItem)
            .MaximumLength(200).WithMessage("预算项目长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.BudgetItem));
        RuleFor(x => x.CountersignTitle)
            .MaximumLength(500).WithMessage("标题长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.CountersignTitle));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
