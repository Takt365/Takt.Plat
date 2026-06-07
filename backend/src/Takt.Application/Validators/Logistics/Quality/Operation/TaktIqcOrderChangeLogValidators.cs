// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderChangeLogValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：IqcOrderChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIqcOrderChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建IqcOrderChangeLog 验证器
// ========================================

/// <summary>
/// 创建IqcOrderChangeLog DTO 验证器
/// </summary>
public class TaktIqcOrderChangeLogCreateValidator : AbstractValidator<TaktIqcOrderChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建IqcOrderChangeLog 校验规则
    /// </summary>
    public TaktIqcOrderChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.IqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IQC检验单ID不能为负数");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表长度不能超过4000个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(1000).WithMessage("变更原因长度不能超过1000个字符");
        RuleFor(x => x.ChangeBy)
            .MaximumLength(50).WithMessage("变更人长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新IqcOrderChangeLog 验证器
// ========================================

/// <summary>
/// 更新IqcOrderChangeLog DTO 验证器
/// </summary>
public class TaktIqcOrderChangeLogUpdateValidator : AbstractValidator<TaktIqcOrderChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新IqcOrderChangeLog 校验规则
    /// </summary>
    public TaktIqcOrderChangeLogUpdateValidator()
    {
        RuleFor(x => x.IqcOrderChangeLogId)
            .GreaterThan(0).WithMessage("IqcOrderChangeLogID无效");
    }
}
