// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlanItemValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPlanItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesPlanItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Planning;

namespace Takt.Application.Validators.Logistics.Manufacturing.Planning;

// ========================================
// 创建SalesPlanItem 验证器
// ========================================

/// <summary>
/// 创建SalesPlanItem DTO 验证器
/// </summary>
public class TaktSalesPlanItemCreateValidator : AbstractValidator<TaktSalesPlanItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesPlanItem 校验规则
    /// </summary>
    public TaktSalesPlanItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SalesPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("销售计划ID不能为负数");
        RuleFor(x => x.SalesPlanCode)
            .NotEmpty().WithMessage("销售计划编码不能为空")
            .MaximumLength(10).WithMessage("销售计划编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.PlanUnit)
            .NotEmpty().WithMessage("计划单位不能为空")
            .MaximumLength(20).WithMessage("计划单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesPlanItem 验证器
// ========================================

/// <summary>
/// 更新SalesPlanItem DTO 验证器
/// </summary>
public class TaktSalesPlanItemUpdateValidator : AbstractValidator<TaktSalesPlanItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesPlanItem 校验规则
    /// </summary>
    public TaktSalesPlanItemUpdateValidator()
    {
        RuleFor(x => x.SalesPlanItemId)
            .GreaterThan(0).WithMessage("SalesPlanItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SalesPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("销售计划ID不能为负数");
        RuleFor(x => x.SalesPlanCode)
            .NotEmpty().WithMessage("销售计划编码不能为空")
            .MaximumLength(10).WithMessage("销售计划编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.PlanUnit)
            .NotEmpty().WithMessage("计划单位不能为空")
            .MaximumLength(20).WithMessage("计划单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesPlanItem 验证器
// ========================================

/// <summary>
/// 导入SalesPlanItem DTO 验证器
/// </summary>
public class TaktSalesPlanItemImportValidator : AbstractValidator<TaktSalesPlanItemImportDto>
{
    /// <summary>
    /// 初始化 导入SalesPlanItem 校验规则
    /// </summary>
    public TaktSalesPlanItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.SalesPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("销售计划ID不能为负数");
        RuleFor(x => x.SalesPlanCode)
            .NotEmpty().WithMessage("销售计划编码不能为空")
            .MaximumLength(10).WithMessage("销售计划编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.PlanUnit)
            .NotEmpty().WithMessage("计划单位不能为空")
            .MaximumLength(20).WithMessage("计划单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
