// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaOutputDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPcbaOutputDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建PcbaOutputDetail 验证器
// ========================================

/// <summary>
/// 创建PcbaOutputDetail DTO 验证器
/// </summary>
public class TaktPcbaOutputDetailCreateValidator : AbstractValidator<TaktPcbaOutputDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建PcbaOutputDetail 校验规则
    /// </summary>
    public TaktPcbaOutputDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PcbaOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.PcbBoardType)
            .NotEmpty().WithMessage("板别不能为空")
            .MaximumLength(20).WithMessage("板别长度不能超过20个字符");
        RuleFor(x => x.PanelSide)
            .NotEmpty().WithMessage("面板别不能为空")
            .MaximumLength(10).WithMessage("面板别长度不能超过10个字符");
        RuleFor(x => x.SerialNo)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(20).WithMessage("序列号长度不能超过20个字符");
        RuleFor(x => x.UnachievedReason)
            .MaximumLength(500).WithMessage("未达成原因长度不能超过500个字符");
        RuleFor(x => x.UnachievedDescription)
            .MaximumLength(500).WithMessage("未达成说明长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PcbaOutputDetail 验证器
// ========================================

/// <summary>
/// 更新PcbaOutputDetail DTO 验证器
/// </summary>
public class TaktPcbaOutputDetailUpdateValidator : AbstractValidator<TaktPcbaOutputDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新PcbaOutputDetail 校验规则
    /// </summary>
    public TaktPcbaOutputDetailUpdateValidator()
    {
        RuleFor(x => x.PcbaOutputDetailId)
            .GreaterThan(0).WithMessage("PcbaOutputDetailID无效");
    }
}

// ========================================
// 导入PcbaOutputDetail 验证器
// ========================================

/// <summary>
/// 导入PcbaOutputDetail DTO 验证器
/// </summary>
public class TaktPcbaOutputDetailImportValidator : AbstractValidator<TaktPcbaOutputDetailImportDto>
{
    /// <summary>
    /// 初始化 导入PcbaOutputDetail 校验规则
    /// </summary>
    public TaktPcbaOutputDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PcbaOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.PcbBoardType)
            .NotEmpty().WithMessage("板别不能为空")
            .MaximumLength(20).WithMessage("板别长度不能超过20个字符");
        RuleFor(x => x.PanelSide)
            .NotEmpty().WithMessage("面板别不能为空")
            .MaximumLength(10).WithMessage("面板别长度不能超过10个字符");
        RuleFor(x => x.SerialNo)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(20).WithMessage("序列号长度不能超过20个字符");
        RuleFor(x => x.UnachievedReason)
            .MaximumLength(500).WithMessage("未达成原因长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.UnachievedReason));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
