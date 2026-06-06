// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Accounting.Controlling
// 文件名称：TaktCostCenterChangeLogValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：CostCenterChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCostCenterChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Controlling;

namespace Takt.Application.Validators.Accounting.Controlling;

// ========================================
// 创建CostCenterChangeLog 验证器
// ========================================

/// <summary>
/// 创建CostCenterChangeLog DTO 验证器
/// </summary>
public class TaktCostCenterChangeLogCreateValidator : AbstractValidator<TaktCostCenterChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建CostCenterChangeLog 校验规则
    /// </summary>
    public TaktCostCenterChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("成本中心 ID不能为负数");
        RuleFor(x => x.CostCenterCode)
            .NotEmpty().WithMessage("成本中心编码不能为空")
            .MaximumLength(50).WithMessage("成本中心编码长度不能超过50个字符");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表 JSON长度不能超过4000个字符");
        RuleFor(x => x.ChangeBy)
            .MaximumLength(50).WithMessage("变更人长度不能超过50个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(500).WithMessage("变更原因长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CostCenterChangeLog 验证器
// ========================================

/// <summary>
/// 更新CostCenterChangeLog DTO 验证器
/// </summary>
public class TaktCostCenterChangeLogUpdateValidator : AbstractValidator<TaktCostCenterChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新CostCenterChangeLog 校验规则
    /// </summary>
    public TaktCostCenterChangeLogUpdateValidator()
    {
        RuleFor(x => x.CostCenterChangeLogId)
            .GreaterThan(0).WithMessage("CostCenterChangeLogID无效");
    }
}
