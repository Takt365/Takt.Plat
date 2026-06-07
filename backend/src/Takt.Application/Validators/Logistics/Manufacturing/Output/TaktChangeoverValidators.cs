// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Changeover 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktChangeover 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建Changeover 验证器
// ========================================

/// <summary>
/// 创建Changeover DTO 验证器
/// </summary>
public class TaktChangeoverCreateValidator : AbstractValidator<TaktChangeoverCreateDto>
{
    /// <summary>
    /// 初始化 创建Changeover 校验规则
    /// </summary>
    public TaktChangeoverCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .MaximumLength(50).WithMessage("生产工厂长度不能超过50个字符");
        RuleFor(x => x.ProductionCategory)
            .MaximumLength(50).WithMessage("生产类别长度不能超过50个字符");
        RuleFor(x => x.ProductionLine)
            .MaximumLength(50).WithMessage("生产线长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Changeover 验证器
// ========================================

/// <summary>
/// 更新Changeover DTO 验证器
/// </summary>
public class TaktChangeoverUpdateValidator : AbstractValidator<TaktChangeoverUpdateDto>
{
    /// <summary>
    /// 初始化 更新Changeover 校验规则
    /// </summary>
    public TaktChangeoverUpdateValidator()
    {
        RuleFor(x => x.ChangeoverId)
            .GreaterThan(0).WithMessage("ChangeoverID无效");
    }
}

// ========================================
// 导入Changeover 验证器
// ========================================

/// <summary>
/// 导入Changeover DTO 验证器
/// </summary>
public class TaktChangeoverImportValidator : AbstractValidator<TaktChangeoverImportDto>
{
    /// <summary>
    /// 初始化 导入Changeover 校验规则
    /// </summary>
    public TaktChangeoverImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(50).WithMessage("生产工厂长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProductionCategory)
            .MaximumLength(50).WithMessage("生产类别长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionCategory));
        RuleFor(x => x.ProductionLine)
            .MaximumLength(50).WithMessage("生产线长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionLine));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
