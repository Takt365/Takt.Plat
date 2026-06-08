// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Attendance
// 文件名称：TaktLeaveValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：Leave 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktLeave 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Attendance;

namespace Takt.Application.Validators.HumanResource.Attendance;

// ========================================
// 创建Leave 验证器
// ========================================

/// <summary>
/// 创建Leave DTO 验证器
/// </summary>
public class TaktLeaveCreateValidator : AbstractValidator<TaktLeaveCreateDto>
{
    /// <summary>
    /// 初始化 创建Leave 校验规则
    /// </summary>
    public TaktLeaveCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(50).WithMessage("员工姓名长度不能超过50个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("部门 ID不能为负数");
        RuleFor(x => x.DeptName)
            .MaximumLength(100).WithMessage("部门名称长度不能超过100个字符");
        RuleFor(x => x.LeaveType)
            .NotEmpty().WithMessage("请假类型不能为空")
            .MaximumLength(50).WithMessage("请假类型长度不能超过50个字符");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("请假事由不能为空")
            .MaximumLength(500).WithMessage("请假事由长度不能超过500个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.HandlingComment)
            .MaximumLength(500).WithMessage("经办备注长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Leave 验证器
// ========================================

/// <summary>
/// 更新Leave DTO 验证器
/// </summary>
public class TaktLeaveUpdateValidator : AbstractValidator<TaktLeaveUpdateDto>
{
    /// <summary>
    /// 初始化 更新Leave 校验规则
    /// </summary>
    public TaktLeaveUpdateValidator()
    {
        RuleFor(x => x.LeaveId)
            .GreaterThan(0).WithMessage("LeaveID无效");
    }
}

// ========================================
// 导入Leave 验证器
// ========================================

/// <summary>
/// 导入Leave DTO 验证器
/// </summary>
public class TaktLeaveImportValidator : AbstractValidator<TaktLeaveImportDto>
{
    /// <summary>
    /// 初始化 导入Leave 校验规则
    /// </summary>
    public TaktLeaveImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(50).WithMessage("员工姓名长度不能超过50个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("部门 ID不能为负数");
        RuleFor(x => x.DeptName)
            .MaximumLength(100).WithMessage("部门名称长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.DeptName));
        RuleFor(x => x.LeaveType)
            .NotEmpty().WithMessage("请假类型不能为空")
            .MaximumLength(50).WithMessage("请假类型长度不能超过50个字符");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("请假事由不能为空")
            .MaximumLength(500).WithMessage("请假事由长度不能超过500个字符");
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.FlowInstanceId)
            .GreaterThanOrEqualTo(0).WithMessage("流程实例 ID不能为负数");
        RuleFor(x => x.HandlingComment)
            .MaximumLength(500).WithMessage("经办备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.HandlingComment));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
