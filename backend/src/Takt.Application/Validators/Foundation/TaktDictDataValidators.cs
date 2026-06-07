// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktDictDataValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：DictData 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDictData 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建DictData 验证器
// ========================================

/// <summary>
/// 创建DictData DTO 验证器
/// </summary>
public class TaktDictDataCreateValidator : AbstractValidator<TaktDictDataCreateDto>
{
    /// <summary>
    /// 初始化 创建DictData 校验规则
    /// </summary>
    public TaktDictDataCreateValidator()
    {
        RuleFor(x => x.DictTypeId)
            .GreaterThanOrEqualTo(0).WithMessage("字典类型ID不能为负数");
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空")
            .MaximumLength(50).WithMessage("字典类型编码长度不能超过50个字符");
        RuleFor(x => x.DictLabel)
            .NotEmpty().WithMessage("字典项标签不能为空")
            .MaximumLength(100).WithMessage("字典项标签长度不能超过100个字符");
        RuleFor(x => x.DictValue)
            .NotEmpty().WithMessage("字典项值不能为空")
            .MaximumLength(100).WithMessage("字典项值长度不能超过100个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化翻译键不能为空")
            .MaximumLength(200).WithMessage("国际化翻译键长度不能超过200个字符");
        RuleFor(x => x.ExtLabel)
            .MaximumLength(200).WithMessage("扩展标签长度不能超过200个字符");
        RuleFor(x => x.ExtValue)
            .MaximumLength(200).WithMessage("扩展值长度不能超过200个字符");
        RuleFor(x => x.IsDefault)
            .IsInEnum().WithMessage("是否默认项无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新DictData 验证器
// ========================================

/// <summary>
/// 更新DictData DTO 验证器
/// </summary>
public class TaktDictDataUpdateValidator : AbstractValidator<TaktDictDataUpdateDto>
{
    /// <summary>
    /// 初始化 更新DictData 校验规则
    /// </summary>
    public TaktDictDataUpdateValidator()
    {
        RuleFor(x => x.DictDataId)
            .GreaterThan(0).WithMessage("DictDataID无效");
    }
}

// ========================================
// 导入DictData 验证器
// ========================================

/// <summary>
/// 导入DictData DTO 验证器
/// </summary>
public class TaktDictDataImportValidator : AbstractValidator<TaktDictDataImportDto>
{
    /// <summary>
    /// 初始化 导入DictData 校验规则
    /// </summary>
    public TaktDictDataImportValidator()
    {
        RuleFor(x => x.DictTypeId)
            .GreaterThanOrEqualTo(0).WithMessage("字典类型ID不能为负数");
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空")
            .MaximumLength(50).WithMessage("字典类型编码长度不能超过50个字符");
        RuleFor(x => x.DictLabel)
            .NotEmpty().WithMessage("字典项标签不能为空")
            .MaximumLength(100).WithMessage("字典项标签长度不能超过100个字符");
        RuleFor(x => x.DictValue)
            .NotEmpty().WithMessage("字典项值不能为空")
            .MaximumLength(100).WithMessage("字典项值长度不能超过100个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化翻译键不能为空")
            .MaximumLength(200).WithMessage("国际化翻译键长度不能超过200个字符");
        RuleFor(x => x.ExtLabel)
            .MaximumLength(200).WithMessage("扩展标签长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtLabel));
        RuleFor(x => x.ExtValue)
            .MaximumLength(200).WithMessage("扩展值长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtValue));
        RuleFor(x => x.IsDefault)
            .IsInEnum().WithMessage("是否默认项无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
