// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsSchedule 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktApsSchedule 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;

namespace Takt.Application.Validators.Logistics.Manufacturing.Scheduling;

// ========================================
// 创建ApsSchedule 验证器
// ========================================

/// <summary>
/// 创建ApsSchedule DTO 验证器
/// </summary>
public class TaktApsScheduleCreateValidator : AbstractValidator<TaktApsScheduleCreateDto>
{
    /// <summary>
    /// 初始化 创建ApsSchedule 校验规则
    /// </summary>
    public TaktApsScheduleCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂编码不能为空")
            .MaximumLength(40).WithMessage("工厂编码长度不能超过40个字符");
        RuleFor(x => x.ScheduleCode)
            .NotEmpty().WithMessage("排程编码不能为空")
            .MaximumLength(40).WithMessage("排程编码长度不能超过40个字符");
        RuleFor(x => x.ScheduleName)
            .NotEmpty().WithMessage("排程名称不能为空")
            .MaximumLength(40).WithMessage("排程名称长度不能超过40个字符");
        RuleFor(x => x.WorkshopCode)
            .MaximumLength(40).WithMessage("车间编码长度不能超过40个字符");
        RuleFor(x => x.WorkshopName)
            .MaximumLength(40).WithMessage("车间名称长度不能超过40个字符");
        RuleFor(x => x.ProductionLineCode)
            .MaximumLength(40).WithMessage("生产线编码长度不能超过40个字符");
        RuleFor(x => x.ProductionLineName)
            .MaximumLength(40).WithMessage("生产线名称长度不能超过40个字符");
        RuleFor(x => x.PlannerId)
            .GreaterThanOrEqualTo(0).WithMessage("计划员ID不能为负数");
        RuleFor(x => x.PlannerName)
            .MaximumLength(40).WithMessage("计划员姓名长度不能超过40个字符");
        RuleFor(x => x.PublishUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发布人ID不能为负数");
        RuleFor(x => x.PublishUserName)
            .MaximumLength(40).WithMessage("发布人姓名长度不能超过40个字符");
        RuleFor(x => x.ScheduleDescription)
            .MaximumLength(1000).WithMessage("排程说明长度不能超过1000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ApsSchedule 验证器
// ========================================

/// <summary>
/// 更新ApsSchedule DTO 验证器
/// </summary>
public class TaktApsScheduleUpdateValidator : AbstractValidator<TaktApsScheduleUpdateDto>
{
    /// <summary>
    /// 初始化 更新ApsSchedule 校验规则
    /// </summary>
    public TaktApsScheduleUpdateValidator()
    {
        RuleFor(x => x.ApsScheduleId)
            .GreaterThan(0).WithMessage("ApsScheduleID无效");
    }
}

// ========================================
// 导入ApsSchedule 验证器
// ========================================

/// <summary>
/// 导入ApsSchedule DTO 验证器
/// </summary>
public class TaktApsScheduleImportValidator : AbstractValidator<TaktApsScheduleImportDto>
{
    /// <summary>
    /// 初始化 导入ApsSchedule 校验规则
    /// </summary>
    public TaktApsScheduleImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂编码不能为空")
            .MaximumLength(40).WithMessage("工厂编码长度不能超过40个字符");
        RuleFor(x => x.ScheduleCode)
            .NotEmpty().WithMessage("排程编码不能为空")
            .MaximumLength(40).WithMessage("排程编码长度不能超过40个字符");
        RuleFor(x => x.ScheduleName)
            .NotEmpty().WithMessage("排程名称不能为空")
            .MaximumLength(40).WithMessage("排程名称长度不能超过40个字符");
        RuleFor(x => x.WorkshopCode)
            .MaximumLength(40).WithMessage("车间编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkshopCode));
        RuleFor(x => x.WorkshopName)
            .MaximumLength(40).WithMessage("车间名称长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkshopName));
        RuleFor(x => x.ProductionLineCode)
            .MaximumLength(40).WithMessage("生产线编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionLineCode));
        RuleFor(x => x.ProductionLineName)
            .MaximumLength(40).WithMessage("生产线名称长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionLineName));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
