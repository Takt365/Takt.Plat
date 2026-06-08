// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Maintenance
// 文件名称：TaktMaintenanceValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：Maintenance 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaintenance 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Maintenance;

namespace Takt.Application.Validators.Logistics.Maintenance;

// ========================================
// 创建Maintenance 验证器
// ========================================

/// <summary>
/// 创建Maintenance DTO 验证器
/// </summary>
public class TaktMaintenanceCreateValidator : AbstractValidator<TaktMaintenanceCreateDto>
{
    /// <summary>
    /// 初始化 创建Maintenance 校验规则
    /// </summary>
    public TaktMaintenanceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(50).WithMessage("设备编码长度不能超过50个字符");
        RuleFor(x => x.MaintenanceCompany)
            .MaximumLength(200).WithMessage("维护单位长度不能超过200个字符");
        RuleFor(x => x.MaintenanceTechnician)
            .MaximumLength(50).WithMessage("维护技师长度不能超过50个字符");
        RuleFor(x => x.MaintenanceContent)
            .MaximumLength(2000).WithMessage("维护内容描述长度不能超过2000个字符");
        RuleFor(x => x.FaultDescription)
            .MaximumLength(1000).WithMessage("故障描述长度不能超过1000个字符");
        RuleFor(x => x.Solution)
            .MaximumLength(1000).WithMessage("处理方案长度不能超过1000个字符");
        RuleFor(x => x.UsedParts)
            .MaximumLength(2000).WithMessage("使用配件长度不能超过2000个字符");
        RuleFor(x => x.MaintenanceDocuments)
            .MaximumLength(2000).WithMessage("维护文档长度不能超过2000个字符");
        RuleFor(x => x.MaintenanceImages)
            .MaximumLength(2000).WithMessage("维护图片长度不能超过2000个字符");
        RuleFor(x => x.AcceptedSummary)
            .MaximumLength(500).WithMessage("验收总结长度不能超过500个字符");
        RuleFor(x => x.AcceptedBy)
            .MaximumLength(50).WithMessage("验收人长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Maintenance 验证器
// ========================================

/// <summary>
/// 更新Maintenance DTO 验证器
/// </summary>
public class TaktMaintenanceUpdateValidator : AbstractValidator<TaktMaintenanceUpdateDto>
{
    /// <summary>
    /// 初始化 更新Maintenance 校验规则
    /// </summary>
    public TaktMaintenanceUpdateValidator()
    {
        RuleFor(x => x.MaintenanceId)
            .GreaterThan(0).WithMessage("MaintenanceID无效");
    }
}

// ========================================
// 导入Maintenance 验证器
// ========================================

/// <summary>
/// 导入Maintenance DTO 验证器
/// </summary>
public class TaktMaintenanceImportValidator : AbstractValidator<TaktMaintenanceImportDto>
{
    /// <summary>
    /// 初始化 导入Maintenance 校验规则
    /// </summary>
    public TaktMaintenanceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(50).WithMessage("设备编码长度不能超过50个字符");
        RuleFor(x => x.MaintenanceCompany)
            .MaximumLength(200).WithMessage("维护单位长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.MaintenanceCompany));
        RuleFor(x => x.MaintenanceTechnician)
            .MaximumLength(50).WithMessage("维护技师长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.MaintenanceTechnician));
        RuleFor(x => x.MaintenanceContent)
            .MaximumLength(2000).WithMessage("维护内容描述长度不能超过2000个字符").When(x => !string.IsNullOrWhiteSpace(x.MaintenanceContent));
        RuleFor(x => x.FaultDescription)
            .MaximumLength(1000).WithMessage("故障描述长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.FaultDescription));
        RuleFor(x => x.Solution)
            .MaximumLength(1000).WithMessage("处理方案长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.Solution));
        RuleFor(x => x.UsedParts)
            .MaximumLength(2000).WithMessage("使用配件长度不能超过2000个字符").When(x => !string.IsNullOrWhiteSpace(x.UsedParts));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
