// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Statistics.Report
// 文件名称：TaktConfigurableSourceValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableSource 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktConfigurableSource 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Report;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Statistics.Report;

// ========================================
// 创建ConfigurableSource 验证器
// ========================================

/// <summary>
/// 创建ConfigurableSource DTO 验证器
/// </summary>
public class TaktConfigurableSourceCreateValidator : AbstractValidator<TaktConfigurableSourceCreateDto>
{
    /// <summary>
    /// 初始化 创建ConfigurableSource 校验规则
    /// </summary>
    public TaktConfigurableSourceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.ConfigurableId)
            .GreaterThanOrEqualTo(0).WithMessage("关联报表主表 ID不能为负数");
        RuleFor(x => x.SourceAlias)
            .NotEmpty().WithMessage("数据源别名不能为空")
            .MaximumLength(10).WithMessage("数据源别名长度不能超过10个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("物理表名不能为空")
            .MaximumLength(40).WithMessage("物理表名长度不能超过40个字符");
        RuleFor(x => x.IsPrimary)
            .IsInEnum().WithMessage("是否主表无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ConfigurableSource 验证器
// ========================================

/// <summary>
/// 更新ConfigurableSource DTO 验证器
/// </summary>
public class TaktConfigurableSourceUpdateValidator : AbstractValidator<TaktConfigurableSourceUpdateDto>
{
    /// <summary>
    /// 初始化 更新ConfigurableSource 校验规则
    /// </summary>
    public TaktConfigurableSourceUpdateValidator()
    {
        RuleFor(x => x.ConfigurableSourceId)
            .GreaterThan(0).WithMessage("ConfigurableSourceID无效");
    }
}

// ========================================
// 导入ConfigurableSource 验证器
// ========================================

/// <summary>
/// 导入ConfigurableSource DTO 验证器
/// </summary>
public class TaktConfigurableSourceImportValidator : AbstractValidator<TaktConfigurableSourceImportDto>
{
    /// <summary>
    /// 初始化 导入ConfigurableSource 校验规则
    /// </summary>
    public TaktConfigurableSourceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.ConfigurableId)
            .GreaterThanOrEqualTo(0).WithMessage("关联报表主表 ID不能为负数");
        RuleFor(x => x.SourceAlias)
            .NotEmpty().WithMessage("数据源别名不能为空")
            .MaximumLength(10).WithMessage("数据源别名长度不能超过10个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("物理表名不能为空")
            .MaximumLength(40).WithMessage("物理表名长度不能超过40个字符");
        RuleFor(x => x.IsPrimary)
            .IsInEnum().WithMessage("是否主表无效");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
