// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialPlantValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialPlant 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialPlant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialPlant 验证器
// ========================================

/// <summary>
/// 创建MaterialPlant DTO 验证器
/// </summary>
public class TaktMaterialPlantCreateValidator : AbstractValidator<TaktMaterialPlantCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialPlant 校验规则
    /// </summary>
    public TaktMaterialPlantCreateValidator()
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
        RuleFor(x => x.MaterialSpecification)
            .MaximumLength(80).WithMessage("物料规格长度不能超过80个字符");
        RuleFor(x => x.MaterialDescription)
            .MaximumLength(80).WithMessage("物料描述长度不能超过80个字符");
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符");
        RuleFor(x => x.MaterialHierarchy)
            .MaximumLength(200).WithMessage("品目阶层长度不能超过200个字符");
        RuleFor(x => x.MaterialGroupCode)
            .MaximumLength(20).WithMessage("品目组代码长度不能超过20个字符");
        RuleFor(x => x.MaterialModel)
            .MaximumLength(100).WithMessage("物料型号长度不能超过100个字符");
        RuleFor(x => x.MaterialBrand)
            .MaximumLength(100).WithMessage("物料品牌长度不能超过100个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本单位不能为空")
            .MaximumLength(20).WithMessage("基本单位长度不能超过20个字符");
        RuleFor(x => x.PurchaseGroup)
            .MaximumLength(50).WithMessage("采购组编码长度不能超过50个字符");
        RuleFor(x => x.Manufacturer)
            .MaximumLength(200).WithMessage("制造商长度不能超过200个字符");
        RuleFor(x => x.ManufacturerPartNumber)
            .MaximumLength(100).WithMessage("制造商零件编号长度不能超过100个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种代码不能为空")
            .MaximumLength(10).WithMessage("币种代码长度不能超过10个字符");
        RuleFor(x => x.ValuationCategory)
            .MaximumLength(50).WithMessage("评估类别代码长度不能超过50个字符");
        RuleFor(x => x.DifferenceCode)
            .MaximumLength(50).WithMessage("差异码长度不能超过50个字符");
        RuleFor(x => x.ProfitCenter)
            .MaximumLength(50).WithMessage("利润中心长度不能超过50个字符");
        RuleFor(x => x.ProductionLocation)
            .MaximumLength(50).WithMessage("生产地点长度不能超过50个字符");
        RuleFor(x => x.PurchasingLocation)
            .MaximumLength(50).WithMessage("采购地点长度不能超过50个字符");
        RuleFor(x => x.MaterialAttributes)
            .MaximumLength(4000).WithMessage("物料属性长度不能超过4000个字符");
        RuleFor(x => x.IsEndOfLife)
            .MaximumLength(10).WithMessage("停产状态长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialPlant 验证器
// ========================================

/// <summary>
/// 更新MaterialPlant DTO 验证器
/// </summary>
public class TaktMaterialPlantUpdateValidator : AbstractValidator<TaktMaterialPlantUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialPlant 校验规则
    /// </summary>
    public TaktMaterialPlantUpdateValidator()
    {
        RuleFor(x => x.MaterialPlantId)
            .GreaterThan(0).WithMessage("MaterialPlantID无效");
    }
}

// ========================================
// 导入MaterialPlant 验证器
// ========================================

/// <summary>
/// 导入MaterialPlant DTO 验证器
/// </summary>
public class TaktMaterialPlantImportValidator : AbstractValidator<TaktMaterialPlantImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialPlant 校验规则
    /// </summary>
    public TaktMaterialPlantImportValidator()
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
        RuleFor(x => x.MaterialSpecification)
            .MaximumLength(80).WithMessage("物料规格长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialSpecification));
        RuleFor(x => x.MaterialDescription)
            .MaximumLength(80).WithMessage("物料描述长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialDescription));
        RuleFor(x => x.IndustrySector)
            .MaximumLength(50).WithMessage("行业领域长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.IndustrySector));
        RuleFor(x => x.MaterialHierarchy)
            .MaximumLength(200).WithMessage("品目阶层长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialHierarchy));
        RuleFor(x => x.MaterialGroupCode)
            .MaximumLength(20).WithMessage("品目组代码长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialGroupCode));
        RuleFor(x => x.MaterialModel)
            .MaximumLength(100).WithMessage("物料型号长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialModel));
        RuleFor(x => x.MaterialBrand)
            .MaximumLength(100).WithMessage("物料品牌长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialBrand));
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本单位不能为空")
            .MaximumLength(20).WithMessage("基本单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
