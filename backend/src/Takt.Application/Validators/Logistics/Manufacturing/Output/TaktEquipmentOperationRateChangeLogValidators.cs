// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktEquipmentOperationRateChangeLogValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EquipmentOperationRateChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEquipmentOperationRateChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建EquipmentOperationRateChangeLog 验证器
// ========================================

/// <summary>
/// 创建EquipmentOperationRateChangeLog DTO 验证器
/// </summary>
public class TaktEquipmentOperationRateChangeLogCreateValidator : AbstractValidator<TaktEquipmentOperationRateChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建EquipmentOperationRateChangeLog 校验规则
    /// </summary>
    public TaktEquipmentOperationRateChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EquipmentOperationRateId)
            .GreaterThanOrEqualTo(0).WithMessage("机器稼动率ID不能为负数");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(20).WithMessage("设备编码长度不能超过20个字符");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表长度不能超过4000个字符");
        RuleFor(x => x.ChangeBy)
            .MaximumLength(50).WithMessage("变更人长度不能超过50个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(500).WithMessage("变更原因长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EquipmentOperationRateChangeLog 验证器
// ========================================

/// <summary>
/// 更新EquipmentOperationRateChangeLog DTO 验证器
/// </summary>
public class TaktEquipmentOperationRateChangeLogUpdateValidator : AbstractValidator<TaktEquipmentOperationRateChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新EquipmentOperationRateChangeLog 校验规则
    /// </summary>
    public TaktEquipmentOperationRateChangeLogUpdateValidator()
    {
        RuleFor(x => x.EquipmentOperationRateChangeLogId)
            .GreaterThan(0).WithMessage("EquipmentOperationRateChangeLogID无效");
    }
}
