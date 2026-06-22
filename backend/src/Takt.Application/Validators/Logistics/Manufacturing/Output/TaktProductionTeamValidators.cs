// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTeamValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionTeam 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductionTeam 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建ProductionTeam 验证器
// ========================================

/// <summary>
/// 创建ProductionTeam DTO 验证器
/// </summary>
public class TaktProductionTeamCreateValidator : AbstractValidator<TaktProductionTeamCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductionTeam 校验规则
    /// </summary>
    public TaktProductionTeamCreateValidator()
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
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("班组编码不能为空")
            .MaximumLength(32).WithMessage("班组编码长度不能超过32个字符");
        RuleFor(x => x.TeamName)
            .NotEmpty().WithMessage("班组名称不能为空")
            .MaximumLength(64).WithMessage("班组名称长度不能超过64个字符");
        RuleFor(x => x.TeamCategory)
            .MaximumLength(10).WithMessage("班组分类编码长度不能超过10个字符");
        RuleFor(x => x.TeamCategoryName)
            .MaximumLength(50).WithMessage("班组分类名称长度不能超过50个字符");
        RuleFor(x => x.ProductionLine)
            .MaximumLength(20).WithMessage("生产线代码长度不能超过20个字符");
        RuleFor(x => x.TeamLeaderId)
            .GreaterThanOrEqualTo(0).WithMessage("班组长员工Id不能为负数");
        RuleFor(x => x.TeamLeaderName)
            .MaximumLength(50).WithMessage("班组长姓名长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductionTeam 验证器
// ========================================

/// <summary>
/// 更新ProductionTeam DTO 验证器
/// </summary>
public class TaktProductionTeamUpdateValidator : AbstractValidator<TaktProductionTeamUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductionTeam 校验规则
    /// </summary>
    public TaktProductionTeamUpdateValidator()
    {
        RuleFor(x => x.ProductionTeamId)
            .GreaterThan(0).WithMessage("ProductionTeamID无效");
    }
}

// ========================================
// 导入ProductionTeam 验证器
// ========================================

/// <summary>
/// 导入ProductionTeam DTO 验证器
/// </summary>
public class TaktProductionTeamImportValidator : AbstractValidator<TaktProductionTeamImportDto>
{
    /// <summary>
    /// 初始化 导入ProductionTeam 校验规则
    /// </summary>
    public TaktProductionTeamImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("班组编码不能为空")
            .MaximumLength(32).WithMessage("班组编码长度不能超过32个字符");
        RuleFor(x => x.TeamName)
            .NotEmpty().WithMessage("班组名称不能为空")
            .MaximumLength(64).WithMessage("班组名称长度不能超过64个字符");
        RuleFor(x => x.TeamCategory)
            .MaximumLength(10).WithMessage("班组分类编码长度不能超过10个字符").When(x => !string.IsNullOrWhiteSpace(x.TeamCategory));
        RuleFor(x => x.TeamCategoryName)
            .MaximumLength(50).WithMessage("班组分类名称长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TeamCategoryName));
        RuleFor(x => x.ProductionLine)
            .MaximumLength(20).WithMessage("生产线代码长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionLine));
        RuleFor(x => x.TeamLeaderId)
            .GreaterThanOrEqualTo(0).WithMessage("班组长员工Id不能为负数");
        RuleFor(x => x.TeamLeaderName)
            .MaximumLength(50).WithMessage("班组长姓名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TeamLeaderName));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
