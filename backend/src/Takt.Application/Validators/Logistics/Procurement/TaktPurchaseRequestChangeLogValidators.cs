// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchaseRequestChangeLogValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseRequestChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseRequestChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchaseRequestChangeLog 验证器
// ========================================

/// <summary>
/// 创建PurchaseRequestChangeLog DTO 验证器
/// </summary>
public class TaktPurchaseRequestChangeLogCreateValidator : AbstractValidator<TaktPurchaseRequestChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseRequestChangeLog 校验规则
    /// </summary>
    public TaktPurchaseRequestChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("采购申请 ID不能为负数");
        RuleFor(x => x.RequestCode)
            .NotEmpty().WithMessage("申请编码不能为空")
            .MaximumLength(50).WithMessage("申请编码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseRequestChangeLog 验证器
// ========================================

/// <summary>
/// 更新PurchaseRequestChangeLog DTO 验证器
/// </summary>
public class TaktPurchaseRequestChangeLogUpdateValidator : AbstractValidator<TaktPurchaseRequestChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseRequestChangeLog 校验规则
    /// </summary>
    public TaktPurchaseRequestChangeLogUpdateValidator()
    {
        RuleFor(x => x.PurchaseRequestChangeLogId)
            .GreaterThan(0).WithMessage("PurchaseRequestChangeLogID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("采购申请 ID不能为负数");
        RuleFor(x => x.RequestCode)
            .NotEmpty().WithMessage("申请编码不能为空")
            .MaximumLength(50).WithMessage("申请编码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
