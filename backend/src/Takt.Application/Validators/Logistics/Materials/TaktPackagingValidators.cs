// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktPackagingValidators.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：Packaging 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPackaging 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建Packaging 验证器
// ========================================

/// <summary>
/// 创建Packaging DTO 验证器
/// </summary>
public class TaktPackagingCreateValidator : AbstractValidator<TaktPackagingCreateDto>
{
    /// <summary>
    /// 初始化 创建Packaging 校验规则
    /// </summary>
    public TaktPackagingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.WeightUnit)
            .NotEmpty().WithMessage("重量单位不能为空")
            .MaximumLength(10).WithMessage("重量单位长度不能超过10个字符");
        RuleFor(x => x.VolumeUnit)
            .NotEmpty().WithMessage("体积单位不能为空")
            .MaximumLength(10).WithMessage("体积单位长度不能超过10个字符");
        RuleFor(x => x.PackagingType)
            .NotEmpty().WithMessage("包装类型不能为空")
            .MaximumLength(50).WithMessage("包装类型长度不能超过50个字符");
        RuleFor(x => x.PackingUnit)
            .NotEmpty().WithMessage("包装单位不能为空")
            .MaximumLength(20).WithMessage("包装单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Packaging 验证器
// ========================================

/// <summary>
/// 更新Packaging DTO 验证器
/// </summary>
public class TaktPackagingUpdateValidator : AbstractValidator<TaktPackagingUpdateDto>
{
    /// <summary>
    /// 初始化 更新Packaging 校验规则
    /// </summary>
    public TaktPackagingUpdateValidator()
    {
        RuleFor(x => x.PackagingId)
            .GreaterThan(0).WithMessage("PackagingID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.WeightUnit)
            .NotEmpty().WithMessage("重量单位不能为空")
            .MaximumLength(10).WithMessage("重量单位长度不能超过10个字符");
        RuleFor(x => x.VolumeUnit)
            .NotEmpty().WithMessage("体积单位不能为空")
            .MaximumLength(10).WithMessage("体积单位长度不能超过10个字符");
        RuleFor(x => x.PackagingType)
            .NotEmpty().WithMessage("包装类型不能为空")
            .MaximumLength(50).WithMessage("包装类型长度不能超过50个字符");
        RuleFor(x => x.PackingUnit)
            .NotEmpty().WithMessage("包装单位不能为空")
            .MaximumLength(20).WithMessage("包装单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Packaging 验证器
// ========================================

/// <summary>
/// 导入Packaging DTO 验证器
/// </summary>
public class TaktPackagingImportValidator : AbstractValidator<TaktPackagingImportDto>
{
    /// <summary>
    /// 初始化 导入Packaging 校验规则
    /// </summary>
    public TaktPackagingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.WeightUnit)
            .NotEmpty().WithMessage("重量单位不能为空")
            .MaximumLength(10).WithMessage("重量单位长度不能超过10个字符");
        RuleFor(x => x.VolumeUnit)
            .NotEmpty().WithMessage("体积单位不能为空")
            .MaximumLength(10).WithMessage("体积单位长度不能超过10个字符");
        RuleFor(x => x.PackagingType)
            .NotEmpty().WithMessage("包装类型不能为空")
            .MaximumLength(50).WithMessage("包装类型长度不能超过50个字符");
        RuleFor(x => x.PackingUnit)
            .NotEmpty().WithMessage("包装单位不能为空")
            .MaximumLength(20).WithMessage("包装单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
