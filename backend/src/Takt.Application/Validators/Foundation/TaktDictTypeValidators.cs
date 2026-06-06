// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktDictTypeValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：DictType 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDictType 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建DictType 验证器
// ========================================

/// <summary>
/// 创建DictType DTO 验证器
/// </summary>
public class TaktDictTypeCreateValidator : AbstractValidator<TaktDictTypeCreateDto>
{
    /// <summary>
    /// 初始化 创建DictType 校验规则
    /// </summary>
    public TaktDictTypeCreateValidator()
    {
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空")
            .MaximumLength(50).WithMessage("字典类型编码长度不能超过50个字符");
        RuleFor(x => x.DictTypeName)
            .NotEmpty().WithMessage("字典类型名称不能为空")
            .MaximumLength(100).WithMessage("字典类型名称长度不能超过100个字符");
        RuleFor(x => x.DataSource)
            .IsInEnum().WithMessage("数据源无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.IsBuiltIn)
            .IsInEnum().WithMessage("是否内置无效");
        RuleFor(x => x.DictStatus)
            .IsInEnum().WithMessage("状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新DictType 验证器
// ========================================

/// <summary>
/// 更新DictType DTO 验证器
/// </summary>
public class TaktDictTypeUpdateValidator : AbstractValidator<TaktDictTypeUpdateDto>
{
    /// <summary>
    /// 初始化 更新DictType 校验规则
    /// </summary>
    public TaktDictTypeUpdateValidator()
    {
        RuleFor(x => x.DictTypeId)
            .GreaterThan(0).WithMessage("DictTypeID无效");
    }
}

// ========================================
// 导入DictType 验证器
// ========================================

/// <summary>
/// 导入DictType DTO 验证器
/// </summary>
public class TaktDictTypeImportValidator : AbstractValidator<TaktDictTypeImportDto>
{
    /// <summary>
    /// 初始化 导入DictType 校验规则
    /// </summary>
    public TaktDictTypeImportValidator()
    {
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空")
            .MaximumLength(50).WithMessage("字典类型编码长度不能超过50个字符");
        RuleFor(x => x.DictTypeName)
            .NotEmpty().WithMessage("字典类型名称不能为空")
            .MaximumLength(100).WithMessage("字典类型名称长度不能超过100个字符");
        RuleFor(x => x.DataSource)
            .IsInEnum().WithMessage("数据源无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.IsBuiltIn)
            .IsInEnum().WithMessage("是否内置无效");
        RuleFor(x => x.DictStatus)
            .IsInEnum().WithMessage("状态无效");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
