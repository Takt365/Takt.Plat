// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktSelfServiceValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SelfService 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSelfService 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

// ========================================
// 创建SelfService 验证器
// ========================================

/// <summary>
/// 创建SelfService DTO 验证器
/// </summary>
public class TaktSelfServiceCreateValidator : AbstractValidator<TaktSelfServiceCreateDto>
{
    /// <summary>
    /// 初始化 创建SelfService 校验规则
    /// </summary>
    public TaktSelfServiceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("自助服务名称不能为空")
            .MaximumLength(100).WithMessage("自助服务名称长度不能超过100个字符");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述长度不能超过500个字符");
        RuleFor(x => x.LinkOrCode)
            .MaximumLength(500).WithMessage("链接地址或表单编码长度不能超过500个字符");
        RuleFor(x => x.IconUrl)
            .MaximumLength(500).WithMessage("图标或图片 URL长度不能超过500个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SelfService 验证器
// ========================================

/// <summary>
/// 更新SelfService DTO 验证器
/// </summary>
public class TaktSelfServiceUpdateValidator : AbstractValidator<TaktSelfServiceUpdateDto>
{
    /// <summary>
    /// 初始化 更新SelfService 校验规则
    /// </summary>
    public TaktSelfServiceUpdateValidator()
    {
        RuleFor(x => x.SelfServiceId)
            .GreaterThan(0).WithMessage("SelfServiceID无效");
    }
}

// ========================================
// 导入SelfService 验证器
// ========================================

/// <summary>
/// 导入SelfService DTO 验证器
/// </summary>
public class TaktSelfServiceImportValidator : AbstractValidator<TaktSelfServiceImportDto>
{
    /// <summary>
    /// 初始化 导入SelfService 校验规则
    /// </summary>
    public TaktSelfServiceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("自助服务名称不能为空")
            .MaximumLength(100).WithMessage("自助服务名称长度不能超过100个字符");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.LinkOrCode)
            .MaximumLength(500).WithMessage("链接地址或表单编码长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.LinkOrCode));
        RuleFor(x => x.IconUrl)
            .MaximumLength(500).WithMessage("图标或图片 URL长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.IconUrl));
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
