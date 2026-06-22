// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialChangeLogValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialChangeLog 验证器
// ========================================

/// <summary>
/// 创建MaterialChangeLog DTO 验证器
/// </summary>
public class TaktMaterialChangeLogCreateValidator : AbstractValidator<TaktMaterialChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialChangeLog 校验规则
    /// </summary>
    public TaktMaterialChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialId)
            .GreaterThanOrEqualTo(0).WithMessage("全局物料ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表长度不能超过4000个字符");
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
// 更新MaterialChangeLog 验证器
// ========================================

/// <summary>
/// 更新MaterialChangeLog DTO 验证器
/// </summary>
public class TaktMaterialChangeLogUpdateValidator : AbstractValidator<TaktMaterialChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialChangeLog 校验规则
    /// </summary>
    public TaktMaterialChangeLogUpdateValidator()
    {
        RuleFor(x => x.MaterialChangeLogId)
            .GreaterThan(0).WithMessage("MaterialChangeLogID无效");
    }
}
