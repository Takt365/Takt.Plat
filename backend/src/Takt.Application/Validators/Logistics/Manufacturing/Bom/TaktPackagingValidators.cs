// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktPackagingValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Packaging 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPackaging 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

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
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.HsCode)
            .MaximumLength(20).WithMessage("海关商品编码长度不能超过20个字符");
        RuleFor(x => x.HsName)
            .MaximumLength(500).WithMessage("商品名称长度不能超过500个字符");
        RuleFor(x => x.AdditionalCode)
            .MaximumLength(20).WithMessage("附加编码长度不能超过20个字符");
        RuleFor(x => x.OriginCountryRegionCode)
            .MaximumLength(20).WithMessage("原产国/地区编码长度不能超过20个字符");
        RuleFor(x => x.OriginCountryRegionName)
            .MaximumLength(100).WithMessage("原产国/地区名称长度不能超过100个字符");
        RuleFor(x => x.DestinationCountryRegionCode)
            .MaximumLength(20).WithMessage("目的国/地区编码长度不能超过20个字符");
        RuleFor(x => x.DestinationCountryRegionName)
            .MaximumLength(100).WithMessage("目的国/地区名称长度不能超过100个字符");
        RuleFor(x => x.RegulatoryConditionCode)
            .MaximumLength(50).WithMessage("监管条件代码长度不能超过50个字符");
        RuleFor(x => x.TariffRateType)
            .MaximumLength(50).WithMessage("税率/协定税率标识长度不能超过50个字符");
        RuleFor(x => x.WeightUnit)
            .NotEmpty().WithMessage("重量单位不能为空")
            .MaximumLength(10).WithMessage("重量单位长度不能超过10个字符");
        RuleFor(x => x.VolumeUnit)
            .NotEmpty().WithMessage("体积单位不能为空")
            .MaximumLength(10).WithMessage("体积单位长度不能超过10个字符");
        RuleFor(x => x.SizeDimension)
            .MaximumLength(50).WithMessage("大小/量纲长度不能超过50个字符");
        RuleFor(x => x.PackagingType)
            .NotEmpty().WithMessage("包装类型不能为空")
            .MaximumLength(50).WithMessage("包装类型长度不能超过50个字符");
        RuleFor(x => x.PackingUnit)
            .NotEmpty().WithMessage("包装单位不能为空")
            .MaximumLength(20).WithMessage("包装单位长度不能超过20个字符");
        RuleFor(x => x.PackagingSpec)
            .MaximumLength(200).WithMessage("包装规格长度不能超过200个字符");
        RuleFor(x => x.PackagingDescription)
            .MaximumLength(500).WithMessage("包装描述长度不能超过500个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
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
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.HsCode)
            .MaximumLength(20).WithMessage("海关商品编码长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.HsCode));
        RuleFor(x => x.HsName)
            .MaximumLength(500).WithMessage("商品名称长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.HsName));
        RuleFor(x => x.AdditionalCode)
            .MaximumLength(20).WithMessage("附加编码长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.AdditionalCode));
        RuleFor(x => x.OriginCountryRegionCode)
            .MaximumLength(20).WithMessage("原产国/地区编码长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.OriginCountryRegionCode));
        RuleFor(x => x.OriginCountryRegionName)
            .MaximumLength(100).WithMessage("原产国/地区名称长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.OriginCountryRegionName));
        RuleFor(x => x.DestinationCountryRegionCode)
            .MaximumLength(20).WithMessage("目的国/地区编码长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.DestinationCountryRegionCode));
        RuleFor(x => x.DestinationCountryRegionName)
            .MaximumLength(100).WithMessage("目的国/地区名称长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.DestinationCountryRegionName));
        RuleFor(x => x.RegulatoryConditionCode)
            .MaximumLength(50).WithMessage("监管条件代码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.RegulatoryConditionCode));
        RuleFor(x => x.TariffRateType)
            .MaximumLength(50).WithMessage("税率/协定税率标识长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TariffRateType));
        RuleFor(x => x.WeightUnit)
            .NotEmpty().WithMessage("重量单位不能为空")
            .MaximumLength(10).WithMessage("重量单位长度不能超过10个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
