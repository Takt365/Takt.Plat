// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductionOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建ProductionOrder 验证器
// ========================================

/// <summary>
/// 创建ProductionOrder DTO 验证器
/// </summary>
public class TaktProductionOrderCreateValidator : AbstractValidator<TaktProductionOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductionOrder 校验规则
    /// </summary>
    public TaktProductionOrderCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.ProdOrderType)
            .NotEmpty().WithMessage("生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造不能为空")
            .MaximumLength(10).WithMessage("生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造长度不能超过10个字符");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(40).WithMessage("生产工单号长度不能超过40个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(40).WithMessage("物料编码长度不能超过40个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(10).WithMessage("计量单位长度不能超过10个字符");
        RuleFor(x => x.WorkCenter)
            .MaximumLength(20).WithMessage("工作中心长度不能超过20个字符");
        RuleFor(x => x.ProdLine)
            .MaximumLength(20).WithMessage("生产线长度不能超过20个字符");
        RuleFor(x => x.ProdBatch)
            .MaximumLength(20).WithMessage("生产批次长度不能超过20个字符");
        RuleFor(x => x.SerialNo)
            .MaximumLength(20).WithMessage("序列号长度不能超过20个字符");
        RuleFor(x => x.RoutingCode)
            .MaximumLength(40).WithMessage("工艺路线编码长度不能超过40个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductionOrder 验证器
// ========================================

/// <summary>
/// 更新ProductionOrder DTO 验证器
/// </summary>
public class TaktProductionOrderUpdateValidator : AbstractValidator<TaktProductionOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductionOrder 校验规则
    /// </summary>
    public TaktProductionOrderUpdateValidator()
    {
        RuleFor(x => x.ProductionOrderId)
            .GreaterThan(0).WithMessage("ProductionOrderID无效");
    }
}

// ========================================
// 导入ProductionOrder 验证器
// ========================================

/// <summary>
/// 导入ProductionOrder DTO 验证器
/// </summary>
public class TaktProductionOrderImportValidator : AbstractValidator<TaktProductionOrderImportDto>
{
    /// <summary>
    /// 初始化 导入ProductionOrder 校验规则
    /// </summary>
    public TaktProductionOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.ProdOrderType)
            .NotEmpty().WithMessage("生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造不能为空")
            .MaximumLength(10).WithMessage("生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造长度不能超过10个字符");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(40).WithMessage("生产工单号长度不能超过40个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(40).WithMessage("物料编码长度不能超过40个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(10).WithMessage("计量单位长度不能超过10个字符");
        RuleFor(x => x.WorkCenter)
            .MaximumLength(20).WithMessage("工作中心长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkCenter));
        RuleFor(x => x.ProdLine)
            .MaximumLength(20).WithMessage("生产线长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ProdLine));
        RuleFor(x => x.ProdBatch)
            .MaximumLength(20).WithMessage("生产批次长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ProdBatch));
        RuleFor(x => x.SerialNo)
            .MaximumLength(20).WithMessage("序列号长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.SerialNo));
        RuleFor(x => x.RoutingCode)
            .MaximumLength(40).WithMessage("工艺路线编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.RoutingCode));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
