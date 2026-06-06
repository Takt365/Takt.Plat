// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktEquipmentOperationRateValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EquipmentOperationRate 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEquipmentOperationRate 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建EquipmentOperationRate 验证器
// ========================================

/// <summary>
/// 创建EquipmentOperationRate DTO 验证器
/// </summary>
public class TaktEquipmentOperationRateCreateValidator : AbstractValidator<TaktEquipmentOperationRateCreateDto>
{
    /// <summary>
    /// 初始化 创建EquipmentOperationRate 校验规则
    /// </summary>
    public TaktEquipmentOperationRateCreateValidator()
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
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(20).WithMessage("设备编码长度不能超过20个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(100).WithMessage("设备名称长度不能超过100个字符");
        RuleFor(x => x.ProductionLine)
            .MaximumLength(20).WithMessage("生产线长度不能超过20个字符");
        RuleFor(x => x.DowntimeReason)
            .MaximumLength(500).WithMessage("停机原因描述长度不能超过500个字符");
        RuleFor(x => x.EquipmentOperator)
            .MaximumLength(50).WithMessage("设备操作员长度不能超过50个字符");
        RuleFor(x => x.EquipmentMaintainer)
            .MaximumLength(50).WithMessage("设备维护员长度不能超过50个字符");
        RuleFor(x => x.TeamLeader)
            .MaximumLength(50).WithMessage("班组长长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EquipmentOperationRate 验证器
// ========================================

/// <summary>
/// 更新EquipmentOperationRate DTO 验证器
/// </summary>
public class TaktEquipmentOperationRateUpdateValidator : AbstractValidator<TaktEquipmentOperationRateUpdateDto>
{
    /// <summary>
    /// 初始化 更新EquipmentOperationRate 校验规则
    /// </summary>
    public TaktEquipmentOperationRateUpdateValidator()
    {
        RuleFor(x => x.EquipmentOperationRateId)
            .GreaterThan(0).WithMessage("EquipmentOperationRateID无效");
    }
}

// ========================================
// 导入EquipmentOperationRate 验证器
// ========================================

/// <summary>
/// 导入EquipmentOperationRate DTO 验证器
/// </summary>
public class TaktEquipmentOperationRateImportValidator : AbstractValidator<TaktEquipmentOperationRateImportDto>
{
    /// <summary>
    /// 初始化 导入EquipmentOperationRate 校验规则
    /// </summary>
    public TaktEquipmentOperationRateImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(20).WithMessage("设备编码长度不能超过20个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(100).WithMessage("设备名称长度不能超过100个字符");
        RuleFor(x => x.ProductionLine)
            .MaximumLength(20).WithMessage("生产线长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionLine));
        RuleFor(x => x.DowntimeReason)
            .MaximumLength(500).WithMessage("停机原因描述长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.DowntimeReason));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
