// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleItemValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsScheduleItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktApsScheduleItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;

namespace Takt.Application.Validators.Logistics.Manufacturing.Scheduling;

// ========================================
// 创建ApsScheduleItem 验证器
// ========================================

/// <summary>
/// 创建ApsScheduleItem DTO 验证器
/// </summary>
public class TaktApsScheduleItemCreateValidator : AbstractValidator<TaktApsScheduleItemCreateDto>
{
    /// <summary>
    /// 初始化 创建ApsScheduleItem 校验规则
    /// </summary>
    public TaktApsScheduleItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ApsScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("APS排程ID不能为负数");
        RuleFor(x => x.ApsScheduleCode)
            .NotEmpty().WithMessage("APS排程编码不能为空")
            .MaximumLength(50).WithMessage("APS排程编码长度不能超过50个字符");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("生产工单编码不能为空")
            .MaximumLength(50).WithMessage("生产工单编码长度不能超过50个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(50).WithMessage("产品编码长度不能超过50个字符");
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("产品名称不能为空")
            .MaximumLength(200).WithMessage("产品名称长度不能超过200个字符");
        RuleFor(x => x.WorkCenterCode)
            .MaximumLength(50).WithMessage("工作中心编码长度不能超过50个字符");
        RuleFor(x => x.WorkCenterName)
            .MaximumLength(200).WithMessage("工作中心名称长度不能超过200个字符");
        RuleFor(x => x.ProcessCode)
            .NotEmpty().WithMessage("工序编码不能为空")
            .MaximumLength(50).WithMessage("工序编码长度不能超过50个字符");
        RuleFor(x => x.ProcessName)
            .NotEmpty().WithMessage("工序名称不能为空")
            .MaximumLength(200).WithMessage("工序名称长度不能超过200个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ApsScheduleItem 验证器
// ========================================

/// <summary>
/// 更新ApsScheduleItem DTO 验证器
/// </summary>
public class TaktApsScheduleItemUpdateValidator : AbstractValidator<TaktApsScheduleItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新ApsScheduleItem 校验规则
    /// </summary>
    public TaktApsScheduleItemUpdateValidator()
    {
        RuleFor(x => x.ApsScheduleItemId)
            .GreaterThan(0).WithMessage("ApsScheduleItemID无效");
    }
}

// ========================================
// 导入ApsScheduleItem 验证器
// ========================================

/// <summary>
/// 导入ApsScheduleItem DTO 验证器
/// </summary>
public class TaktApsScheduleItemImportValidator : AbstractValidator<TaktApsScheduleItemImportDto>
{
    /// <summary>
    /// 初始化 导入ApsScheduleItem 校验规则
    /// </summary>
    public TaktApsScheduleItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ApsScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("APS排程ID不能为负数");
        RuleFor(x => x.ApsScheduleCode)
            .NotEmpty().WithMessage("APS排程编码不能为空")
            .MaximumLength(50).WithMessage("APS排程编码长度不能超过50个字符");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("生产工单编码不能为空")
            .MaximumLength(50).WithMessage("生产工单编码长度不能超过50个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(50).WithMessage("产品编码长度不能超过50个字符");
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("产品名称不能为空")
            .MaximumLength(200).WithMessage("产品名称长度不能超过200个字符");
        RuleFor(x => x.WorkCenterCode)
            .MaximumLength(50).WithMessage("工作中心编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkCenterCode));
        RuleFor(x => x.WorkCenterName)
            .MaximumLength(200).WithMessage("工作中心名称长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkCenterName));
        RuleFor(x => x.ProcessCode)
            .NotEmpty().WithMessage("工序编码不能为空")
            .MaximumLength(50).WithMessage("工序编码长度不能超过50个字符");
        RuleFor(x => x.ProcessName)
            .NotEmpty().WithMessage("工序名称不能为空")
            .MaximumLength(200).WithMessage("工序名称长度不能超过200个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
