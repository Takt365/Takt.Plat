// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOutput 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAssyOutput 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建AssyOutput 验证器
// ========================================

/// <summary>
/// 创建AssyOutput DTO 验证器
/// </summary>
public class TaktAssyOutputCreateValidator : AbstractValidator<TaktAssyOutputCreateDto>
{
    /// <summary>
    /// 初始化 创建AssyOutput 校验规则
    /// </summary>
    public TaktAssyOutputCreateValidator()
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
        RuleFor(x => x.ProdCategory)
            .NotEmpty().WithMessage("生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP:不能为空")
            .MaximumLength(20).WithMessage("生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP:长度不能超过20个字符");
        RuleFor(x => x.ProdLine)
            .NotEmpty().WithMessage("生产线不能为空")
            .MaximumLength(20).WithMessage("生产线长度不能超过20个字符");
        RuleFor(x => x.ProdOrderType)
            .MaximumLength(20).WithMessage("生产订单类型长度不能超过20个字符");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(20).WithMessage("机种长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(20).WithMessage("批次长度不能超过20个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新AssyOutput 验证器
// ========================================

/// <summary>
/// 更新AssyOutput DTO 验证器
/// </summary>
public class TaktAssyOutputUpdateValidator : AbstractValidator<TaktAssyOutputUpdateDto>
{
    /// <summary>
    /// 初始化 更新AssyOutput 校验规则
    /// </summary>
    public TaktAssyOutputUpdateValidator()
    {
        RuleFor(x => x.AssyOutputId)
            .GreaterThan(0).WithMessage("AssyOutputID无效");
    }
}

// ========================================
// 导入AssyOutput 验证器
// ========================================

/// <summary>
/// 导入AssyOutput DTO 验证器
/// </summary>
public class TaktAssyOutputImportValidator : AbstractValidator<TaktAssyOutputImportDto>
{
    /// <summary>
    /// 初始化 导入AssyOutput 校验规则
    /// </summary>
    public TaktAssyOutputImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProdCategory)
            .NotEmpty().WithMessage("生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP:不能为空")
            .MaximumLength(20).WithMessage("生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP:长度不能超过20个字符");
        RuleFor(x => x.ProdLine)
            .NotEmpty().WithMessage("生产线不能为空")
            .MaximumLength(20).WithMessage("生产线长度不能超过20个字符");
        RuleFor(x => x.ProdOrderType)
            .MaximumLength(20).WithMessage("生产订单类型长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.ProdOrderType));
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(20).WithMessage("机种长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(20).WithMessage("批次长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.BatchNo));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
