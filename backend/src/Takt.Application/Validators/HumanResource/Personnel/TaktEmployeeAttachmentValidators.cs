// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeAttachment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeAttachment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建EmployeeAttachment 验证器
// ========================================

/// <summary>
/// 创建EmployeeAttachment DTO 验证器
/// </summary>
public class TaktEmployeeAttachmentCreateValidator : AbstractValidator<TaktEmployeeAttachmentCreateDto>
{
    /// <summary>
    /// 初始化 创建EmployeeAttachment 校验规则
    /// </summary>
    public TaktEmployeeAttachmentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工ID不能为负数");
        RuleFor(x => x.FileId)
            .GreaterThanOrEqualTo(0).WithMessage("文件ID不能为负数");
        RuleFor(x => x.FileCode)
            .MaximumLength(50).WithMessage("文件编码长度不能超过50个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(255).WithMessage("文件名称长度不能超过255个字符");
        RuleFor(x => x.FilePath)
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.FileType)
            .MaximumLength(100).WithMessage("文件类型/MIME长度不能超过100个字符");
        RuleFor(x => x.AttachmentDescription)
            .MaximumLength(500).WithMessage("附件说明长度不能超过500个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EmployeeAttachment 验证器
// ========================================

/// <summary>
/// 更新EmployeeAttachment DTO 验证器
/// </summary>
public class TaktEmployeeAttachmentUpdateValidator : AbstractValidator<TaktEmployeeAttachmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新EmployeeAttachment 校验规则
    /// </summary>
    public TaktEmployeeAttachmentUpdateValidator()
    {
        RuleFor(x => x.EmployeeAttachmentId)
            .GreaterThan(0).WithMessage("EmployeeAttachmentID无效");
    }
}

// ========================================
// 导入EmployeeAttachment 验证器
// ========================================

/// <summary>
/// 导入EmployeeAttachment DTO 验证器
/// </summary>
public class TaktEmployeeAttachmentImportValidator : AbstractValidator<TaktEmployeeAttachmentImportDto>
{
    /// <summary>
    /// 初始化 导入EmployeeAttachment 校验规则
    /// </summary>
    public TaktEmployeeAttachmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工ID不能为负数");
        RuleFor(x => x.FileId)
            .GreaterThanOrEqualTo(0).WithMessage("文件ID不能为负数");
        RuleFor(x => x.FileCode)
            .MaximumLength(50).WithMessage("文件编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.FileCode));
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(255).WithMessage("文件名称长度不能超过255个字符");
        RuleFor(x => x.FilePath)
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.FilePath));
        RuleFor(x => x.FileType)
            .MaximumLength(100).WithMessage("文件类型/MIME长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.FileType));
        RuleFor(x => x.AttachmentDescription)
            .MaximumLength(500).WithMessage("附件说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.AttachmentDescription));
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
