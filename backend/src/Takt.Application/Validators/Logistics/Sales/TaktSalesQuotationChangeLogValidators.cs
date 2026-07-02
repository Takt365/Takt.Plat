// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesQuotationChangeLogValidators.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesQuotationChangeLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesQuotationChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesQuotationChangeLog 验证器
// ========================================

/// <summary>
/// 创建SalesQuotationChangeLog DTO 验证器
/// </summary>
public class TaktSalesQuotationChangeLogCreateValidator : AbstractValidator<TaktSalesQuotationChangeLogCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesQuotationChangeLog 校验规则
    /// </summary>
    public TaktSalesQuotationChangeLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SalesQuotationId)
            .GreaterThanOrEqualTo(0).WithMessage("销售报价不能为负数");
        RuleFor(x => x.SalesQuotationCode)
            .NotEmpty().WithMessage("销售报价编码不能为空")
            .MaximumLength(50).WithMessage("销售报价编码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesQuotationChangeLog 验证器
// ========================================

/// <summary>
/// 更新SalesQuotationChangeLog DTO 验证器
/// </summary>
public class TaktSalesQuotationChangeLogUpdateValidator : AbstractValidator<TaktSalesQuotationChangeLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesQuotationChangeLog 校验规则
    /// </summary>
    public TaktSalesQuotationChangeLogUpdateValidator()
    {
        RuleFor(x => x.SalesQuotationChangeLogId)
            .GreaterThan(0).WithMessage("SalesQuotationChangeLogID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.SalesQuotationId)
            .GreaterThanOrEqualTo(0).WithMessage("销售报价不能为负数");
        RuleFor(x => x.SalesQuotationCode)
            .NotEmpty().WithMessage("销售报价编码不能为空")
            .MaximumLength(50).WithMessage("销售报价编码长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
