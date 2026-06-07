// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktCycleScheduleValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：CycleSchedule 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCycleSchedule 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建CycleSchedule 验证器
// ========================================

/// <summary>
/// 创建CycleSchedule DTO 验证器
/// </summary>
public class TaktCycleScheduleCreateValidator : AbstractValidator<TaktCycleScheduleCreateDto>
{
    /// <summary>
    /// 初始化 创建CycleSchedule 校验规则
    /// </summary>
    public TaktCycleScheduleCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CycleCode)
            .NotEmpty().WithMessage("周期编码不能为空")
            .MaximumLength(64).WithMessage("周期编码长度不能超过64个字符");
        RuleFor(x => x.CycleName)
            .NotEmpty().WithMessage("周期名称不能为空")
            .MaximumLength(128).WithMessage("周期名称长度不能超过128个字符");
        RuleFor(x => x.CycleType)
            .NotEmpty().WithMessage("周期类型不能为空")
            .MaximumLength(50).WithMessage("周期类型长度不能超过50个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("周期说明不能为空")
            .MaximumLength(500).WithMessage("周期说明长度不能超过500个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CycleSchedule 验证器
// ========================================

/// <summary>
/// 更新CycleSchedule DTO 验证器
/// </summary>
public class TaktCycleScheduleUpdateValidator : AbstractValidator<TaktCycleScheduleUpdateDto>
{
    /// <summary>
    /// 初始化 更新CycleSchedule 校验规则
    /// </summary>
    public TaktCycleScheduleUpdateValidator()
    {
        RuleFor(x => x.CycleScheduleId)
            .GreaterThan(0).WithMessage("CycleScheduleID无效");
    }
}

// ========================================
// 导入CycleSchedule 验证器
// ========================================

/// <summary>
/// 导入CycleSchedule DTO 验证器
/// </summary>
public class TaktCycleScheduleImportValidator : AbstractValidator<TaktCycleScheduleImportDto>
{
    /// <summary>
    /// 初始化 导入CycleSchedule 校验规则
    /// </summary>
    public TaktCycleScheduleImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CycleCode)
            .NotEmpty().WithMessage("周期编码不能为空")
            .MaximumLength(64).WithMessage("周期编码长度不能超过64个字符");
        RuleFor(x => x.CycleName)
            .NotEmpty().WithMessage("周期名称不能为空")
            .MaximumLength(128).WithMessage("周期名称长度不能超过128个字符");
        RuleFor(x => x.CycleType)
            .NotEmpty().WithMessage("周期类型不能为空")
            .MaximumLength(50).WithMessage("周期类型长度不能超过50个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("周期说明不能为空")
            .MaximumLength(500).WithMessage("周期说明长度不能超过500个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
