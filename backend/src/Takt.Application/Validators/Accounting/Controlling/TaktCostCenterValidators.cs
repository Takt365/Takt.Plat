// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Accounting.Controlling
// 文件名称：TaktCostCenterValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：CostCenter 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCostCenter 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Controlling;

namespace Takt.Application.Validators.Accounting.Controlling;

// ========================================
// 创建CostCenter 验证器
// ========================================

/// <summary>
/// 创建CostCenter DTO 验证器
/// </summary>
public class TaktCostCenterCreateValidator : AbstractValidator<TaktCostCenterCreateDto>
{
    /// <summary>
    /// 初始化 创建CostCenter 校验规则
    /// </summary>
    public TaktCostCenterCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CostCenterCode)
            .NotEmpty().WithMessage("成本中心编码不能为空")
            .MaximumLength(50).WithMessage("成本中心编码长度不能超过50个字符");
        RuleFor(x => x.CostCenterName)
            .NotEmpty().WithMessage("成本中心名称不能为空")
            .MaximumLength(100).WithMessage("成本中心名称长度不能超过100个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级 ID不能为负数");
        RuleFor(x => x.ManagerId)
            .GreaterThanOrEqualTo(0).WithMessage("负责人用户 ID不能为负数");
        RuleFor(x => x.ManagerName)
            .MaximumLength(50).WithMessage("负责人姓名长度不能超过50个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("所属部门 ID不能为负数");
        RuleFor(x => x.DeptName)
            .MaximumLength(100).WithMessage("所属部门名称长度不能超过100个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CostCenter 验证器
// ========================================

/// <summary>
/// 更新CostCenter DTO 验证器
/// </summary>
public class TaktCostCenterUpdateValidator : AbstractValidator<TaktCostCenterUpdateDto>
{
    /// <summary>
    /// 初始化 更新CostCenter 校验规则
    /// </summary>
    public TaktCostCenterUpdateValidator()
    {
        RuleFor(x => x.CostCenterId)
            .GreaterThan(0).WithMessage("CostCenterID无效");
    }
}

// ========================================
// 导入CostCenter 验证器
// ========================================

/// <summary>
/// 导入CostCenter DTO 验证器
/// </summary>
public class TaktCostCenterImportValidator : AbstractValidator<TaktCostCenterImportDto>
{
    /// <summary>
    /// 初始化 导入CostCenter 校验规则
    /// </summary>
    public TaktCostCenterImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CostCenterCode)
            .NotEmpty().WithMessage("成本中心编码不能为空")
            .MaximumLength(50).WithMessage("成本中心编码长度不能超过50个字符");
        RuleFor(x => x.CostCenterName)
            .NotEmpty().WithMessage("成本中心名称不能为空")
            .MaximumLength(100).WithMessage("成本中心名称长度不能超过100个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级 ID不能为负数");
        RuleFor(x => x.ManagerId)
            .GreaterThanOrEqualTo(0).WithMessage("负责人用户 ID不能为负数");
        RuleFor(x => x.ManagerName)
            .MaximumLength(50).WithMessage("负责人姓名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ManagerName));
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("所属部门 ID不能为负数");
        RuleFor(x => x.DeptName)
            .MaximumLength(100).WithMessage("所属部门名称长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.DeptName));
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
