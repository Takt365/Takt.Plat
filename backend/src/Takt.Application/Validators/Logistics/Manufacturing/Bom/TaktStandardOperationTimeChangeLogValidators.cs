// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeChangeLogValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：StandardOperationTimeChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktStandardOperationTimeChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建StandardOperationTimeChangeLog 验证器
// ========================================

/// <summary>
/// 创建StandardOperationTimeChangeLog DTO 验证器
/// </summary>
public class TaktStandardOperationTimeChangeLogCreateValidator : AbstractValidator<TaktStandardOperationTimeChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建StandardOperationTimeChangeLog 校验规则
    /// </summary>
    public TaktStandardOperationTimeChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.StandardOperationTimeId)
            .GreaterThanOrEqualTo(0).WithMessage("标准工序时间ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新StandardOperationTimeChangeLog 验证器
// ========================================

/// <summary>
/// 更新StandardOperationTimeChangeLog DTO 验证器
/// </summary>
public class TaktStandardOperationTimeChangeLogUpdateValidator : AbstractValidator<TaktStandardOperationTimeChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新StandardOperationTimeChangeLog 校验规则
    /// </summary>
    public TaktStandardOperationTimeChangeLogUpdateValidator()
    {
        RuleFor(x => x.StandardOperationTimeChangeLogId)
            .GreaterThan(0).WithMessage("StandardOperationTimeChangeLogID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.StandardOperationTimeId)
            .GreaterThanOrEqualTo(0).WithMessage("标准工序时间ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
