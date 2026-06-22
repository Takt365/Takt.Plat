// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialTransactionValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialTransaction 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialTransaction 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialTransaction 验证器
// ========================================

/// <summary>
/// 创建MaterialTransaction DTO 验证器
/// </summary>
public class TaktMaterialTransactionCreateValidator : AbstractValidator<TaktMaterialTransactionCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialTransaction 校验规则
    /// </summary>
    public TaktMaterialTransactionCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MaterialTransactionCode)
            .NotEmpty().WithMessage("物料交易单号不能为空")
            .MaximumLength(50).WithMessage("物料交易单号长度不能超过50个字符");
        RuleFor(x => x.SourceCode)
            .MaximumLength(50).WithMessage("来源单号长度不能超过50个字符");
        RuleFor(x => x.PartnerCode)
            .MaximumLength(50).WithMessage("往来方编码长度不能超过50个字符");
        RuleFor(x => x.PartnerName)
            .MaximumLength(200).WithMessage("往来方名称长度不能超过200个字符");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("源仓库编码不能为空")
            .MaximumLength(50).WithMessage("源仓库编码长度不能超过50个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("源库位编码不能为空")
            .MaximumLength(50).WithMessage("源库位编码长度不能超过50个字符");
        RuleFor(x => x.TargetWarehouseCode)
            .MaximumLength(50).WithMessage("目标仓库编码长度不能超过50个字符");
        RuleFor(x => x.TargetLocationCode)
            .MaximumLength(50).WithMessage("目标库位编码长度不能超过50个字符");
        RuleFor(x => x.RelatedCompany)
            .NotEmpty().WithMessage("关联公司不能为空")
            .MaximumLength(4).WithMessage("关联公司长度不能超过4个字符");
        RuleFor(x => x.PostedBy)
            .MaximumLength(50).WithMessage("过账人长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialTransaction 验证器
// ========================================

/// <summary>
/// 更新MaterialTransaction DTO 验证器
/// </summary>
public class TaktMaterialTransactionUpdateValidator : AbstractValidator<TaktMaterialTransactionUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialTransaction 校验规则
    /// </summary>
    public TaktMaterialTransactionUpdateValidator()
    {
        RuleFor(x => x.MaterialTransactionId)
            .GreaterThan(0).WithMessage("MaterialTransactionID无效");
    }
}

// ========================================
// 导入MaterialTransaction 验证器
// ========================================

/// <summary>
/// 导入MaterialTransaction DTO 验证器
/// </summary>
public class TaktMaterialTransactionImportValidator : AbstractValidator<TaktMaterialTransactionImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialTransaction 校验规则
    /// </summary>
    public TaktMaterialTransactionImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MaterialTransactionCode)
            .NotEmpty().WithMessage("物料交易单号不能为空")
            .MaximumLength(50).WithMessage("物料交易单号长度不能超过50个字符");
        RuleFor(x => x.SourceCode)
            .MaximumLength(50).WithMessage("来源单号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SourceCode));
        RuleFor(x => x.PartnerCode)
            .MaximumLength(50).WithMessage("往来方编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PartnerCode));
        RuleFor(x => x.PartnerName)
            .MaximumLength(200).WithMessage("往来方名称长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.PartnerName));
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("源仓库编码不能为空")
            .MaximumLength(50).WithMessage("源仓库编码长度不能超过50个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("源库位编码不能为空")
            .MaximumLength(50).WithMessage("源库位编码长度不能超过50个字符");
        RuleFor(x => x.TargetWarehouseCode)
            .MaximumLength(50).WithMessage("目标仓库编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TargetWarehouseCode));
        RuleFor(x => x.TargetLocationCode)
            .MaximumLength(50).WithMessage("目标库位编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TargetLocationCode));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
