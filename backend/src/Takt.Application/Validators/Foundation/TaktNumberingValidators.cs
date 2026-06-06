// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktNumberingValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Numbering 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktNumbering 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Numbering 验证器
// ========================================

/// <summary>
/// 创建Numbering DTO 验证器
/// </summary>
public class TaktNumberingCreateValidator : AbstractValidator<TaktNumberingCreateDto>
{
    /// <summary>
    /// 初始化 创建Numbering 校验规则
    /// </summary>
    public TaktNumberingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.RuleCode)
            .NotEmpty().WithMessage("规则编码不能为空")
            .MaximumLength(50).WithMessage("规则编码长度不能超过50个字符");
        RuleFor(x => x.RuleName)
            .NotEmpty().WithMessage("规则名称不能为空")
            .MaximumLength(100).WithMessage("规则名称长度不能超过100个字符");
        RuleFor(x => x.DocumentType)
            .IsInEnum().WithMessage("单据类型无效");
        RuleFor(x => x.DepartmentCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(50).WithMessage("部门编码长度不能超过50个字符");
        RuleFor(x => x.Prefix)
            .MaximumLength(20).WithMessage("前缀长度不能超过20个字符");
        RuleFor(x => x.DateFormat)
            .MaximumLength(20).WithMessage("日期格式长度不能超过20个字符");
        RuleFor(x => x.Suffix)
            .MaximumLength(20).WithMessage("后缀长度不能超过20个字符");
        RuleFor(x => x.ResetPeriod)
            .NotEmpty().WithMessage("重置周期不能为空")
            .MaximumLength(20).WithMessage("重置周期长度不能超过20个字符");
        RuleFor(x => x.ExampleCode)
            .MaximumLength(100).WithMessage("示例编码长度不能超过100个字符");
        RuleFor(x => x.Separator)
            .NotEmpty().WithMessage("分隔符不能为空")
            .MaximumLength(1).WithMessage("分隔符长度不能超过1个字符");
        RuleFor(x => x.IsBuiltIn)
            .IsInEnum().WithMessage("是否内置无效");
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("状态无效");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述说明；可选配置编码段顺序，格式：segments:DocumentType,长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Numbering 验证器
// ========================================

/// <summary>
/// 更新Numbering DTO 验证器
/// </summary>
public class TaktNumberingUpdateValidator : AbstractValidator<TaktNumberingUpdateDto>
{
    /// <summary>
    /// 初始化 更新Numbering 校验规则
    /// </summary>
    public TaktNumberingUpdateValidator()
    {
        RuleFor(x => x.NumberingId)
            .GreaterThan(0).WithMessage("NumberingID无效");
    }
}

// ========================================
// 导入Numbering 验证器
// ========================================

/// <summary>
/// 导入Numbering DTO 验证器
/// </summary>
public class TaktNumberingImportValidator : AbstractValidator<TaktNumberingImportDto>
{
    /// <summary>
    /// 初始化 导入Numbering 校验规则
    /// </summary>
    public TaktNumberingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.RuleCode)
            .NotEmpty().WithMessage("规则编码不能为空")
            .MaximumLength(50).WithMessage("规则编码长度不能超过50个字符");
        RuleFor(x => x.RuleName)
            .NotEmpty().WithMessage("规则名称不能为空")
            .MaximumLength(100).WithMessage("规则名称长度不能超过100个字符");
        RuleFor(x => x.DocumentType)
            .IsInEnum().WithMessage("单据类型无效");
        RuleFor(x => x.DepartmentCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(50).WithMessage("部门编码长度不能超过50个字符");
        RuleFor(x => x.Prefix)
            .MaximumLength(20).WithMessage("前缀长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.Prefix));
        RuleFor(x => x.DateFormat)
            .MaximumLength(20).WithMessage("日期格式长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.DateFormat));
        RuleFor(x => x.Suffix)
            .MaximumLength(20).WithMessage("后缀长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.Suffix));
        RuleFor(x => x.ResetPeriod)
            .NotEmpty().WithMessage("重置周期不能为空")
            .MaximumLength(20).WithMessage("重置周期长度不能超过20个字符");
        RuleFor(x => x.ExampleCode)
            .MaximumLength(100).WithMessage("示例编码长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.ExampleCode));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
