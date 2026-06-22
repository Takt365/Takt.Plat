// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktItAssetChangeLogValidators.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ItAssetChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktItAssetChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

// ========================================
// 创建ItAssetChangeLog 验证器
// ========================================

/// <summary>
/// 创建ItAssetChangeLog DTO 验证器
/// </summary>
public class TaktItAssetChangeLogCreateValidator : AbstractValidator<TaktItAssetChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建ItAssetChangeLog 校验规则
    /// </summary>
    public TaktItAssetChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.ItAssetId)
            .GreaterThanOrEqualTo(0).WithMessage("IT 设备保修扩展 ID不能为负数");
        RuleFor(x => x.AssetCode)
            .MaximumLength(40).WithMessage("资产号码长度不能超过40个字符");
        RuleFor(x => x.ChangeSummary)
            .MaximumLength(500).WithMessage("修改内容摘要长度不能超过500个字符");
        RuleFor(x => x.ChangeFields)
            .MaximumLength(4000).WithMessage("变更字段列表长度不能超过4000个字符");
        RuleFor(x => x.ChangeReason)
            .MaximumLength(500).WithMessage("变更原因或备注长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ItAssetChangeLog 验证器
// ========================================

/// <summary>
/// 更新ItAssetChangeLog DTO 验证器
/// </summary>
public class TaktItAssetChangeLogUpdateValidator : AbstractValidator<TaktItAssetChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新ItAssetChangeLog 校验规则
    /// </summary>
    public TaktItAssetChangeLogUpdateValidator()
    {
        RuleFor(x => x.ItAssetChangeLogId)
            .GreaterThan(0).WithMessage("ItAssetChangeLogID无效");
    }
}
