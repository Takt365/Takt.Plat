// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.CustomerService
// 文件名称：TaktServiceTicketValidators.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：ServiceTicket 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktServiceTicket 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.CustomerService;

namespace Takt.Application.Validators.Logistics.CustomerService;

// ========================================
// 创建ServiceTicket 验证器
// ========================================

/// <summary>
/// 创建ServiceTicket DTO 验证器
/// </summary>
public class TaktServiceTicketCreateValidator : AbstractValidator<TaktServiceTicketCreateDto>
{
    /// <summary>
    /// 初始化 创建ServiceTicket 校验规则
    /// </summary>
    public TaktServiceTicketCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceTicketCode)
            .NotEmpty().WithMessage("服务工单编码不能为空")
            .MaximumLength(50).WithMessage("服务工单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.ServiceOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务订单ID不能为负数");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.TicketSubject)
            .NotEmpty().WithMessage("工单主题不能为空")
            .MaximumLength(200).WithMessage("工单主题长度不能超过200个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("指派服务人员工ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ServiceTicket 验证器
// ========================================

/// <summary>
/// 更新ServiceTicket DTO 验证器
/// </summary>
public class TaktServiceTicketUpdateValidator : AbstractValidator<TaktServiceTicketUpdateDto>
{
    /// <summary>
    /// 初始化 更新ServiceTicket 校验规则
    /// </summary>
    public TaktServiceTicketUpdateValidator()
    {
        RuleFor(x => x.ServiceTicketId)
            .GreaterThan(0).WithMessage("ServiceTicketID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceTicketCode)
            .NotEmpty().WithMessage("服务工单编码不能为空")
            .MaximumLength(50).WithMessage("服务工单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.ServiceOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务订单ID不能为负数");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.TicketSubject)
            .NotEmpty().WithMessage("工单主题不能为空")
            .MaximumLength(200).WithMessage("工单主题长度不能超过200个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("指派服务人员工ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ServiceTicket 验证器
// ========================================

/// <summary>
/// 导入ServiceTicket DTO 验证器
/// </summary>
public class TaktServiceTicketImportValidator : AbstractValidator<TaktServiceTicketImportDto>
{
    /// <summary>
    /// 初始化 导入ServiceTicket 校验规则
    /// </summary>
    public TaktServiceTicketImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceTicketCode)
            .NotEmpty().WithMessage("服务工单编码不能为空")
            .MaximumLength(50).WithMessage("服务工单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.ServiceOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务订单ID不能为负数");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.TicketSubject)
            .NotEmpty().WithMessage("工单主题不能为空")
            .MaximumLength(200).WithMessage("工单主题长度不能超过200个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("指派服务人员工ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
