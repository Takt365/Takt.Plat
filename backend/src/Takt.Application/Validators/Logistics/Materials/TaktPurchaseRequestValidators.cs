// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktPurchaseRequestValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseRequest 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseRequest 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建PurchaseRequest 验证器
// ========================================

/// <summary>
/// 创建PurchaseRequest DTO 验证器
/// </summary>
public class TaktPurchaseRequestCreateValidator : AbstractValidator<TaktPurchaseRequestCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseRequest 校验规则
    /// </summary>
    public TaktPurchaseRequestCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.PurchaseRequestCode)
            .NotEmpty().WithMessage("采购申请编码不能为空")
            .MaximumLength(10).WithMessage("采购申请编码长度不能超过10个字符");
        RuleFor(x => x.RequestId)
            .GreaterThanOrEqualTo(0).WithMessage("申请人员工ID不能为负数");
        RuleFor(x => x.RequestBy)
            .NotEmpty().WithMessage("申请人不能为空")
            .MaximumLength(50).WithMessage("申请人长度不能超过50个字符");
        RuleFor(x => x.RequestStatus)
            .IsInEnum().WithMessage("申请状态无效");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例ID不能为负数");
        RuleFor(x => x.RequestReason)
            .MaximumLength(1000).WithMessage("申请原因长度不能超过1000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseRequest 验证器
// ========================================

/// <summary>
/// 更新PurchaseRequest DTO 验证器
/// </summary>
public class TaktPurchaseRequestUpdateValidator : AbstractValidator<TaktPurchaseRequestUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseRequest 校验规则
    /// </summary>
    public TaktPurchaseRequestUpdateValidator()
    {
        RuleFor(x => x.PurchaseRequestId)
            .GreaterThan(0).WithMessage("PurchaseRequestID无效");
    }
}

// ========================================
// 导入PurchaseRequest 验证器
// ========================================

/// <summary>
/// 导入PurchaseRequest DTO 验证器
/// </summary>
public class TaktPurchaseRequestImportValidator : AbstractValidator<TaktPurchaseRequestImportDto>
{
    /// <summary>
    /// 初始化 导入PurchaseRequest 校验规则
    /// </summary>
    public TaktPurchaseRequestImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(50).WithMessage("工厂代码长度不能超过50个字符");
        RuleFor(x => x.PurchaseRequestCode)
            .NotEmpty().WithMessage("采购申请编码不能为空")
            .MaximumLength(10).WithMessage("采购申请编码长度不能超过10个字符");
        RuleFor(x => x.RequestId)
            .GreaterThanOrEqualTo(0).WithMessage("申请人员工ID不能为负数");
        RuleFor(x => x.RequestBy)
            .NotEmpty().WithMessage("申请人不能为空")
            .MaximumLength(50).WithMessage("申请人长度不能超过50个字符");
        RuleFor(x => x.RequestStatus)
            .IsInEnum().WithMessage("申请状态无效");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例ID不能为负数");
        RuleFor(x => x.RequestReason)
            .MaximumLength(1000).WithMessage("申请原因长度不能超过1000个字符").When(x => !string.IsNullOrWhiteSpace(x.RequestReason));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
