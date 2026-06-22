// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Maintenance
// 文件名称：TaktMaintenanceNotificationValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceNotification 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaintenanceNotification 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Maintenance;

namespace Takt.Application.Validators.Logistics.Maintenance;

// ========================================
// 创建MaintenanceNotification 验证器
// ========================================

/// <summary>
/// 创建MaintenanceNotification DTO 验证器
/// </summary>
public class TaktMaintenanceNotificationCreateValidator : AbstractValidator<TaktMaintenanceNotificationCreateDto>
{
    /// <summary>
    /// 初始化 创建MaintenanceNotification 校验规则
    /// </summary>
    public TaktMaintenanceNotificationCreateValidator()
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
        RuleFor(x => x.NotificationCode)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(50).WithMessage("通知单号长度不能超过50个字符");
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(50).WithMessage("设备编码长度不能超过50个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(200).WithMessage("设备名称长度不能超过200个字符");
        RuleFor(x => x.FaultDescription)
            .NotEmpty().WithMessage("异常/故障描述不能为空")
            .MaximumLength(2000).WithMessage("异常/故障描述长度不能超过2000个字符");
        RuleFor(x => x.ReportedBy)
            .MaximumLength(50).WithMessage("报告人长度不能超过50个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("责任成本中心ID不能为负数");
        RuleFor(x => x.CostCenterCode)
            .MaximumLength(50).WithMessage("责任成本中心编码长度不能超过50个字符");
        RuleFor(x => x.MaintenanceWorkOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联维护工单ID不能为负数");
        RuleFor(x => x.MaintenanceWorkOrderCode)
            .MaximumLength(50).WithMessage("关联维护工单号长度不能超过50个字符");
        RuleFor(x => x.NotificationImages)
            .MaximumLength(2000).WithMessage("通知图片长度不能超过2000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaintenanceNotification 验证器
// ========================================

/// <summary>
/// 更新MaintenanceNotification DTO 验证器
/// </summary>
public class TaktMaintenanceNotificationUpdateValidator : AbstractValidator<TaktMaintenanceNotificationUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaintenanceNotification 校验规则
    /// </summary>
    public TaktMaintenanceNotificationUpdateValidator()
    {
        RuleFor(x => x.MaintenanceNotificationId)
            .GreaterThan(0).WithMessage("MaintenanceNotificationID无效");
    }
}

// ========================================
// 导入MaintenanceNotification 验证器
// ========================================

/// <summary>
/// 导入MaintenanceNotification DTO 验证器
/// </summary>
public class TaktMaintenanceNotificationImportValidator : AbstractValidator<TaktMaintenanceNotificationImportDto>
{
    /// <summary>
    /// 初始化 导入MaintenanceNotification 校验规则
    /// </summary>
    public TaktMaintenanceNotificationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.NotificationCode)
            .NotEmpty().WithMessage("通知单号不能为空")
            .MaximumLength(50).WithMessage("通知单号长度不能超过50个字符");
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(50).WithMessage("设备编码长度不能超过50个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(200).WithMessage("设备名称长度不能超过200个字符");
        RuleFor(x => x.FaultDescription)
            .NotEmpty().WithMessage("异常/故障描述不能为空")
            .MaximumLength(2000).WithMessage("异常/故障描述长度不能超过2000个字符");
        RuleFor(x => x.ReportedBy)
            .MaximumLength(50).WithMessage("报告人长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ReportedBy));
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("责任成本中心ID不能为负数");
        RuleFor(x => x.CostCenterCode)
            .MaximumLength(50).WithMessage("责任成本中心编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.CostCenterCode));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
