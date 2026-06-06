// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktObjectiveValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Objective 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktObjective 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建Objective 验证器
// ========================================

/// <summary>
/// 创建Objective DTO 验证器
/// </summary>
public class TaktObjectiveCreateValidator : AbstractValidator<TaktObjectiveCreateDto>
{
    /// <summary>
    /// 初始化 创建Objective 校验规则
    /// </summary>
    public TaktObjectiveCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(50).WithMessage("员工姓名长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标 ID不能为负数");
        RuleFor(x => x.ObjectivePeriod)
            .NotEmpty().WithMessage("目标周期不能为空")
            .MaximumLength(50).WithMessage("目标周期长度不能超过50个字符");
        RuleFor(x => x.ObjectiveDescription)
            .NotEmpty().WithMessage("目标描述不能为空")
            .MaximumLength(500).WithMessage("目标描述长度不能超过500个字符");
        RuleFor(x => x.AchievementNotes)
            .NotEmpty().WithMessage("目标达成说明不能为空")
            .MaximumLength(1000).WithMessage("目标达成说明长度不能超过1000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Objective 验证器
// ========================================

/// <summary>
/// 更新Objective DTO 验证器
/// </summary>
public class TaktObjectiveUpdateValidator : AbstractValidator<TaktObjectiveUpdateDto>
{
    /// <summary>
    /// 初始化 更新Objective 校验规则
    /// </summary>
    public TaktObjectiveUpdateValidator()
    {
        RuleFor(x => x.ObjectiveId)
            .GreaterThan(0).WithMessage("ObjectiveID无效");
    }
}

// ========================================
// 导入Objective 验证器
// ========================================

/// <summary>
/// 导入Objective DTO 验证器
/// </summary>
public class TaktObjectiveImportValidator : AbstractValidator<TaktObjectiveImportDto>
{
    /// <summary>
    /// 初始化 导入Objective 校验规则
    /// </summary>
    public TaktObjectiveImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(50).WithMessage("员工姓名长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标 ID不能为负数");
        RuleFor(x => x.ObjectivePeriod)
            .NotEmpty().WithMessage("目标周期不能为空")
            .MaximumLength(50).WithMessage("目标周期长度不能超过50个字符");
        RuleFor(x => x.ObjectiveDescription)
            .NotEmpty().WithMessage("目标描述不能为空")
            .MaximumLength(500).WithMessage("目标描述长度不能超过500个字符");
        RuleFor(x => x.AchievementNotes)
            .NotEmpty().WithMessage("目标达成说明不能为空")
            .MaximumLength(1000).WithMessage("目标达成说明长度不能超过1000个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
