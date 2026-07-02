// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderChangeLogValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：IpqcOrderChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIpqcOrderChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建IpqcOrderChangeLog 验证器
// ========================================

/// <summary>
/// 创建IpqcOrderChangeLog DTO 验证器
/// </summary>
public class TaktIpqcOrderChangeLogCreateValidator : AbstractValidator<TaktIpqcOrderChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建IpqcOrderChangeLog 校验规则
    /// </summary>
    public TaktIpqcOrderChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.IpqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IPQC检验单 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新IpqcOrderChangeLog 验证器
// ========================================

/// <summary>
/// 更新IpqcOrderChangeLog DTO 验证器
/// </summary>
public class TaktIpqcOrderChangeLogUpdateValidator : AbstractValidator<TaktIpqcOrderChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新IpqcOrderChangeLog 校验规则
    /// </summary>
    public TaktIpqcOrderChangeLogUpdateValidator()
    {
        RuleFor(x => x.IpqcOrderChangeLogId)
            .GreaterThan(0).WithMessage("IpqcOrderChangeLogID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.IpqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IPQC检验单 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
