// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlanValidators.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPlan 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Planning;

namespace Takt.Application.Validators.Logistics.Manufacturing.Planning;

// ========================================
// 创建SalesPlan 验证器
// ========================================

/// <summary>
/// 创建SalesPlan DTO 验证器
/// </summary>
public class TaktSalesPlanCreateValidator : AbstractValidator<TaktSalesPlanCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesPlan 校验规则
    /// </summary>
    public TaktSalesPlanCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.SalesPlanCode)
            .NotEmpty().WithMessage("销售计划编码不能为空")
            .MaximumLength(10).WithMessage("销售计划编码长度不能超过10个字符");
        RuleFor(x => x.PlannerId)
            .GreaterThanOrEqualTo(0).WithMessage("计划人员工ID不能为负数");
        RuleFor(x => x.PlanBy)
            .NotEmpty().WithMessage("计划人不能为空")
            .MaximumLength(50).WithMessage("计划人长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesPlan 验证器
// ========================================

/// <summary>
/// 更新SalesPlan DTO 验证器
/// </summary>
public class TaktSalesPlanUpdateValidator : AbstractValidator<TaktSalesPlanUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesPlan 校验规则
    /// </summary>
    public TaktSalesPlanUpdateValidator()
    {
        RuleFor(x => x.SalesPlanId)
            .GreaterThan(0).WithMessage("SalesPlanID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.SalesPlanCode)
            .NotEmpty().WithMessage("销售计划编码不能为空")
            .MaximumLength(10).WithMessage("销售计划编码长度不能超过10个字符");
        RuleFor(x => x.PlannerId)
            .GreaterThanOrEqualTo(0).WithMessage("计划人员工ID不能为负数");
        RuleFor(x => x.PlanBy)
            .NotEmpty().WithMessage("计划人不能为空")
            .MaximumLength(50).WithMessage("计划人长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesPlan 验证器
// ========================================

/// <summary>
/// 导入SalesPlan DTO 验证器
/// </summary>
public class TaktSalesPlanImportValidator : AbstractValidator<TaktSalesPlanImportDto>
{
    /// <summary>
    /// 初始化 导入SalesPlan 校验规则
    /// </summary>
    public TaktSalesPlanImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.SalesPlanCode)
            .NotEmpty().WithMessage("销售计划编码不能为空")
            .MaximumLength(10).WithMessage("销售计划编码长度不能超过10个字符");
        RuleFor(x => x.PlannerId)
            .GreaterThanOrEqualTo(0).WithMessage("计划人员工ID不能为负数");
        RuleFor(x => x.PlanBy)
            .NotEmpty().WithMessage("计划人不能为空")
            .MaximumLength(50).WithMessage("计划人长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
