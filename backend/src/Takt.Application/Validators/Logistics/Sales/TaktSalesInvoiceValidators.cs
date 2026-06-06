// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesInvoiceValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesInvoice 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesInvoice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesInvoice 验证器
// ========================================

/// <summary>
/// 创建SalesInvoice DTO 验证器
/// </summary>
public class TaktSalesInvoiceCreateValidator : AbstractValidator<TaktSalesInvoiceCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesInvoice 校验规则
    /// </summary>
    public TaktSalesInvoiceCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.SalesInvoiceCode)
            .NotEmpty().WithMessage("销售发票编码不能为空")
            .MaximumLength(50).WithMessage("销售发票编码长度不能超过50个字符");
        RuleFor(x => x.SalesOrderCode)
            .MaximumLength(50).WithMessage("关联销售订单编码长度不能超过50个字符");
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(50).WithMessage("客户编码长度不能超过50个字符");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(200).WithMessage("客户名称长度不能超过200个字符");
        RuleFor(x => x.TaxInvoiceNo)
            .MaximumLength(50).WithMessage("发票号码长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesInvoice 验证器
// ========================================

/// <summary>
/// 更新SalesInvoice DTO 验证器
/// </summary>
public class TaktSalesInvoiceUpdateValidator : AbstractValidator<TaktSalesInvoiceUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesInvoice 校验规则
    /// </summary>
    public TaktSalesInvoiceUpdateValidator()
    {
        RuleFor(x => x.SalesInvoiceId)
            .GreaterThan(0).WithMessage("SalesInvoiceID无效");
    }
}

// ========================================
// 导入SalesInvoice 验证器
// ========================================

/// <summary>
/// 导入SalesInvoice DTO 验证器
/// </summary>
public class TaktSalesInvoiceImportValidator : AbstractValidator<TaktSalesInvoiceImportDto>
{
    /// <summary>
    /// 初始化 导入SalesInvoice 校验规则
    /// </summary>
    public TaktSalesInvoiceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesInvoiceCode)
            .NotEmpty().WithMessage("销售发票编码不能为空")
            .MaximumLength(50).WithMessage("销售发票编码长度不能超过50个字符");
        RuleFor(x => x.SalesOrderCode)
            .MaximumLength(50).WithMessage("关联销售订单编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.SalesOrderCode));
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(50).WithMessage("客户编码长度不能超过50个字符");
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("客户名称不能为空")
            .MaximumLength(200).WithMessage("客户名称长度不能超过200个字符");
        RuleFor(x => x.TaxInvoiceNo)
            .MaximumLength(50).WithMessage("发票号码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TaxInvoiceNo));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
