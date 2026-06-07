// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Ec 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建Ec 验证器
// ========================================

/// <summary>
/// 创建Ec DTO 验证器
/// </summary>
public class TaktEcCreateValidator : AbstractValidator<TaktEcCreateDto>
{
    /// <summary>
    /// 初始化 创建Ec 校验规则
    /// </summary>
    public TaktEcCreateValidator()
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
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcTitle)
            .NotEmpty().WithMessage("设变主题/标题不能为空")
            .MaximumLength(500).WithMessage("设变主题/标题长度不能超过500个字符");
        RuleFor(x => x.EcDetailText)
            .NotEmpty().WithMessage("设变详情/详细说明不能为空");
        RuleFor(x => x.EcLeader)
            .NotEmpty().WithMessage("负责人不能为空")
            .MaximumLength(50).WithMessage("负责人长度不能超过50个字符");
        RuleFor(x => x.EcDistinction)
            .NotEmpty().WithMessage("区分/类别 1:全仕向，2：部管，3：内部，4：技术不能为空")
            .MaximumLength(50).WithMessage("区分/类别 1:全仕向，2：部管，3：内部，4：技术长度不能超过50个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例ID不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Ec 验证器
// ========================================

/// <summary>
/// 更新Ec DTO 验证器
/// </summary>
public class TaktEcUpdateValidator : AbstractValidator<TaktEcUpdateDto>
{
    /// <summary>
    /// 初始化 更新Ec 校验规则
    /// </summary>
    public TaktEcUpdateValidator()
    {
        RuleFor(x => x.EcId)
            .GreaterThan(0).WithMessage("EcID无效");
    }
}

// ========================================
// 导入Ec 验证器
// ========================================

/// <summary>
/// 导入Ec DTO 验证器
/// </summary>
public class TaktEcImportValidator : AbstractValidator<TaktEcImportDto>
{
    /// <summary>
    /// 初始化 导入Ec 校验规则
    /// </summary>
    public TaktEcImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcTitle)
            .NotEmpty().WithMessage("设变主题/标题不能为空")
            .MaximumLength(500).WithMessage("设变主题/标题长度不能超过500个字符");
        RuleFor(x => x.EcDetailText)
            .NotEmpty().WithMessage("设变详情/详细说明不能为空");
        RuleFor(x => x.EcLeader)
            .NotEmpty().WithMessage("负责人不能为空")
            .MaximumLength(50).WithMessage("负责人长度不能超过50个字符");
        RuleFor(x => x.EcDistinction)
            .NotEmpty().WithMessage("区分/类别 1:全仕向，2：部管，3：内部，4：技术不能为空")
            .MaximumLength(50).WithMessage("区分/类别 1:全仕向，2：部管，3：内部，4：技术长度不能超过50个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例ID不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
