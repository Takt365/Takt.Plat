// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.NewsCenter
// 文件名称：TaktNewsAttachmentValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：NewsAttachment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktNewsAttachment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.NewsCenter;

namespace Takt.Application.Validators.Routine.NewsCenter;

// ========================================
// 创建NewsAttachment 验证器
// ========================================

/// <summary>
/// 创建NewsAttachment DTO 验证器
/// </summary>
public class TaktNewsAttachmentCreateValidator : AbstractValidator<TaktNewsAttachmentCreateDto>
{
    /// <summary>
    /// 初始化 创建NewsAttachment 校验规则
    /// </summary>
    public TaktNewsAttachmentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.NewsId)
            .GreaterThanOrEqualTo(0).WithMessage("新闻 ID不能为负数");
        RuleFor(x => x.FileId)
            .GreaterThanOrEqualTo(0).WithMessage("文件 ID不能为负数");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新NewsAttachment 验证器
// ========================================

/// <summary>
/// 更新NewsAttachment DTO 验证器
/// </summary>
public class TaktNewsAttachmentUpdateValidator : AbstractValidator<TaktNewsAttachmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新NewsAttachment 校验规则
    /// </summary>
    public TaktNewsAttachmentUpdateValidator()
    {
        RuleFor(x => x.NewsAttachmentId)
            .GreaterThan(0).WithMessage("NewsAttachmentID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.NewsId)
            .GreaterThanOrEqualTo(0).WithMessage("新闻 ID不能为负数");
        RuleFor(x => x.FileId)
            .GreaterThanOrEqualTo(0).WithMessage("文件 ID不能为负数");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入NewsAttachment 验证器
// ========================================

/// <summary>
/// 导入NewsAttachment DTO 验证器
/// </summary>
public class TaktNewsAttachmentImportValidator : AbstractValidator<TaktNewsAttachmentImportDto>
{
    /// <summary>
    /// 初始化 导入NewsAttachment 校验规则
    /// </summary>
    public TaktNewsAttachmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.NewsId)
            .GreaterThanOrEqualTo(0).WithMessage("新闻 ID不能为负数");
        RuleFor(x => x.FileId)
            .GreaterThanOrEqualTo(0).WithMessage("文件 ID不能为负数");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
