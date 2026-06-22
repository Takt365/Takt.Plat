// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaRepairDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPcbaRepairDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;

namespace Takt.Application.Validators.Logistics.Manufacturing.Defect;

// ========================================
// 创建PcbaRepairDetail 验证器
// ========================================

/// <summary>
/// 创建PcbaRepairDetail DTO 验证器
/// </summary>
public class TaktPcbaRepairDetailCreateValidator : AbstractValidator<TaktPcbaRepairDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建PcbaRepairDetail 校验规则
    /// </summary>
    public TaktPcbaRepairDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PcbaRepairId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA改修日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.PcbaBoardType)
            .MaximumLength(50).WithMessage("PCBA板别长度不能超过50个字符");
        RuleFor(x => x.ProdLine)
            .MaximumLength(50).WithMessage("生产线长度不能超过50个字符");
        RuleFor(x => x.CardNo)
            .MaximumLength(50).WithMessage("卡号长度不能超过50个字符");
        RuleFor(x => x.DefectSymptom)
            .MaximumLength(500).WithMessage("不良症状长度不能超过500个字符");
        RuleFor(x => x.DefectEngineering)
            .MaximumLength(50).WithMessage("检出工程长度不能超过50个字符");
        RuleFor(x => x.DefectReason)
            .MaximumLength(500).WithMessage("不良原因长度不能超过500个字符");
        RuleFor(x => x.DefectResponsibility)
            .MaximumLength(50).WithMessage("责任归属长度不能超过50个字符");
        RuleFor(x => x.DefectNature)
            .MaximumLength(50).WithMessage("不良性质长度不能超过50个字符");
        RuleFor(x => x.RepairOperator)
            .MaximumLength(20).WithMessage("修理员长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PcbaRepairDetail 验证器
// ========================================

/// <summary>
/// 更新PcbaRepairDetail DTO 验证器
/// </summary>
public class TaktPcbaRepairDetailUpdateValidator : AbstractValidator<TaktPcbaRepairDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新PcbaRepairDetail 校验规则
    /// </summary>
    public TaktPcbaRepairDetailUpdateValidator()
    {
        RuleFor(x => x.PcbaRepairDetailId)
            .GreaterThan(0).WithMessage("PcbaRepairDetailID无效");
    }
}

// ========================================
// 导入PcbaRepairDetail 验证器
// ========================================

/// <summary>
/// 导入PcbaRepairDetail DTO 验证器
/// </summary>
public class TaktPcbaRepairDetailImportValidator : AbstractValidator<TaktPcbaRepairDetailImportDto>
{
    /// <summary>
    /// 初始化 导入PcbaRepairDetail 校验规则
    /// </summary>
    public TaktPcbaRepairDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PcbaRepairId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA改修日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("生产工单号不能为空")
            .MaximumLength(20).WithMessage("生产工单号长度不能超过20个字符");
        RuleFor(x => x.PcbaBoardType)
            .MaximumLength(50).WithMessage("PCBA板别长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PcbaBoardType));
        RuleFor(x => x.ProdLine)
            .MaximumLength(50).WithMessage("生产线长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ProdLine));
        RuleFor(x => x.CardNo)
            .MaximumLength(50).WithMessage("卡号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.CardNo));
        RuleFor(x => x.DefectSymptom)
            .MaximumLength(500).WithMessage("不良症状长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.DefectSymptom));
        RuleFor(x => x.DefectEngineering)
            .MaximumLength(50).WithMessage("检出工程长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.DefectEngineering));
        RuleFor(x => x.DefectReason)
            .MaximumLength(500).WithMessage("不良原因长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.DefectReason));
        RuleFor(x => x.DefectResponsibility)
            .MaximumLength(50).WithMessage("责任归属长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.DefectResponsibility));
        RuleFor(x => x.DefectNature)
            .MaximumLength(50).WithMessage("不良性质长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.DefectNature));
        RuleFor(x => x.RepairOperator)
            .MaximumLength(20).WithMessage("修理员长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.RepairOperator));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
