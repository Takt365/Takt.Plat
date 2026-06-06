// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktSchemeMetricValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SchemeMetric 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSchemeMetric 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建SchemeMetric 验证器
// ========================================

/// <summary>
/// 创建SchemeMetric DTO 验证器
/// </summary>
public class TaktSchemeMetricCreateValidator : AbstractValidator<TaktSchemeMetricCreateDto>
{
    /// <summary>
    /// 初始化 创建SchemeMetric 校验规则
    /// </summary>
    public TaktSchemeMetricCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SchemeCode)
            .NotEmpty().WithMessage("方案编码不能为空")
            .MaximumLength(64).WithMessage("方案编码长度不能超过64个字符");
        RuleFor(x => x.SchemeName)
            .NotEmpty().WithMessage("方案名称不能为空")
            .MaximumLength(128).WithMessage("方案名称长度不能超过128个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.CycleType)
            .NotEmpty().WithMessage("考核周期类型不能为空")
            .MaximumLength(50).WithMessage("考核周期类型长度不能超过50个字符");
        RuleFor(x => x.ScoringStandard)
            .NotEmpty().WithMessage("评分标准不能为空")
            .MaximumLength(50).WithMessage("评分标准长度不能超过50个字符");
        RuleFor(x => x.MetricCode)
            .NotEmpty().WithMessage("指标编码不能为空")
            .MaximumLength(64).WithMessage("指标编码长度不能超过64个字符");
        RuleFor(x => x.MetricName)
            .NotEmpty().WithMessage("指标名称不能为空")
            .MaximumLength(128).WithMessage("指标名称长度不能超过128个字符");
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("指标类别不能为空")
            .MaximumLength(50).WithMessage("指标类别长度不能超过50个字符");
        RuleFor(x => x.MetricType)
            .NotEmpty().WithMessage("指标类型不能为空")
            .MaximumLength(50).WithMessage("指标类型长度不能超过50个字符");
        RuleFor(x => x.ScoringCriteria)
            .NotEmpty().WithMessage("评分标准说明不能为空")
            .MaximumLength(1000).WithMessage("评分标准说明长度不能超过1000个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SchemeMetric 验证器
// ========================================

/// <summary>
/// 更新SchemeMetric DTO 验证器
/// </summary>
public class TaktSchemeMetricUpdateValidator : AbstractValidator<TaktSchemeMetricUpdateDto>
{
    /// <summary>
    /// 初始化 更新SchemeMetric 校验规则
    /// </summary>
    public TaktSchemeMetricUpdateValidator()
    {
        RuleFor(x => x.SchemeMetricId)
            .GreaterThan(0).WithMessage("SchemeMetricID无效");
    }
}

// ========================================
// 导入SchemeMetric 验证器
// ========================================

/// <summary>
/// 导入SchemeMetric DTO 验证器
/// </summary>
public class TaktSchemeMetricImportValidator : AbstractValidator<TaktSchemeMetricImportDto>
{
    /// <summary>
    /// 初始化 导入SchemeMetric 校验规则
    /// </summary>
    public TaktSchemeMetricImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.SchemeCode)
            .NotEmpty().WithMessage("方案编码不能为空")
            .MaximumLength(64).WithMessage("方案编码长度不能超过64个字符");
        RuleFor(x => x.SchemeName)
            .NotEmpty().WithMessage("方案名称不能为空")
            .MaximumLength(128).WithMessage("方案名称长度不能超过128个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.CycleType)
            .NotEmpty().WithMessage("考核周期类型不能为空")
            .MaximumLength(50).WithMessage("考核周期类型长度不能超过50个字符");
        RuleFor(x => x.ScoringStandard)
            .NotEmpty().WithMessage("评分标准不能为空")
            .MaximumLength(50).WithMessage("评分标准长度不能超过50个字符");
        RuleFor(x => x.MetricCode)
            .NotEmpty().WithMessage("指标编码不能为空")
            .MaximumLength(64).WithMessage("指标编码长度不能超过64个字符");
        RuleFor(x => x.MetricName)
            .NotEmpty().WithMessage("指标名称不能为空")
            .MaximumLength(128).WithMessage("指标名称长度不能超过128个字符");
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("指标类别不能为空")
            .MaximumLength(50).WithMessage("指标类别长度不能超过50个字符");
        RuleFor(x => x.MetricType)
            .NotEmpty().WithMessage("指标类型不能为空")
            .MaximumLength(50).WithMessage("指标类型长度不能超过50个字符");
        RuleFor(x => x.ScoringCriteria)
            .NotEmpty().WithMessage("评分标准说明不能为空")
            .MaximumLength(1000).WithMessage("评分标准说明长度不能超过1000个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
