// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktModelDestinationValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：ModelDestination 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktModelDestination 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建ModelDestination 验证器
// ========================================

/// <summary>
/// 创建ModelDestination DTO 验证器
/// </summary>
public class TaktModelDestinationCreateValidator : AbstractValidator<TaktModelDestinationCreateDto>
{
    /// <summary>
    /// 初始化 创建ModelDestination 校验规则
    /// </summary>
    public TaktModelDestinationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("机种名称不能为空")
            .MaximumLength(200).WithMessage("机种名称长度不能超过200个字符");
        RuleFor(x => x.DestinationName)
            .NotEmpty().WithMessage("仕向地名称不能为空")
            .MaximumLength(200).WithMessage("仕向地名称长度不能超过200个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ModelDestination 验证器
// ========================================

/// <summary>
/// 更新ModelDestination DTO 验证器
/// </summary>
public class TaktModelDestinationUpdateValidator : AbstractValidator<TaktModelDestinationUpdateDto>
{
    /// <summary>
    /// 初始化 更新ModelDestination 校验规则
    /// </summary>
    public TaktModelDestinationUpdateValidator()
    {
        RuleFor(x => x.ModelDestinationId)
            .GreaterThan(0).WithMessage("ModelDestinationID无效");
    }
}

// ========================================
// 导入ModelDestination 验证器
// ========================================

/// <summary>
/// 导入ModelDestination DTO 验证器
/// </summary>
public class TaktModelDestinationImportValidator : AbstractValidator<TaktModelDestinationImportDto>
{
    /// <summary>
    /// 初始化 导入ModelDestination 校验规则
    /// </summary>
    public TaktModelDestinationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("机种名称不能为空")
            .MaximumLength(200).WithMessage("机种名称长度不能超过200个字符");
        RuleFor(x => x.DestinationName)
            .NotEmpty().WithMessage("仕向地名称不能为空")
            .MaximumLength(200).WithMessage("仕向地名称长度不能超过200个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
