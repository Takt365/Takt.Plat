// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：EcDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcDetail 验证器
// ========================================

/// <summary>
/// 创建EcDetail DTO 验证器
/// </summary>
public class TaktEcDetailCreateValidator : AbstractValidator<TaktEcDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建EcDetail 校验规则
    /// </summary>
    public TaktEcDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcModel)
            .NotEmpty().WithMessage("型号不能为空")
            .MaximumLength(50).WithMessage("型号长度不能超过50个字符");
        RuleFor(x => x.EcBomItem)
            .MaximumLength(50).WithMessage("BOM 主项料号长度不能超过50个字符");
        RuleFor(x => x.EcBomSubItem)
            .MaximumLength(50).WithMessage("BOM 子项料号长度不能超过50个字符");
        RuleFor(x => x.EcBomNo)
            .MaximumLength(50).WithMessage("BOM 编号长度不能超过50个字符");
        RuleFor(x => x.EcChange)
            .MaximumLength(500).WithMessage("变更内容长度不能超过500个字符");
        RuleFor(x => x.EcLocal)
            .MaximumLength(50).WithMessage("本地/现场长度不能超过50个字符");
        RuleFor(x => x.EcNote)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
        RuleFor(x => x.EcProcess)
            .MaximumLength(50).WithMessage("工序长度不能超过50个字符");
        RuleFor(x => x.EcOldItem)
            .MaximumLength(50).WithMessage("旧料号长度不能超过50个字符");
        RuleFor(x => x.EcOldText)
            .MaximumLength(200).WithMessage("旧料号描述长度不能超过200个字符");
        RuleFor(x => x.EcOldSet)
            .MaximumLength(20).WithMessage("旧单位/设置长度不能超过20个字符");
        RuleFor(x => x.EcNewItem)
            .MaximumLength(50).WithMessage("新料号长度不能超过50个字符");
        RuleFor(x => x.EcNewText)
            .MaximumLength(200).WithMessage("新料号描述长度不能超过200个字符");
        RuleFor(x => x.EcNewSet)
            .MaximumLength(20).WithMessage("新单位/设置长度不能超过20个字符");
        RuleFor(x => x.EcWarehouse)
            .MaximumLength(50).WithMessage("仓库长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcDetail 验证器
// ========================================

/// <summary>
/// 更新EcDetail DTO 验证器
/// </summary>
public class TaktEcDetailUpdateValidator : AbstractValidator<TaktEcDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcDetail 校验规则
    /// </summary>
    public TaktEcDetailUpdateValidator()
    {
        RuleFor(x => x.EcDetailId)
            .GreaterThan(0).WithMessage("EcDetailID无效");
    }
}

// ========================================
// 导入EcDetail 验证器
// ========================================

/// <summary>
/// 导入EcDetail DTO 验证器
/// </summary>
public class TaktEcDetailImportValidator : AbstractValidator<TaktEcDetailImportDto>
{
    /// <summary>
    /// 初始化 导入EcDetail 校验规则
    /// </summary>
    public TaktEcDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcModel)
            .NotEmpty().WithMessage("型号不能为空")
            .MaximumLength(50).WithMessage("型号长度不能超过50个字符");
        RuleFor(x => x.EcBomItem)
            .MaximumLength(50).WithMessage("BOM 主项料号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcBomItem));
        RuleFor(x => x.EcBomSubItem)
            .MaximumLength(50).WithMessage("BOM 子项料号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcBomSubItem));
        RuleFor(x => x.EcBomNo)
            .MaximumLength(50).WithMessage("BOM 编号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcBomNo));
        RuleFor(x => x.EcChange)
            .MaximumLength(500).WithMessage("变更内容长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.EcChange));
        RuleFor(x => x.EcLocal)
            .MaximumLength(50).WithMessage("本地/现场长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcLocal));
        RuleFor(x => x.EcNote)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.EcNote));
        RuleFor(x => x.EcProcess)
            .MaximumLength(50).WithMessage("工序长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcProcess));
        RuleFor(x => x.EcOldItem)
            .MaximumLength(50).WithMessage("旧料号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.EcOldItem));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
