// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：EcAttachment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcAttachment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcAttachment 验证器
// ========================================

/// <summary>
/// 创建EcAttachment DTO 验证器
/// </summary>
public class TaktEcAttachmentCreateValidator : AbstractValidator<TaktEcAttachmentCreateDto>
{
    /// <summary>
    /// 初始化 创建EcAttachment 校验规则
    /// </summary>
    public TaktEcAttachmentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.AttachmentType)
            .NotEmpty().WithMessage("文件类别：Liaison=联络, EPP, FPP, ExternalLiais不能为空")
            .MaximumLength(30).WithMessage("文件类别：Liaison=联络, EPP, FPP, ExternalLiais长度不能超过30个字符");
        RuleFor(x => x.DocNo)
            .NotEmpty().WithMessage("文件编号不能为空")
            .MaximumLength(50).WithMessage("文件编号长度不能超过50个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.AccessUrl)
            .NotEmpty().WithMessage("访问地址不能为空")
            .MaximumLength(500).WithMessage("访问地址长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcAttachment 验证器
// ========================================

/// <summary>
/// 更新EcAttachment DTO 验证器
/// </summary>
public class TaktEcAttachmentUpdateValidator : AbstractValidator<TaktEcAttachmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcAttachment 校验规则
    /// </summary>
    public TaktEcAttachmentUpdateValidator()
    {
        RuleFor(x => x.EcAttachmentId)
            .GreaterThan(0).WithMessage("EcAttachmentID无效");
    }
}

// ========================================
// 导入EcAttachment 验证器
// ========================================

/// <summary>
/// 导入EcAttachment DTO 验证器
/// </summary>
public class TaktEcAttachmentImportValidator : AbstractValidator<TaktEcAttachmentImportDto>
{
    /// <summary>
    /// 初始化 导入EcAttachment 校验规则
    /// </summary>
    public TaktEcAttachmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EcId)
            .GreaterThanOrEqualTo(0).WithMessage("设变主表ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.AttachmentType)
            .NotEmpty().WithMessage("文件类别：Liaison=联络, EPP, FPP, ExternalLiais不能为空")
            .MaximumLength(30).WithMessage("文件类别：Liaison=联络, EPP, FPP, ExternalLiais长度不能超过30个字符");
        RuleFor(x => x.DocNo)
            .NotEmpty().WithMessage("文件编号不能为空")
            .MaximumLength(50).WithMessage("文件编号长度不能超过50个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.AccessUrl)
            .NotEmpty().WithMessage("访问地址不能为空")
            .MaximumLength(500).WithMessage("访问地址长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
