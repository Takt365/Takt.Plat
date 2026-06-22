// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialTransactionItemValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialTransactionItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialTransactionItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialTransactionItem 验证器
// ========================================

/// <summary>
/// 创建MaterialTransactionItem DTO 验证器
/// </summary>
public class TaktMaterialTransactionItemCreateValidator : AbstractValidator<TaktMaterialTransactionItemCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialTransactionItem 校验规则
    /// </summary>
    public TaktMaterialTransactionItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.MaterialTransactionId)
            .GreaterThanOrEqualTo(0).WithMessage("物料交易ID不能为负数");
        RuleFor(x => x.MaterialTransactionCode)
            .NotEmpty().WithMessage("物料交易单号不能为空")
            .MaximumLength(50).WithMessage("物料交易单号长度不能超过50个字符");
        RuleFor(x => x.SourceCode)
            .MaximumLength(50).WithMessage("来源单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.MaterialSpecification)
            .MaximumLength(80).WithMessage("物料规格长度不能超过80个字符");
        RuleFor(x => x.TransactionUnit)
            .NotEmpty().WithMessage("交易单位不能为空")
            .MaximumLength(20).WithMessage("交易单位长度不能超过20个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(50).WithMessage("批次号长度不能超过50个字符");
        RuleFor(x => x.WarehouseCode)
            .MaximumLength(50).WithMessage("源仓库编码长度不能超过50个字符");
        RuleFor(x => x.LocationCode)
            .MaximumLength(50).WithMessage("源库位编码长度不能超过50个字符");
        RuleFor(x => x.TargetWarehouseCode)
            .MaximumLength(50).WithMessage("目标仓库编码长度不能超过50个字符");
        RuleFor(x => x.TargetLocationCode)
            .MaximumLength(50).WithMessage("目标库位编码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialTransactionItem 验证器
// ========================================

/// <summary>
/// 更新MaterialTransactionItem DTO 验证器
/// </summary>
public class TaktMaterialTransactionItemUpdateValidator : AbstractValidator<TaktMaterialTransactionItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialTransactionItem 校验规则
    /// </summary>
    public TaktMaterialTransactionItemUpdateValidator()
    {
        RuleFor(x => x.MaterialTransactionItemId)
            .GreaterThan(0).WithMessage("MaterialTransactionItemID无效");
    }
}

// ========================================
// 导入MaterialTransactionItem 验证器
// ========================================

/// <summary>
/// 导入MaterialTransactionItem DTO 验证器
/// </summary>
public class TaktMaterialTransactionItemImportValidator : AbstractValidator<TaktMaterialTransactionItemImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialTransactionItem 校验规则
    /// </summary>
    public TaktMaterialTransactionItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.MaterialTransactionId)
            .GreaterThanOrEqualTo(0).WithMessage("物料交易ID不能为负数");
        RuleFor(x => x.MaterialTransactionCode)
            .NotEmpty().WithMessage("物料交易单号不能为空")
            .MaximumLength(50).WithMessage("物料交易单号长度不能超过50个字符");
        RuleFor(x => x.SourceCode)
            .MaximumLength(50).WithMessage("来源单号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SourceCode));
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.MaterialSpecification)
            .MaximumLength(80).WithMessage("物料规格长度不能超过80个字符").When(x => !string.IsNullOrWhiteSpace(x.MaterialSpecification));
        RuleFor(x => x.TransactionUnit)
            .NotEmpty().WithMessage("交易单位不能为空")
            .MaximumLength(20).WithMessage("交易单位长度不能超过20个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(50).WithMessage("批次号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.BatchNo));
        RuleFor(x => x.WarehouseCode)
            .MaximumLength(50).WithMessage("源仓库编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.WarehouseCode));
        RuleFor(x => x.LocationCode)
            .MaximumLength(50).WithMessage("源库位编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.LocationCode));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
