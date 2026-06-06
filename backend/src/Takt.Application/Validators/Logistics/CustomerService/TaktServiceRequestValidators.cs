// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.CustomerService
// 文件名称：TaktServiceRequestValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：ServiceRequest 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktServiceRequest 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.CustomerService;

namespace Takt.Application.Validators.Logistics.CustomerService;

// ========================================
// 创建ServiceRequest 验证器
// ========================================

/// <summary>
/// 创建ServiceRequest DTO 验证器
/// </summary>
public class TaktServiceRequestCreateValidator : AbstractValidator<TaktServiceRequestCreateDto>
{
    /// <summary>
    /// 初始化 创建ServiceRequest 校验规则
    /// </summary>
    public TaktServiceRequestCreateValidator()
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
        RuleFor(x => x.ServiceRequestCode)
            .NotEmpty().WithMessage("服务请求单号不能为空")
            .MaximumLength(50).WithMessage("服务请求单号长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.ServiceContractCode)
            .MaximumLength(50).WithMessage("关联服务合同编码长度不能超过50个字符");
        RuleFor(x => x.RequestSubject)
            .NotEmpty().WithMessage("请求主题不能为空")
            .MaximumLength(200).WithMessage("请求主题长度不能超过200个字符");
        RuleFor(x => x.RequestDescription)
            .NotEmpty().WithMessage("请求描述不能为空")
            .MaximumLength(2000).WithMessage("请求描述长度不能超过2000个字符");
        RuleFor(x => x.ContactPerson)
            .MaximumLength(50).WithMessage("联系人长度不能超过50个字符");
        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("联系电话长度不能超过50个字符");
        RuleFor(x => x.ContactEmail)
            .MaximumLength(100).WithMessage("联系邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.ServiceAddress)
            .MaximumLength(500).WithMessage("服务地址长度不能超过500个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("受理人员工ID不能为负数");
        RuleFor(x => x.AssignedEmployeeName)
            .MaximumLength(50).WithMessage("受理人姓名长度不能超过50个字符");
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("排序号不能为负数");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ServiceRequest 验证器
// ========================================

/// <summary>
/// 更新ServiceRequest DTO 验证器
/// </summary>
public class TaktServiceRequestUpdateValidator : AbstractValidator<TaktServiceRequestUpdateDto>
{
    /// <summary>
    /// 初始化 更新ServiceRequest 校验规则
    /// </summary>
    public TaktServiceRequestUpdateValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .GreaterThan(0).WithMessage("ServiceRequestID无效");
    }
}

// ========================================
// 导入ServiceRequest 验证器
// ========================================

/// <summary>
/// 导入ServiceRequest DTO 验证器
/// </summary>
public class TaktServiceRequestImportValidator : AbstractValidator<TaktServiceRequestImportDto>
{
    /// <summary>
    /// 初始化 导入ServiceRequest 校验规则
    /// </summary>
    public TaktServiceRequestImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceRequestCode)
            .NotEmpty().WithMessage("服务请求单号不能为空")
            .MaximumLength(50).WithMessage("服务请求单号长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(80).WithMessage("客户端名称长度不能超过80个字符");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.ServiceContractCode)
            .MaximumLength(50).WithMessage("关联服务合同编码长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.ServiceContractCode));
        RuleFor(x => x.RequestSubject)
            .NotEmpty().WithMessage("请求主题不能为空")
            .MaximumLength(200).WithMessage("请求主题长度不能超过200个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
