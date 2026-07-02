// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialPlantChangeLogValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialPlantChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialPlantChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialPlantChangeLog 验证器
// ========================================

/// <summary>
/// 创建MaterialPlantChangeLog DTO 验证器
/// </summary>
public class TaktMaterialPlantChangeLogCreateValidator : AbstractValidator<TaktMaterialPlantChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialPlantChangeLog 校验规则
    /// </summary>
    public TaktMaterialPlantChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.MaterialPlantId)
            .GreaterThanOrEqualTo(0).WithMessage("工厂物料 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialPlantChangeLog 验证器
// ========================================

/// <summary>
/// 更新MaterialPlantChangeLog DTO 验证器
/// </summary>
public class TaktMaterialPlantChangeLogUpdateValidator : AbstractValidator<TaktMaterialPlantChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialPlantChangeLog 校验规则
    /// </summary>
    public TaktMaterialPlantChangeLogUpdateValidator()
    {
        RuleFor(x => x.MaterialPlantChangeLogId)
            .GreaterThan(0).WithMessage("MaterialPlantChangeLogID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.MaterialPlantId)
            .GreaterThanOrEqualTo(0).WithMessage("工厂物料 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
