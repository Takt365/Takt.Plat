// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityScrapItemValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityScrapItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityScrapItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityScrapItem 验证器
// ========================================

/// <summary>
/// 创建QualityScrapItem DTO 验证器
/// </summary>
public class TaktQualityScrapItemCreateValidator : AbstractValidator<TaktQualityScrapItemCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityScrapItem 校验规则
    /// </summary>
    public TaktQualityScrapItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.QualityScrapId)
            .GreaterThanOrEqualTo(0).WithMessage("品质废弃主表ID不能为负数");
        RuleFor(x => x.QualityScrapCode)
            .NotEmpty().WithMessage("品质废弃编码不能为空")
            .MaximumLength(30).WithMessage("品质废弃编码长度不能超过30个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityScrapItem 验证器
// ========================================

/// <summary>
/// 更新QualityScrapItem DTO 验证器
/// </summary>
public class TaktQualityScrapItemUpdateValidator : AbstractValidator<TaktQualityScrapItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityScrapItem 校验规则
    /// </summary>
    public TaktQualityScrapItemUpdateValidator()
    {
        RuleFor(x => x.QualityScrapItemId)
            .GreaterThan(0).WithMessage("QualityScrapItemID无效");
    }
}

// ========================================
// 导入QualityScrapItem 验证器
// ========================================

/// <summary>
/// 导入QualityScrapItem DTO 验证器
/// </summary>
public class TaktQualityScrapItemImportValidator : AbstractValidator<TaktQualityScrapItemImportDto>
{
    /// <summary>
    /// 初始化 导入QualityScrapItem 校验规则
    /// </summary>
    public TaktQualityScrapItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.QualityScrapId)
            .GreaterThanOrEqualTo(0).WithMessage("品质废弃主表ID不能为负数");
        RuleFor(x => x.QualityScrapCode)
            .NotEmpty().WithMessage("品质废弃编码不能为空")
            .MaximumLength(30).WithMessage("品质废弃编码长度不能超过30个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
