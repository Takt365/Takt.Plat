// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaInspectionDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPcbaInspectionDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;

namespace Takt.Application.Validators.Logistics.Manufacturing.Defect;

// ========================================
// 创建PcbaInspectionDetail 验证器
// ========================================

/// <summary>
/// 创建PcbaInspectionDetail DTO 验证器
/// </summary>
public class TaktPcbaInspectionDetailCreateValidator : AbstractValidator<TaktPcbaInspectionDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建PcbaInspectionDetail 校验规则
    /// </summary>
    public TaktPcbaInspectionDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PcbaInspectionId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA检查日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.PcbaBoardType)
            .MaximumLength(50).WithMessage("PCBA板别长度不能超过50个字符");
        RuleFor(x => x.VisualInspectionLine)
            .MaximumLength(50).WithMessage("目视线别长度不能超过50个字符");
        RuleFor(x => x.AoiLine)
            .MaximumLength(50).WithMessage("AOI线别长度不能超过50个字符");
        RuleFor(x => x.InspectorName)
            .MaximumLength(50).WithMessage("检查员长度不能超过50个字符");
        RuleFor(x => x.ProdLine)
            .MaximumLength(50).WithMessage("生产线长度不能超过50个字符");
        RuleFor(x => x.HandPlacement)
            .MaximumLength(100).WithMessage("手贴长度不能超过100个字符");
        RuleFor(x => x.SerialNumber)
            .MaximumLength(50).WithMessage("流水号长度不能超过50个字符");
        RuleFor(x => x.Content)
            .MaximumLength(500).WithMessage("内容长度不能超过500个字符");
        RuleFor(x => x.DefectLocation)
            .MaximumLength(200).WithMessage("不良个所长度不能超过200个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PcbaInspectionDetail 验证器
// ========================================

/// <summary>
/// 更新PcbaInspectionDetail DTO 验证器
/// </summary>
public class TaktPcbaInspectionDetailUpdateValidator : AbstractValidator<TaktPcbaInspectionDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新PcbaInspectionDetail 校验规则
    /// </summary>
    public TaktPcbaInspectionDetailUpdateValidator()
    {
        RuleFor(x => x.PcbaInspectionDetailId)
            .GreaterThan(0).WithMessage("PcbaInspectionDetailID无效");
    }
}

// ========================================
// 导入PcbaInspectionDetail 验证器
// ========================================

/// <summary>
/// 导入PcbaInspectionDetail DTO 验证器
/// </summary>
public class TaktPcbaInspectionDetailImportValidator : AbstractValidator<TaktPcbaInspectionDetailImportDto>
{
    /// <summary>
    /// 初始化 导入PcbaInspectionDetail 校验规则
    /// </summary>
    public TaktPcbaInspectionDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PcbaInspectionId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA检查日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.PcbaBoardType)
            .MaximumLength(50).WithMessage("PCBA板别长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PcbaBoardType));
        RuleFor(x => x.VisualInspectionLine)
            .MaximumLength(50).WithMessage("目视线别长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.VisualInspectionLine));
        RuleFor(x => x.AoiLine)
            .MaximumLength(50).WithMessage("AOI线别长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.AoiLine));
        RuleFor(x => x.InspectorName)
            .MaximumLength(50).WithMessage("检查员长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.InspectorName));
        RuleFor(x => x.ProdLine)
            .MaximumLength(50).WithMessage("生产线长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ProdLine));
        RuleFor(x => x.HandPlacement)
            .MaximumLength(100).WithMessage("手贴长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.HandPlacement));
        RuleFor(x => x.SerialNumber)
            .MaximumLength(50).WithMessage("流水号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
