// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktIpqcDefectHandlingValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：IpqcDefectHandling 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIpqcDefectHandling 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建IpqcDefectHandling 验证器
// ========================================

/// <summary>
/// 创建IpqcDefectHandling DTO 验证器
/// </summary>
public class TaktIpqcDefectHandlingCreateValidator : AbstractValidator<TaktIpqcDefectHandlingCreateDto>
{
    /// <summary>
    /// 初始化 创建IpqcDefectHandling 校验规则
    /// </summary>
    public TaktIpqcDefectHandlingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.IpqcDefectHandlingCode)
            .NotEmpty().WithMessage("IPQC不良处理编码不能为空")
            .MaximumLength(50).WithMessage("IPQC不良处理编码长度不能超过50个字符");
        RuleFor(x => x.IpqcOrderItemId)
            .GreaterThanOrEqualTo(0).WithMessage("IPQC检验单明细ID不能为负数");
        RuleFor(x => x.IpqcOrderCode)
            .NotEmpty().WithMessage("IPQC检验单编码不能为空")
            .MaximumLength(50).WithMessage("IPQC检验单编码长度不能超过50个字符");
        RuleFor(x => x.DefectCode)
            .NotEmpty().WithMessage("不良现象编码不能为空")
            .MaximumLength(50).WithMessage("不良现象编码长度不能超过50个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(500).WithMessage("不良现象描述长度不能超过500个字符");
        RuleFor(x => x.HandlingDescription)
            .MaximumLength(1000).WithMessage("处理说明长度不能超过1000个字符");
        RuleFor(x => x.ResponsibleDept)
            .MaximumLength(100).WithMessage("责任部门长度不能超过100个字符");
        RuleFor(x => x.ResponsibleBy)
            .MaximumLength(50).WithMessage("责任人长度不能超过50个字符");
        RuleFor(x => x.HandlerBy)
            .MaximumLength(50).WithMessage("处理人长度不能超过50个字符");
        RuleFor(x => x.CorrectiveAction)
            .MaximumLength(1000).WithMessage("预防措施/纠正措施长度不能超过1000个字符");
        RuleFor(x => x.DefectImages)
            .MaximumLength(2000).WithMessage("不良图片长度不能超过2000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新IpqcDefectHandling 验证器
// ========================================

/// <summary>
/// 更新IpqcDefectHandling DTO 验证器
/// </summary>
public class TaktIpqcDefectHandlingUpdateValidator : AbstractValidator<TaktIpqcDefectHandlingUpdateDto>
{
    /// <summary>
    /// 初始化 更新IpqcDefectHandling 校验规则
    /// </summary>
    public TaktIpqcDefectHandlingUpdateValidator()
    {
        RuleFor(x => x.IpqcDefectHandlingId)
            .GreaterThan(0).WithMessage("IpqcDefectHandlingID无效");
    }
}

// ========================================
// 导入IpqcDefectHandling 验证器
// ========================================

/// <summary>
/// 导入IpqcDefectHandling DTO 验证器
/// </summary>
public class TaktIpqcDefectHandlingImportValidator : AbstractValidator<TaktIpqcDefectHandlingImportDto>
{
    /// <summary>
    /// 初始化 导入IpqcDefectHandling 校验规则
    /// </summary>
    public TaktIpqcDefectHandlingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.IpqcDefectHandlingCode)
            .NotEmpty().WithMessage("IPQC不良处理编码不能为空")
            .MaximumLength(50).WithMessage("IPQC不良处理编码长度不能超过50个字符");
        RuleFor(x => x.IpqcOrderItemId)
            .GreaterThanOrEqualTo(0).WithMessage("IPQC检验单明细ID不能为负数");
        RuleFor(x => x.IpqcOrderCode)
            .NotEmpty().WithMessage("IPQC检验单编码不能为空")
            .MaximumLength(50).WithMessage("IPQC检验单编码长度不能超过50个字符");
        RuleFor(x => x.DefectCode)
            .NotEmpty().WithMessage("不良现象编码不能为空")
            .MaximumLength(50).WithMessage("不良现象编码长度不能超过50个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(500).WithMessage("不良现象描述长度不能超过500个字符");
        RuleFor(x => x.HandlingDescription)
            .MaximumLength(1000).WithMessage("处理说明长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.HandlingDescription));
        RuleFor(x => x.ResponsibleDept)
            .MaximumLength(100).WithMessage("责任部门长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.ResponsibleDept));
        RuleFor(x => x.ResponsibleBy)
            .MaximumLength(50).WithMessage("责任人长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ResponsibleBy));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
